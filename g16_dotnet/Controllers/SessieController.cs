using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g16_dotnet.Models.Domain;
using g16_dotnet.Models.GroepViewModels;
using Microsoft.AspNetCore.Mvc;

namespace g16_dotnet.Controllers
{
    public class SessieController : Controller
    {
        private readonly ISessieRepository _sessieRepository;
        private Sessie correcteSessie;
        public SessieController(ISessieRepository sessieRepository)
        {
            _sessieRepository = sessieRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int inputCode)
        {

            Sessie sessie = _sessieRepository.valideerCode(inputCode);
            if (sessie != null)
            {
                TempData["code"] = "geldig";
                correcteSessie = sessie;
                return View(sessie.Groepen);
            }
            else
            {
                TempData["code"] = "niet geldig";
            }
            return View();
        }
        public IActionResult Deelnemen(string groepsnaam)
        {
            return View(new GroepViewModel(_sessieRepository.getByName(correcteSessie, groepsnaam)));
        }
        [HttpPost]
        public IActionResult Deelnemen(string groepsnaam, GroepViewModel model)
        {
            return View(new GroepViewModel(_sessieRepository.getByName(correcteSessie, groepsnaam)));
        }
    }
}