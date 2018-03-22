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
        /// <param name="groepId">Id van de gekozen groep</param>
        /// <returns>
        ///     RedirectToAction Index in SpelController
        ///     NotFoundResult indien de Groep niet werd gevonden     
        /// </returns>
        public IActionResult StartSpel(int groepId) {
            Groep huidigeGroep = _groepsRepository.GetById(groepId);
            if (huidigeGroep == null)
                return NotFound();

            return RedirectToAction("Index", "Spel", new { padId = huidigeGroep.Pad.PadId });
        }

        [Authorize(Policy = "Leerling")]
        [ServiceFilter(typeof(LeerlingFilter))]
        public IActionResult Deelnemen(Sessie sessie, Leerling leerling, int groepId)
        {
            Groep groep = _groepsRepository.GetById(groepId);
            if (sessie.Klas.Leerlingen.Any(l => l.LeerlingId == leerling.LeerlingId))
            {
                groep.Leerlingen.Add(leerling);
                _groepsRepository.SaveChanges();
            } else
            {
                TempData["error"] = "Je zit niet in de klas voor deze sessie!";
            }
            return RedirectToAction("ValideerSessieCode", "Sessie", new { code = sessie.SessieCode.ToString()});
        }


        //public IActionResult ModifieerGroep(Sessie sessie, int groepsId) {
        //    return View("ModifieerGroep", new GroepViewModel(_groepsRepository.GetById(groepsId), sessie));
        //}


        //public IActionResult ModifieerGroepVerwijderLeerling(Sessie sessie, int groepsId, int leerlingId) {
        //    Groep groep = _groepsRepository.GetById(groepsId);
        //    var lln = _groepsRepository.GetById(groepsId).Leerlingen.First(x => x.LeerlingId.Equals(leerlingId));
        //    groep.VerwijderLeerlingUitGroep(lln);
        //    TempData["message"] = $"Leerling {lln.Voornaam} {lln.Naam} is verwijderd";

        //    //sessie.Groepen.First(y => y.GroepId.Equals(groepsId)).VerwijderLeerlingUitGroep(lln);
        //    return View("ModifieerGroep", new GroepViewModel(groep, sessie));

        //}

        //public IActionResult ModifieerGroepLeerlingToevoegen(Sessie sessie, string leerlingId, int groepId) {
        //    //throw new NotImplementedException();

        //    //Ik krijg in sessie.klas enkel de lln die al in een groep zitten...
        //    Groep groep = _groepsRepository.GetById(groepId);
        //    if (leerlingId != null) {
        //        Leerling leerling = sessie.Klas.Leerlingen.First(x => x.LeerlingId.ToString().Equals(leerlingId));
        //        groep.Leerlingen.Add(leerling);
        //        TempData["message"] = $"Leerling {leerling.Voornaam} {leerling.Naam} is toegevoegd";
        //        return View("ModifieerGroep", new GroepViewModel(groep, sessie));
        //    } else {
        //        TempData["error"] = "Selecteer een leerling om toe te voegen";
        //        return View("ModifieerGroep", new GroepViewModel(groep, sessie));
        //    }
        //}
        //[HttpPost]
        //public IActionResult ModifieerGroepGroepsnaamWijzigen(Sessie sessie, GroepViewModel gVM, int groepId) {
        //    sessie.Groepen.First(x => x.GroepId.Equals(groepId)).Groepsnaam = gVM.GroepNaam;
        //    ViewBag.GroepsnaamSuccesvolVerandert = "ok";
        //    TempData["message"] = "Groepsnaam succesvol gewijzigd.";
        //    return View("ModifieerGroep", new GroepViewModel(_groepsRepository.GetById(groepId), sessie));
        //}
    }
}