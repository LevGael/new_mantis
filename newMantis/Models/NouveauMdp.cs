namespace newMantis.Models
{
    /// <summary>
    /// Model Compte
    /// 
    /// Description     :   Ce Model permet d'obtenir les details du formulaire Première_connection
    /// 
    /// Date de creation : 02/08/2022 Levanier
    /// 
    /// 
    /// </summary>

    public class NouveauMdp
    {
        public string AncienMotdepasse { get; set; }
        public string NouveauMotdepasse { get; set; }
        public string RepeatMotdepasse { get; set; }
    }
}
