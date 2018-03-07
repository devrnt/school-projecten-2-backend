using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using g16_dotnet.Models.SessieViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Controllers
{
    [Authorize(Policy = "Leerkracht")]
    [ServiceFilter(typeof(LeerkrachtFilter))]
    public class SessieController : Controller
    {
        private readonly ISessieRepository _sessieRepository;

        public SessieController(ISessieRepository sessieRepository)
        {
            _sessieRepository = sessieRepository;
        }

        /// <summary>
        ///     Toont de startpagina waar men de sessiecode kan ingeven
        ///     Leerkrachten kunnen van hieruit naar sessiebeheer 
        /// </summary>
        /// <returns>Index View</returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
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
        [AllowAnonymous]
        public IActionResult ValideerSessiecode(int code)
        {
            Sessie sessie = _sessieRepository.GetById(code);
            if (sessie != null)
            {
                ViewData["codeIngegeven"] = true;
                return View("Index", sessie.Groepen);
            }
            else
            {
                TempData["error"] = code == 0 ? "Geef een code in" : $"{code} hoort niet bij een sessie, vraag hulp aan je leerkracht";
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

        /// <summary>
        /// Blokkeer een groep op basis van groepId + sessieId
        /// </summary>
        /// <param name="leerkracht">Aangeleverd door LeerkrachtFilter</param>
        /// <param name="sessieId">Id van de sessie van het huidig detailvenster</param>
        /// <param name="groepId">Id van de corresponderende groep</param>
        /// <returns>
        /// geeft huidige view geupdate terug</returns>
        public IActionResult BlokkeerGroep(Leerkracht leerkracht, int sessieId, int groepId)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();
            Groep groep = sessie.Groepen.FirstOrDefault(g => g.GroepId == groepId); // gebruik linq
            //foreach (Groep g in sessie.Groepen)
            //{
            //    if (g.GroepId == groepId)
            //    {
            //        groep = g;
            //    }

            //}
            if (groep == null)
            {
                TempData["error"] = "Groep niet gevonden";
            } else
            {
                groep.BlokkeerPad();
                _sessieRepository.SaveChanges();
                TempData["message"] = "Groep werd succesvol geblokkeerd.";
            }

            return View("SessieDetail", new SessieDetailViewModel(sessie));
        }

        /// <summary>
        /// Delokkeer een groep op basis van groepId + sessieId
        /// </summary>
        /// <param name="leerkracht">Aangeleverd door LeerkrachtFilter</param>
        /// <param name="sessieId">Id van de sessie van het huidig detailvenster</param>
        /// <param name="groepId">Id van de corresponderende groep</param>
        /// <returns>
        /// geeft huidige view geupdate terug</returns>
        public IActionResult DeblokkeerGroep(Leerkracht leerkracht, int sessieId, int groepId)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();
            Groep groep = sessie.Groepen.FirstOrDefault(g => g.GroepId == groepId);
            if (groep != null)
            {
                groep.DeblokkeerPad();
                _sessieRepository.SaveChanges();
                TempData["message"] = "Groep werd succesvol gedeblokkeerd.";
            } else
            {
                TempData["error"] = "Groep niet gevonden.";
            }
            return View("SessieDetail", new SessieDetailViewModel(sessie));
        }

        /// <summary>
        /// Blokkeer alle groepen op basis van sessieId
        /// </summary>
        /// <param name="leerkracht">Aangeleverd door LeerkrachtFilter</param>
        /// <param name="sessieId">Id van de sessie van het huidig detailvenster</param>
        /// <returns>
        /// geeft huidige view geupdate terug</returns>
        public IActionResult BlokkeerAlleGroepen(Leerkracht leerkracht, int sessieId)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();
            sessie.BlokkeerAlleGroepen();
            //foreach (Groep g in sessie.Groepen)
            //{
            //    g.BlokkeerPad();

            //}
            _sessieRepository.SaveChanges();
            TempData["message"] = "Alle groepen werden succesvol geblokkeerd.";
            return View("SessieDetail", new SessieDetailViewModel(sessie));
        }

        /// <summary>
        /// Deblokkeer alle groepen op basis van sessieId
        /// </summary>
        /// <param name="leerkracht">Aangeleverd door LeerkrachtFilter</param>
        /// <param name="sessieId">Id van de sessie van het huidig detailvenster</param>
        /// <returns>
        /// geeft huidige view geupdate terug</returns>
        public IActionResult DeblokkeerAlleGroepen(Leerkracht leerkracht, int sessieId)
        {
            
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();
            sessie.DeblokkeerAlleGroepen();
            _sessieRepository.SaveChanges();
            TempData["message"] = "Alle groepen werden succesvol gedeblokkeerd.";
            return View("SessieDetail", new SessieDetailViewModel(sessie));
        }
    }


}