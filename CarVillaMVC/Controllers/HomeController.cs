using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarVillaMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}