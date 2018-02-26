using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace g16_dotnet.Controllers {
    [ServiceFilter(typeof(SessieFilter))]
    public class GroepController : Controller {
        private readonly IGroepRepository _groepsRepository;
        public GroepController(IGroepRepository groepRepository) {
            _groepsRepository = groepRepository;
        }
        public IActionResult Index() {
            return View();
        }

        public IActionResult KiesGroep(Sessie sessie, int groepsId) {
            Groep gekozenGroep = _groepsRepository.GetById(groepsId);
            try {
                if (gekozenGroep != null) {
                    gekozenGroep.DeelnameBevestigd = true;
                    _groepsRepository.SaveChanges();
                } else {
                    return NotFound();
                }
            } catch {
                TempData["error"] = "Er is iets fout gelopen bij het kiezen van uw groep.";
            }
            return View("GroepOverzicht", gekozenGroep);
        }
    }
}