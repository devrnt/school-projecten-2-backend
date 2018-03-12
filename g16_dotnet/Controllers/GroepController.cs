using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace g16_dotnet.Controllers
{
    [ServiceFilter(typeof(SessieFilter))]
    public class GroepController : Controller {
        private readonly IGroepRepository _groepsRepository;
        public GroepController(IGroepRepository groepRepository) {
            _groepsRepository = groepRepository;
        }
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        ///     Selecteert de Groep om de deelname te bevestigen
        /// </summary>
        /// <param name="sessie">Aangeleverd door SessieFilter</param>
        /// <param name="groepsId">Het id van de gekozen Groep</param>
        /// <returns>
        ///     GroepOverzicht View met een Groep als Model
        ///     Indien er geen Groep wordt gevonden met meegegeven id wordt er een NotFoundResult teruggeven
        /// </returns>
        public IActionResult KiesGroep(Sessie sessie, int groepsId) {
            Groep gekozenGroep = _groepsRepository.GetById(groepsId);
            if (gekozenGroep == null)
                return NotFound();
            if (gekozenGroep.DeelnameBevestigd)
            {
                TempData["error"] = "Groep is al gekozen!";
                return RedirectToAction(nameof(Index), "Sessie");
            }

            try {
                    gekozenGroep.DeelnameBevestigd = true;
                    _groepsRepository.SaveChanges();
                    TempData["message"] = "U heeft succesvol deelgenomen aan de sessie. Op dit moment moet u wachten op het startsignaal van uw leerkracht.";
            } catch {
                TempData["error"] = "Er is iets fout gelopen bij het kiezen van uw groep.";
            }
            return View("GroepOverzicht", gekozenGroep);
        }

        /// <summary>
        ///     Start het spel voor de gekozen groep
        /// </summary>
        /// <param name="sessie">Aangeleverd door SessieFilter</param>
        /// <param name="groepId">Id van de gekozen groep</param>
        /// <returns>
        ///     RedirectToAction Index in SpelController
        ///     Indien de Groep niet werd gevonden wordt er een NotFoundResult teruggegeven
        ///     Indien de Sessie nog niet op actief is gezet wordt de GroepOverzicht teruggegeven
        ///     met een Groep als Model.
        /// </returns>
        public IActionResult StartSpel(Sessie sessie, int groepId)
        {
            Groep huidigeGroep = _groepsRepository.GetById(groepId);
            if (huidigeGroep == null)
                return NotFound();
            if (sessie.IsActief)
                return RedirectToAction("Index", "Spel", new { pad = huidigeGroep.Pad });
            TempData["error"] = "Wacht op signaal van je leerkracht om verder te gaan!";
            
            return View("GroepOverzicht", huidigeGroep);
        }

        [HttpGet]
        public JsonResult IsSessieActief(Sessie sessie)
        {
            return Json(new { sessie.IsActief });
        }
    }
}