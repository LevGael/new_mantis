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
    public class Utilisateur
    {
        [Key]
        public string? email { get; set; }
        public string? mdp { get; set; }
        public string? nom { get; set; }
        public string? prenom { get; set; }
        public int role { get; set; }
        public int firstconnexion { get; set; }
        public string? tempsJour { get; set; }
        public int tempsMineur { get; set; }
        public int tempsMajeur { get; set; }
        public int tempsCritique { get; set; }
    }
}