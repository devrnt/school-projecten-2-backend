using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Controllers
{
    [ServiceFilter(typeof(PadSessionFilter))]
    public class SpelController : Controller
    {
        public IActionResult Index(Pad pad)
        {
            ViewData["fase"] = "opdracht";
            return View(pad);
        }

        public IActionResult BeantwoordVraag(Pad pad, string groepsAntwoord)
        {
            Opdracht huidig = pad.Opdrachten.First(o => !o.isVoltooid);
            if (huidig.Oefening.ControleerAntwoord(groepsAntwoord))
            {
                huidig.isVoltooid = true;
                if (pad.Voortgang == pad.AantalOpdrachten)
                {
                    ViewData["fase"] = "schatkist";
                    return View("Index", pad);
                }
                ViewData["fase"] = "actie";
                return View("Index", pad);
            }
            TempData["error"] = (groepsAntwoord == null || groepsAntwoord.Trim().Length == 0) ? "Je hebt geen antwoord opgegeven" : $"{groepsAntwoord} is fout!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult VoerActieUit(Pad pad, string toegangsCode)
        {
            if (pad.huidigeOpdracht.ControleerToegangsCode(toegangsCode))
            {
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = (toegangsCode == null || toegangsCode.Trim().Length == 0) ?  $"Je hebt geen toegangscode ingegeven" : $"{toegangsCode} is fout!";
            ViewData["fase"] = "actie";
            return View("Index", pad);
        }
    }
}