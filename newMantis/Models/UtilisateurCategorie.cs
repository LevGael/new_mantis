namespace newMantis.Models
{
    /// <summary>
    /// Model UtilisateurCategorie
    /// 
    /// Description     :   Ce Model permet de lier les catégories à un utilisateur
    /// 
    /// Date de creation : 05/2022 Soleilhavoup
    /// 
    /// Note : Extension du model Utilisateur
    /// 
    /// </summary>
    public class UtilisateurCategorie
    {
        public int idCategorie { get; set; }
        public string email { get; set; }
    }
}