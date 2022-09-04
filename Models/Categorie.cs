using System.ComponentModel.DataAnnotations;

namespace newMantis.Models
{
    public class Categorie
    {
        /// <summary>
        /// Model Categorie
        /// 
        /// Description     :   Ce Model permet d'avoir les informations d'une cat�gorie d'un projet
        /// 
        /// Date de creation : 05/2022 Soleilhavoup
        /// 
        /// 
        /// </summary>
        [Key]
        public int idCategorie { get; set; }
        public string? libelle { get; set; }
    }
    
}