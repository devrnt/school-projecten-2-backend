using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace g16_dotnet.Controllers
{
    public class CodeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Valideer()
        {
            if (1 == 1)
            {
                ViewData["test"] = "Code correct";
            }
            else
            {
                //ViewData["test"] = "Code fout";
            }
            return View("Index");
        }
    }
}