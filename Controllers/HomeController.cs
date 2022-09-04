using System.Diagnostics;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using newMantis.Models;
using newMantis.Infrastructure;
using newMantis.Configuration;

namespace newMantis.Controllers;

[CustomAuthFilter]
[CustomFirstConnectionFilter]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly newMantisContext _context;
    private readonly IMantisConfigManager _configuration;

    public HomeController(ILogger<HomeController> logger, newMantisContext context, IMantisConfigManager configuration)
    {
        _logger = logger;
        _context = context;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return RedirectToAction("General");
    }

    /**
        * Méthode Général
        *
        * Permet d'afficher l'onglet Général. On récupère via l'API la liste de tous les projets disponibles.
        * On traite cette liste pour rétablir les relations père-fils entre les projets pour faliciter l'affichage sous forme d'un arbre.
        * On envoit alors un Json qui sera traiter dans la vue avec une fonction javascript. 
        *
        * Créé : 07/2022 Soleilhavoup
    */

    public IActionResult General()
    {
        CallMantisAPI apiMantis = new CallMantisAPI(_configuration);
        Projet allProject = new Projet();
        List<TreeViewNode> nodes = new List<TreeViewNode>();
        
        allProject = apiMantis.getAllProjects().Result;

        int cpt = 0;

        /* Méthode TreeView */

        if(allProject.projects.Count > 0)
        {
            
            for (int i = 0; i <allProject.projects.Count; i++)
            {
                if(allProject.projects[i].subprojects == null)
                {
                    nodes.Add(new TreeViewNode {id = allProject.projects[i].id, parent = "#", text = allProject.projects[i].name});
                    allProject.projects.Remove(allProject.projects[i]);
                    i--;
                }
            }

            while(allProject.projects.Count>0)
            {
                if(cpt >= allProject.projects.Count)
                {
                    cpt = 0;
                }
                int subCount = allProject.projects[cpt].subprojects.Count;
                int count = 0;
                foreach(projects subprojet in allProject.projects[cpt].subprojects)
                {
                    if (nodes.Find(s => s.id == subprojet.id) != null )
                    {
                        TreeViewNode nodeIndex = nodes.Find(s => s.id == subprojet.id)!;
                        TreeViewNode node = new TreeViewNode {id = nodeIndex.id, parent = allProject.projects[cpt].id.ToString(), text = nodeIndex.text};
                        int index = nodes.FindIndex(n => n.id == node.id);
                        if(index != -1)
                        {
                            nodes[index] = node;
                        }
                        else
                        {
                            nodes.Add(node);
                        }
                        count ++;
                        if (count == subCount)
                        {
                            nodes.Add(new TreeViewNode{id = allProject.projects[cpt].id, parent = "#", text = allProject.projects[cpt].name});
                            allProject.projects.Remove(allProject.projects[cpt]);
                        }
                    }
                }
                cpt ++;
            }
        }

        ViewBag.Json = JsonConvert.SerializeObject(nodes);

        return View();
    }

    /**
        * Méthode SendToCSV
        *
        * Permet d'envoyer toutes les données à la classe Excel pour générer le bilan. 
        *
        * @param type_client : Type string. Permet de savoir si on veut des client externe ou interne. 
        *
        * @param type_client : Type string. Permet de savoir si on veut des client externe ou interne. 
        *
        * @param list_projets : Type string[]. Contient la liste de id des projets que l'on souhaite intégrer dans le bilan.
        *
        * @param datedeb : Type DateTime. Date de début pour prendre en compte les issues si elle est demandée.
        *
        * @param datefi : Type DateTime. Date de fin pour prendre en compte les issues si elle est demandée.
        *
        * Créé : 05/2022 Soleilhavoup
    */

    public void sendToCSV(string type_client, bool est_detaille, string[] list_projets, DateTime datedeb, DateTime datefi)
    {
        CallMantisAPI api = new CallMantisAPI(_configuration);

        ExcelController excel = new ExcelController(_configuration);

        Projet[] projects = new Projet[list_projets.Length];

        for(int i = 0; i < projects.Length; i++)
        {
            Projet project = api.getAProject(int.Parse(list_projets[i])).Result;
            
            projects[i] = project;
        }

        excel.CreateCSVFile(type_client, est_detaille, projects, datedeb, datefi);
    }

    /**
        * Méthode Admin
        *
        * Permet d'afficher l'onglet Admin avec les différents formulaires demandés. Un formulaire pour renseigner les temps de chaque
        * type d'issues, et un pour renseigner les catégories que l'utilisateur souhaite intégrer dans le bilan. 
        *
        * Créé : 05/2022 Soleilhavoup
    */

    [HttpGet]
    public IActionResult Admin()
    {
        ListeCat model = new ListeCat();
        string ?currentUser = this.HttpContext.Session.GetString("email");
        Utilisateur ?user = _context.Utilisateur.Where(x => x.email == currentUser).FirstOrDefault();
        Temps temps = new Temps();
        temps.tempsCritique = user.tempsCritique;
        temps.tempsMajeur = user.tempsMajeur;
        temps.tempsMineur = user.tempsMineur;
        temps.tempsJour = user.tempsJour;
        var categories =    (from c in _context.Categorie
                            join u in _context.UtilisateurCategorie
                            on c.idCategorie equals u.idCategorie
                            where u.email == currentUser
                            select c).ToList();
        var tmpCategories = _context.Categorie.ToList();
        List<Categorie> AllCategories = new List<Categorie>();
        model.Temps = temps;
        for(int i=0; i<tmpCategories.Count(); i++)
        {
            if(categories.ToList().FirstOrDefault(c => c.idCategorie == tmpCategories[i].idCategorie) == null )
            {
                AllCategories.Add(tmpCategories[i]);
            }
        }
        model.Categories = categories;
        model.AllCategories = AllCategories;
        return View(model);
    }

    /**
        * Méthode Admin - Post
        *
        * Permet de valider le formulaire pour enregistrer les temps renseigner dans le formulaire. On les sauvegarde dans la base de données.
        *
        * @param temps : Type Temps. Permet de modéliser les différents temps existants (Critique, Majeur, Mineur, Jour).
        * Contient les valeurs entrées par l'utilisateur dans le formulaire.
        *
        * Créé : 05/2022 Soleilhavoup
    */

    [HttpPost]
    public async Task<IActionResult> Admin(Temps temps)
    {
        if(ModelState.IsValid)
        {
            Utilisateur updateUser = new Utilisateur();
            updateUser = _context.Utilisateur.Where(u => u.email == this.HttpContext.Session.GetString("email")).FirstOrDefault();
            updateUser.tempsMineur = temps.tempsMineur;
            updateUser.tempsMajeur = temps.tempsMajeur;
            updateUser.tempsCritique = temps.tempsCritique;
            updateUser.tempsJour = temps.tempsJour;
            try
            {
                _context.Utilisateur.Update(updateUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateurExists(updateUser.email))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            ViewBag.Message = "Les données ont été mises à jours";
    }
        else
        {
            ViewBag.Message = "Il y a eu une erreur dans le formulaire";
        }
        return RedirectToAction(nameof(Admin));
    }

    /**
        * Méthode AddCategorie
        *
        * Permet d'ajouter une catégorie dans la liste de l'utilisateur. On affiche dans un select les catégories existantes en enlevant
        * celle déjà sélectionnées par l'utilisateur. Lorsque qu'il clique sur une option, la catégorie est directement enregistrée dans la 
        * base de données. 
        *
        * @param id : Type string. Identifiant de la catégorie que l'utilisateur souhaite ajouter à sa liste. 
        *
        * Créé : 05/2022 Soleilhavoup
    */

    [HttpPost]
    public async Task<JsonResult> AddCategorie([FromBody]string id)
    {
        int idCat = int.Parse(id);
        UtilisateurCategorie UCat = new UtilisateurCategorie();
        UCat.idCategorie = idCat;
        UCat.email = this.HttpContext.Session.GetString("email");
        try
        {
            _context.UtilisateurCategorie.Add(UCat);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {

        }
        return Json(UCat);
    }

    /**
        * Méthode RemoveCategorie
        *
        * Permet de supprimer une catégorie dans la liste de l'utilisateur. On affiche dans un tableau les catégories déjà sélectionnées 
        * par l'utilisateur. Lorsque qu'il clique, la catégorie est supprimée de la base de données. 
        *
        * @param id : Type string. Identifiant de la catégorie que l'utilisateur souhaite supprimer de sa liste. 
        *
        * Créé : 05/2022 Soleilhavoup
    */

    public async Task<IActionResult> RemoveCategorie(string id)
    {
        int idCat = int.Parse(id);
        string ?currentUser = this.HttpContext.Session.GetString("email");
        UtilisateurCategorie UCat = new UtilisateurCategorie();
        UCat.idCategorie = idCat;
        UCat.email = currentUser;
        try
        {
            _context.UtilisateurCategorie.Remove(UCat);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {

        }
        return RedirectToAction(nameof(Admin));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool UtilisateurExists(string mail)
    {
        return _context.Utilisateur.Any(e => e.email == mail);
    }
}
