namespace newMantis.Models
{
    /// <summary>
    /// Model TreeViewNode
    /// 
    /// Description     :   Permet la création de noeud pour rétablir la relation père-fils des projets et sous-projets. 
    /// 
    /// Date de creation : 05/2022 Soleilhavoup
    /// 
    /// 
    /// </summary>

    public class TreeViewNode
    {
        public int id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
    }
}