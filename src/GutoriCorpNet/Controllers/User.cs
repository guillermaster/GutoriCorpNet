using Microsoft.AspNetCore.Mvc;

namespace GutoriCorp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Lock()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Recover()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
    }
}