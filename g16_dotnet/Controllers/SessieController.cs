using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace g16_dotnet.Controllers {
    public class SessieController : Controller {
        private readonly ISessieRepository _sessieController;

        public SessieController(ISessieRepository sessieRepository) {
            _sessieController = sessieRepository;
        }

        public IActionResult Index() {
            TempData["index"] = "This is the index page.";
            return View();
        }

        [HttpPost]
        public IActionResult Index(int inputCode) {
            Sessie sessie = _sessieController.GetById(inputCode);
            if(sessie != null) {
                TempData["code"] = "geldig";
                var groepen = new List<Groep> { new Groep(), new Groep()};
                return View(groepen);
            } else {
                TempData["code"] = "niet geldig";
            }
            return View();
        }
    }
}