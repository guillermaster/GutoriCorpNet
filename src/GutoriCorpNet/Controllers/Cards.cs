using Microsoft.AspNetCore.Mvc;

namespace GutoriCorp.Controllers
{
    public class CardsController : Controller
    {
        public IActionResult Cards()
        {
            return View();
        }
    }
}