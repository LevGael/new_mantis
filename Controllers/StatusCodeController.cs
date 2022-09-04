using Microsoft.AspNetCore.Mvc;

namespace newMantis.Controllers;

public class StatusCodeController : Controller
{

    public IActionResult Erreur404()
    {
        return View();
    }

}