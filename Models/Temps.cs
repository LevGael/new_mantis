namespace newMantis.Models
{
    /// <summary>
    /// Model Temps
    /// 
    /// Description     :   Modélise les différents temps que l'utilisateur peut modifier dans l'onglet Admin
    /// 
    /// Date de creation : 05/2022 Soleilhavoup
    /// 
    /// 
    /// </summary>
    public class Temps
    {
        public string tempsJour { get; set; }
        public int tempsMineur { get; set; }
        public int tempsMajeur { get; set; }
        public int tempsCritique { get; set; }
    }
}