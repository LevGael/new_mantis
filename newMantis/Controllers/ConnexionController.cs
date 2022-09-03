using Microsoft.AspNetCore.Mvc;
using newMantis.Models;

namespace newMantis.Controllers;

public class ConnexionController : Controller
{
    private readonly newMantisContext _context;

    public ConnexionController(newMantisContext context)
    {
        _context = context;
    }

    /**
        * Méthode Index - Get
        *
        * Cette méthode permet d'afficher le formulaire de connexion en vérifiant si personne n'est déja connecté. 
        *
        * Créé : 05/2022 Soleilhavoup
    */

    [HttpGet]
    public IActionResult Index()
    {
        if (this.HttpContext.Session.GetString("email") != null)
        {
            return Redirect("/");
        }
        Compte login = new Compte();
        return View(login);
    }

    /**
        * Méthode Index - Post
        *
        * Cette méthode permet à l'utilisateur de s'authentifier. Elle vérifie si le formulaire de connexion est bien rempli. Puis on
        * on récupère dans la base de données le compte qui correspond à l'adresse mail qui est demandée dans le formulaire. Enfin on
        * compare les mots de passes. Si tout est bon, on enregistre le profil dans les sessions et l'utilisateur peut accéder à l'application.
        * Sinon, on affiche un message d'erreur et on reste sur la même page. 
        *
        * @param login  Type Compte. Il contient le mail et le mot de passe rentré dans le formulaire. 
        *
        * Créé : 05/2022 Soleilhavoup
    */

    [HttpPost]
    public IActionResult Index(Compte login)
    {

        if (ModelState.IsValid)
        {
            var status = _context.Utilisateur.Where(u => u.email == login.Email).FirstOrDefault();

            if (status != null)
            {
                if (BCrypt.Net.BCrypt.Verify(login.Motdepasse, status.mdp))
                {
                    if (status.firstconnexion == 1)
                    {
                        this.HttpContext.Session.SetString("email", status.email);
                        this.HttpContext.Session.SetString("role", status.role.ToString());
                        return Redirect("/");
                    } else
                    {
                        this.HttpContext.Session.SetString("email", status.email);
                        this.HttpContext.Session.SetString("isfirst", status.firstconnexion.ToString());
                        return RedirectToAction(nameof(Premiere_connexion));
                    }
                }
                else {
                    ViewBag.Message = "Identifiants incorrects";
                }
            }
            else
            {
                ViewBag.Message = "Identifiants incorrects";
            }
            return View(login);
        }
        else
        {
            return View(login);
        }
    }

    [HttpGet]
    public IActionResult Premiere_connexion()
    {

        NouveauMdp NMDP = new NouveauMdp();
        return View(NMDP);
    }

    [HttpPost]
    public async Task<IActionResult> Premiere_connexion(NouveauMdp NMDP)
    {
        var memlogin = this.HttpContext.Session.GetString("email");
        var status = _context.Utilisateur.Where(u => u.email == memlogin).FirstOrDefault();
        if(status != null)
        {
            if (BCrypt.Net.BCrypt.Verify(NMDP.AncienMotdepasse, status.mdp))
            {
                if (NMDP.NouveauMotdepasse == NMDP.RepeatMotdepasse)
                {
                    status.mdp = BCrypt.Net.BCrypt.HashPassword(NMDP.NouveauMotdepasse);
                    status.firstconnexion = 1;
                    _context.Utilisateur.Update(status);
                    await _context.SaveChangesAsync();
                    this.HttpContext.Session.SetString("role", status.role.ToString());
                    this.HttpContext.Session.Remove("isfirst");
                    return Redirect("/");
                }
                else
                {
                    ViewBag.Message = "Les nouveaux mots de passe ne correspondent pas";
                    return View(NMDP);
                }
            } 
            else
            {
                ViewBag.Message = "L'ancien mot de passe est incorrect";
                return View(NMDP);
            }
        }
        else 
        {
            ViewBag.Message = "Impossible de retrouver le compte";
            return View(NMDP);
        }
    }
        public IActionResult Deconnexion()
    {
        this.HttpContext.Session.Remove("email");
        this.HttpContext.Session.Remove("role");
        return Redirect("/");
    }
}