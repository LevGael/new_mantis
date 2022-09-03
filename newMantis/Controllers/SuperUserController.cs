using newMantis.Models;
using newMantis.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace newMantis.Controllers
{
    [CustomAuthFilter]
    [CustomSuperUserFilter]
    [CustomFirstConnectionFilter]
    public class SuperUserController : Controller
    {

        private readonly newMantisContext _context;

        public SuperUserController(newMantisContext context)
        {
            _context = context;
        }

        /**
            * Méthode Index - Get
            *
            * Cette méthode permet d'afficher le formulaire pour ajouter un utilisateur ainsi que la liste des utilisateurs déjà ajoutés. 
            *
            * Créé : 08/2022 Soleilhavoup
        */

        [HttpGet]
        public IActionResult Index()
        {
            UtilisateurCompteViewModel viewModel = new UtilisateurCompteViewModel();
            viewModel.listeUtilisateur = _context.Utilisateur.ToArray();
            viewModel.compte = "";
            
            return View(viewModel);
        }

        /**
            * Méthode Index - Post
            *
            * Cette méthode permet au super utilisateur d'ajouter un nouvel utilisateur. Elle vérifie si le formulaire est bien rempli. Puis on
            * on vérifie dans la base de données si l'adresse mail ajoutée existe déjà. Si elle existe on renvoie un message d'erreur.
            * Sinon, on ajoute l'adresse mail avec un mot de passe de première connexion.
            *
            * @param viexModel  Type UtilisateurCompteViewModel. Permet d'avoir le type Compte pour récupérer l'adresse mail dans le formulaires.
            *                                                    Mais également la liste des utilisateurs déjà ajoutés.  
            *
            * Créé : 08/2022 Soleilhavoup
        */

        [HttpPost]
        public async Task<IActionResult> Index(UtilisateurCompteViewModel viewModel)
        {
            viewModel.listeUtilisateur = _context.Utilisateur.ToArray();
            if(ModelState.IsValid)
            {
                var status = _context.Utilisateur.Where(u => u.email == viewModel.compte).FirstOrDefault();

                if (status == null)
                {
                    char[] delimiterChars = {'.', '@'};
                    Utilisateur user = new Utilisateur();
                    user.email = viewModel.compte;
                    user.mdp = BCrypt.Net.BCrypt.HashPassword("Mdp1ereConnexion");
                    string[] emailSplit = viewModel.compte.Split(delimiterChars);
                    string nom = emailSplit[1];
                    string prenom = emailSplit[0];
                    user.nom = char.ToUpper(nom[0]) + nom.Substring(1);
                    user.prenom = char.ToUpper(prenom[0]) + prenom.Substring(1);
                    if(viewModel.isSuper)
                    {
                        user.role = 1;
                    }
                    try
                    {
                        TempData["Success"] = "L'utilisateur a bien été ajouté";
                        _context.Utilisateur.Add(user);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    } catch(DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                else
                {
                    TempData["Error"] = "L'utilisateur que vous voulez ajouter existe déjà";
                    return View(viewModel);
                }
            }
            else
            {
                return View(viewModel);
            }
        }

        public async Task<IActionResult> RemoveUser(string email)
        {
            if(email != "admin@celios.fr")
            {
                Utilisateur user = new Utilisateur();
                user = _context.Utilisateur.Where(u => u.email == email).FirstOrDefault();
                if (user != null)
                {
                    try
                    {
                        _context.Utilisateur.Remove(user);
                        await _context.SaveChangesAsync();
                        TempData["Success"] = "L'utilisateur a bien été supprimé";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["Error"] = "Vous n'avez pas le pouvoir de supprimer le super admin";
                return RedirectToAction(nameof(Index));
            }
        }
    }

}