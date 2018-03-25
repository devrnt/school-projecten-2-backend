using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using g16_dotnet.Models.GroepViewModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace g16_dotnet.Controllers {
    [ServiceFilter(typeof(SessieFilter))]
    public class GroepController : Controller {
        private readonly IGroepRepository _groepsRepository;

        public GroepController(IGroepRepository groepRepository) {
            _groepsRepository = groepRepository;
        }

        public IActionResult Index() {
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
        public IActionResult KiesGroep(int sessieId, int groepsId) {
            Groep gekozenGroep = _groepsRepository.GetById(groepsId);
            if (gekozenGroep == null)
                return NotFound();
            if (gekozenGroep.DeelnameBevestigd) {
                TempData["error"] = "Groep is al gekozen!";
                return RedirectToAction(nameof(Index), "Sessie");
            }

            gekozenGroep.DeelnameBevestigd = true;
            _groepsRepository.SaveChanges();
            TempData["message"] = "Je hebt nu deelgenomen, zo dadelijk kan je beginnen";
            ViewData["sessieId"] = sessieId;

            return View("GroepOverzicht", gekozenGroep);
        }

        /// <summary>
        ///     Start het spel voor de gekozen groep
        /// </summary>
        /// <param name="sessie">De huidige sessie</param>
        /// <param name="groepId">Id van de gekozen groep</param>
        /// <returns>
        ///     RedirectToAction Index in SpelController
        ///     NotFoundResult indien de Groep niet werd gevonden     
        /// </returns>
        public IActionResult StartSpel(Sessie sessie, int groepId) {
            Groep huidigeGroep = _groepsRepository.GetById(groepId);
            if (huidigeGroep == null)
                return NotFound();
            if (!sessie.IsActief)
            {
                ViewData["sessieId"] = sessie.SessieCode;
                return View("GroepOverzicht", huidigeGroep);
            }

            return RedirectToAction("Index", "Spel", new { padId = huidigeGroep.Pad.PadId });
        }

        /// <summary>
        ///     Sluit de Leerling aan aan de gekozen Groep
        /// </summary>
        /// <param name="sessie">De huidige sessie</param>
        /// <param name="leerling">De Leerling die zich wil aansluiten</param>
        /// <param name="groepId">Het id van de Groep waarbij de Leerling zich wil aansluiten</param>
        /// <returns>RedirectToAction naar ValideerSessieCode in Sessie voor een Jongeren Sessie
        /// RedirectToAction naar StartSpel voor een Volwassenen Sessie
        /// </returns>
        [Authorize(Policy = "Leerling")]
        [ServiceFilter(typeof(LeerlingFilter))]
        public IActionResult NeemDeel(Sessie sessie, Leerling leerling, int groepId)
        {
            Groep groep = _groepsRepository.GetById(groepId);
            if (groep == null)
                return NotFound();
            if (sessie.Klas.Leerlingen.Any(l => l.LeerlingId == leerling.LeerlingId))
            {
                groep.Leerlingen.Add(leerling);
                _groepsRepository.SaveChanges();
                if (sessie.Doelgroep == DoelgroepEnum.Volwassenen)
                {
                    groep.DeelnameBevestigd = true;
                    _groepsRepository.SaveChanges();
                    return RedirectToAction(nameof(StartSpel), new { groepId });
                }
            } else
            {
                TempData["error"] = "Je zit niet in de klas voor deze sessie!";
            }
            
            return RedirectToAction("ValideerSessieCode", "Sessie", new { code = sessie.SessieCode.ToString()});
        }
    }
}