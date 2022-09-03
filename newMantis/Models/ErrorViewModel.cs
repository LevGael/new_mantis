namespace newMantis.Models;

/// <summary>
    /// Model ErrorViewModel
    /// 
    /// Description     :   Ce Model permet de modéliser les différents cas d'erreur que l'on peut obtenir. 
    /// 
    /// Date de creation : 05/2022 Soleilhavoup
    /// 
    /// 
/// </summary>

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
