using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using g16_dotnet.Models.SessieViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace g16_dotnet.Controllers
{
    [ServiceFilter(typeof(LeerkrachtFilter))]
    public class SessieController : Controller {
        private readonly ISessieRepository _sessieRepository;

        public SessieController(ISessieRepository sessieRepository) {
            _sessieRepository = sessieRepository;
        }

        /// <summary>
        ///     Toont de startpagina waar men de sessiecode kan ingeven
        ///     Leerkrachten kunnen van hieruit naar sessiebeheer 
        /// </summary>
        /// <returns>Index View</returns>
        public IActionResult Index() {
            ViewData["codeIngegeven"] = false;
            return View();
        }

        /// <summary>
        ///     Controleert of de sessiecode juist is
        /// </summary>
        /// <param name="code">De code van de sessie waarbij men moet aansluiten</param>
        /// <returns>
        /// Juiste code: Index View met als Model een IEnumerable van type Groep
        ///
        /// Foute code: RedirectToAction Index
        /// </returns>
        public IActionResult ValideerSessiecode(int code)
        {
            Sessie sessie = _sessieRepository.GetById(code);
            if (sessie != null)
            {
                ViewData["codeIngegeven"] = true;
                return View("Index", sessie.Groepen);
            } else
            {
                TempData["error"] = $"{code} hoort niet bij een bestaande sessie";
            }
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        ///     Laat een leerkracht zijn/haar sessies beheren
        /// </summary>
        /// <param name="leerkracht">Aangeleverd door LeerkrachtFilter</param>
        /// <returns>BeheerSessies View met een ICollection van SessieLijstViewModel als Model</returns>
        public IActionResult BeheerSessies(Leerkracht leerkracht)
        {
            ICollection<SessieLijstViewModel> sessieLijst = new List<SessieLijstViewModel>();
            foreach (var sessie in leerkracht.Sessies)
                sessieLijst.Add(new SessieLijstViewModel(sessie));
            return View(sessieLijst);
        }

        /// <summary>
        ///     Selecteert een sessie om de details ervan te kunnen bekijken.
        /// </summary>
        /// <param name="leerkracht">Aangeleverd door LeerkrachtFilter</param>
        /// <param name="sessieId">Het id van de geselecteerde sessie</param>
        /// <returns>
        ///     SessieDetail View met een SessieDetailViewModel als Model.
        ///     Indien er geen Sessie wordt gevonden met het meegegeven id wordt er een
        ///     NotFoundResult teruggeven.
        /// </returns>
        public IActionResult SelecteerSessie(Leerkracht leerkracht, int sessieId)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();
            return View("SessieDetail", new SessieDetailViewModel(sessie));
        }


        /// <summary>
        ///     Activeert de sessie waarvan het detail werd weergegeven.
        /// </summary>
        /// <param name="leerkracht">Aangeleverd door LeerkrachtFilter</param>
        /// <param name="sessieId">Id van de Sessie waarvan het detail werd weergegeven</param>
        /// <returns>
        ///     RedirectToAction BeheerSessies
        ///     Indien er geen Sessie wordt gevonden met het meegegeven id wordt er een
        ///     NotFoundResult teruggeven.
        ///     Indien nog niet alle deelnames werden bevestigd wordt opnieuw de SessieDetail
        ///     View teruggeven met een SessieDetailViewModel als Model.
        /// </returns>
        public IActionResult ActiveerSessie(Leerkracht leerkracht, int sessieId)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();
            try
            {
                sessie.ActiveerSessie();
                _sessieRepository.SaveChanges();
                TempData["message"] = "Sessie is succesvol geactiveerd.";
                return RedirectToAction(nameof(BeheerSessies));
            }
            catch (InvalidOperationException e)
            {
                TempData["error"] = e.Message;
            }
            return View("SessieDetail", new SessieDetailViewModel(sessie));
        }

    }
}