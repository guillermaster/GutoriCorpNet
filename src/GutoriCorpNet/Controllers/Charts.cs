using Microsoft.AspNetCore.Mvc;

namespace GutoriCorp.Controllers
{
    public class ChartsController : Controller
    {
        public IActionResult Flot()
        {
            return View();
        }
        public IActionResult Radial()
        {
            return View();
        }
        public IActionResult Rickshaw()
        {
            return View();
        }

    }
}