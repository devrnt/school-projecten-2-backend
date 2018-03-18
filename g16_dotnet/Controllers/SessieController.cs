using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using g16_dotnet.Models.SessieViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace g16_dotnet.Controllers
{
    [Authorize(Policy = "Leerkracht")]
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
        /// Juiste code: Index View met als Model een IEnumerable<Groep>
        ///
        /// Foute code: RedirectToAction Index
        /// </returns>
        [AllowAnonymous]
        [ServiceFilter(typeof(SessieFilter))]
        public IActionResult ValideerSessiecode(string code)
        {
            if (code == null || code.Trim().Length == 0)
                TempData["error"] = "Geef een code in: ";
            else
            {
                try
                {
                    int sessieCode = int.Parse(code);
                    Sessie sessie = _sessieRepository.GetById(sessieCode);
                    if (sessie != null)
                    {
                        ViewData["codeIngegeven"] = true;
                        ViewData["sessieOmschrijving"] = sessie.Omschrijving;
                        // om via de filter in Session weg te schrijven
                        ViewData["sessieCode"] = sessieCode.ToString();
                        ViewData["Doelgroep"] = JsonConvert.SerializeObject(sessie.Doelgroep);
                        return View("Index", sessie.Groepen);
                    }
                    else
                    {
                        TempData["error"] = $"{code} hoort niet bij een sessie, vraag hulp aan je lesgever";
                    }
                }
                catch (FormatException)
                {
                    TempData["error"] = "De sessiecode moet een getal zijn!";
                }
            }
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        ///     Laat een leerkracht zijn/haar sessies beheren
        /// </summary>
        /// <param name="leerkracht">De leerkracht voor wie de bijhorende sessie worden opgevraagd</param>
        /// <returns>BeheerSessies View met een ICollection<SessieLijstViewModel> als Model</returns>
        [ServiceFilter(typeof(LeerkrachtFilter))]
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
        /// <param name="sessieId">Het id van de geselecteerde sessie</param>
        /// <returns>
        ///     SessieDetail View met een SessieDetailViewModel als Model.
        ///     Indien er geen Sessie wordt gevonden met het meegegeven id wordt er een
        ///     NotFoundResult teruggeven.
        /// </returns>
        public IActionResult SelecteerSessie(int sessieId)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();
            ViewData["Doelgroepen"] = GetDoelgroepenAsSelectList(sessie.Doelgroep);

            return View("SessieDetail", new SessieDetailViewModel(sessie));
        }


        /// <summary>
        ///     Activeert de sessie waarvan het detail werd weergegeven.
        /// </summary>
        /// <param name="sessieId">Id van de te activeren Sessie</param>
        /// <returns>
        ///     RedirectToAction BeheerSessies
        ///     Indien er geen Sessie wordt gevonden met het meegegeven id wordt er een
        ///     NotFoundResult teruggeven.
        /// </returns>
        public IActionResult ActiveerSessie(int sessieId)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();

            sessie.ActiveerSessie();
            _sessieRepository.SaveChanges();
            TempData["message"] = "Sessie is succesvol geactiveerd.";
            return RedirectToAction(nameof(SelecteerSessie), new { sessieId });

        }

        /// <summary>
        ///     Wijzigt de Groepen in de Sessie
        /// </summary>
        /// <param name="sessieId">Id van de Sessie waarvoor de Groepen moeten gewijzigd worden</param>
        /// <param name="behaviourId">0: Blokkeren, 1: Deblokkeren, 2: Ontgrendelen</param>
        /// <param name="groepId">Id van de te wijzigen Groep (of 0 voor alle Groepen)</param>
        /// <returns></returns>
        public IActionResult WijzigGroepen(int sessieId, int behaviourId, int groepId = 0)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();

            sessie.WijzigGroepen(behaviourId, groepId);
            _sessieRepository.SaveChanges();

            return RedirectToAction(nameof(SelecteerSessie), new { sessieId });
        }

        /// <summary>
        ///     Haalt een Sessie op een geeft deze mee in een Partial View
        /// </summary>
        /// <param name="sessieId">De te op te halen Sessie</param>
        /// <returns>PartialView _GroepenOverzicht met als Model een SessieDetailViewModel</returns>
        [HttpGet]
        public IActionResult CheckDeelnames(int sessieId)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();
            return PartialView("_GroepenOverzicht", new SessieDetailViewModel(sessie));
        }

        /// <summary>
        ///     Selecteer de Doelgroep voor een specifieke Sessie
        /// </summary>
        /// <param name="sessieId">De Sessie waarvoor de Doelgroep moet ingesteld worden</param>
        /// <param name="doelgroep">De waarde van de gekozen DoelgroepEnum</param>
        /// <returns>SessieDetail View met een SessieDetailViewModel als Model</returns>
        [HttpPost]
        [ServiceFilter(typeof(SessieFilter))]
        public IActionResult SelecteerDoelgroep(int sessieId, int doelgroep) {
            DoelgroepEnum gekozen = ((DoelgroepEnum)doelgroep);
            var sessie =_sessieRepository.GetById(sessieId);
            if (sessie == null) {
                return NotFound();
            }
            if (Enum.GetValues(typeof(DoelgroepEnum)).Length > doelgroep && doelgroep >= 0)
            {
                ViewData["sessieCode"] = sessieId.ToString();
                sessie.Doelgroep = gekozen;
                _sessieRepository.SaveChanges();
                ViewData["Doelgroep"] = JsonConvert.SerializeObject(sessie.Doelgroep);
            } else
            {
                TempData["error"] = "Ongeldige doelgroep";
            }

            return RedirectToAction(nameof(SelecteerSessie), new { sessieId });

        }

        /// <summary>
        ///     Controleert of de meegegeven sessie actief is
        /// </summary>
        /// <param name="sessieId">Id van de te controleren Sessie</param>
        /// <returns>
        ///     JsonObject met de property IsActief
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        public JsonResult IsSessieActief(string sessieId)
        {
            Sessie sessie = null;
            int id = 0;
            if (int.TryParse(sessieId, out id))
                sessie = _sessieRepository.GetById(id);
            return Json(new { sessie?.IsActief });
        }

        private SelectList GetDoelgroepenAsSelectList(DoelgroepEnum doelgroep) {
            return new SelectList(Enum.GetValues(typeof(DoelgroepEnum))
                .Cast<DoelgroepEnum>()
                .Select(d => new SelectListItem {
                    Text = d.ToString(),
                    Value = ((int)d).ToString()
                }).ToList(), "Value", "Text", doelgroep);
        }
    }


   



}