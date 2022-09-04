namespace newMantis.Models
{
    /// <summary>
    /// Model ListeCat
    /// 
    /// Description     :   Ce Model permet d'obtenir la liste des categories
    /// 
    /// Notes: Appartient au Model Catégorie
    /// 
    /// Date de creation : 05/2022 Soleilhavoup
    /// 
    /// </summary>
    public class ListeCat
    {
        public List<Categorie> Categories { get; set; }
        public List<Categorie> AllCategories { get; set; }
        public Temps Temps { get; set; }
    }
    
}