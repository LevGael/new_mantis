using Newtonsoft.Json;

namespace newMantis.Models
{
    /// <summary>
    /// Model Projet
    /// 
    /// Description     :   Ce Model permet d'obtenir la liste des projets et des sous-projets
    /// 
    /// Date de creation : 05/2022 Levanier
    /// 
    /// 
    /// </summary>

    public class Projet
    {
        [JsonProperty("projects")]
        public List<projects> projects { get; set; }
    }

    /// <summary>
    /// Model projects
    /// 
    /// Description     :   Obtenir un projet dans une liste
    /// 
    /// Date de creation : 05/2022 Levanier
    /// 
    /// 
    /// </summary>
    public class projects
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
        [JsonProperty("description")]
        public string? description { get; set; }

        [JsonProperty("subProjects")]
        public List<projects> subprojects { get; set; }
    }
    
}