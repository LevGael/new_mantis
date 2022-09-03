using Newtonsoft.Json;

namespace newMantis.Models 
{
    /// <summary>
    /// Model Issue
    /// 
    /// Description     :   Ce Model permet d'obtenir la liste des issues avec les différents détails
    /// 
    /// Date de creation : 05/2022 Levanier
    /// 
    /// 
    /// </summary>
    public class Issue
    {
        [JsonProperty("issues")]
        public List<issue> issues { get; set; }
    }

    /// <summary>
    /// Model Projets
    /// 
    /// Description     :   Obtenir les issues d'un projet
    /// 
    /// Date de creation : 05/2022 Levanier
    /// 
    /// 
    /// </summary>
    public class issue
    {
        [JsonProperty("id")]
        public int id { get; set; }
        public string? summary { get; set; }
        [JsonProperty("project")]
        public project projects { get; set; }
        [JsonProperty("category")]
        public category categories { get; set; }
        [JsonProperty("status")]
        public status status { get; set; }
        [JsonProperty("resolution")]
        public resolution resolutions { get; set; }
        [JsonProperty("severity")]
        public severity severities { get; set; }
        [JsonProperty("reporter")]
        public reporter reporters { get; set; }
        [JsonProperty("history")]
        public List<history> histories { get; set; }
        [JsonProperty("created_at")]
        public DateTime created_at { get; set; }
    }

    public class project
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
    }

    public class category
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
    }

    public class status
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
    }

    public class resolution
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string? name { get; set; }
    }

    public class severity
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("label")]
        public string? label { get; set; }
    }

    public class reporter
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("email")]
        public string? email { get; set; }
    }

    public class history
    {
        public string? messages { get; set; }
    }
}

