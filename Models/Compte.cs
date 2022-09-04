using System.ComponentModel.DataAnnotations;

namespace newMantis.Models;

public class Compte
{
    /// <summary>
    /// Model Compte
    /// 
    /// Description     :   Ce Model permet d'obtenir les details d'un compte
    /// 
    /// Date de creation : 05/2022 Soleilhavoup
    /// 
    /// 
    /// </summary>
    [RegularExpression(@"^\w+([-+.']\w+)*@celios.fr$", ErrorMessage = "L'adresse mail est invalide")]
    public string Email { get; set; }
    public string Motdepasse { get; set; }
}