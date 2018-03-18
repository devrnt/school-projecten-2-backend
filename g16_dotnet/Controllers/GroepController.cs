using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace g16_dotnet.Controllers
{
    public class GroepController : Controller
    {
        private readonly IGroepRepository _groepsRepository;

        public GroepController(IGroepRepository groepRepository)
        {
            _groepsRepository = groepRepository;
        }

        public IActionResult Index()
        {
            return NotFound();
        }

        /// <summary>
        ///     Kiest de Groep om het spel te spelen
        /// </summary>
        /// <param name="sessieId">Id van de huidige Sessie</param>
        /// <param name="groepsId">Id van de gekozen Groep</param>
        /// <returns>
        ///     GroepOverzicht View met als Model de gekozen Groep
        ///     NotFoundResult indien de Groep niet gevonden werd
        ///     RedirectToAction Index in SessieController indien de Groep al gekozen werd
        /// </returns>
        public IActionResult KiesGroep(int sessieId, int groepsId)
        {
            Groep gekozenGroep = _groepsRepository.GetById(groepsId);
            if (gekozenGroep == null)
                return NotFound();
            if (gekozenGroep.DeelnameBevestigd)
            {
                TempData["error"] = "Groep is al gekozen!";
                return RedirectToAction(nameof(Index), "Sessie");
            }

            try
            {
                gekozenGroep.DeelnameBevestigd = true;
                _groepsRepository.SaveChanges();
                TempData["message"] = "Je hebt nu deelgenomen, zo dadelijk kan je beginnen";
                ViewData["sessieId"] = sessieId;
            }
            catch
            {
                TempData["error"] = "Er is iets fout gelopen bij het kiezen van uw groep.";
            }
            return View("GroepOverzicht", gekozenGroep);
        }

        /// <summary>
        ///     Start het spel voor de gekozen groep
        /// </summary>
        /// <param name="groepId">Id van de gekozen groep</param>
        /// <returns>
        ///     RedirectToAction Index in SpelController
        ///     NotFoundResult indien de Groep niet werd gevonden     
        /// </returns>
        public IActionResult StartSpel(int groepId)
        {
            Groep huidigeGroep = _groepsRepository.GetById(groepId);
            if (huidigeGroep == null)
                return NotFound();

            return RedirectToAction("Index", "Spel", new { padId = huidigeGroep.Pad.PadId });
        }



    }
}