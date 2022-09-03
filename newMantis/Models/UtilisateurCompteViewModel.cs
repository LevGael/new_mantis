using System.ComponentModel.DataAnnotations;

namespace newMantis.Models
{
    /// <summary>
    /// Model Utilisateur
    /// 
    /// Description     :   Ce Model permet d'avoir un compte utilisateur pour se connecter
    /// 
    /// Date de creation : 05/2022 Soleilhavoup
    /// </summary>
    public class UtilisateurCompteViewModel
    {
        [RegularExpression(@"^\w+([-+.']\w+)*@celios.fr$", ErrorMessage = "L'adresse mail est invalide")]
        public string? compte { get; set; }

        public bool isSuper { get; set; }

        public Utilisateur[]? listeUtilisateur { get; set; }
    }
}