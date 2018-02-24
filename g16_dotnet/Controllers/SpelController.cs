using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
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
            Opdracht huidig = pad.HuidigeOpdracht;
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
            if (pad.Voortgang <= pad.Acties.Where(a => a.IsUitgevoerd).Count())
            {
                TempData["error"] = "Los eerst de opdracht op!";
                return RedirectToAction(nameof(Index));
            }

            if (pad.HuidigeOpdracht.ControleerToegangsCode(toegangsCode))
            {
                pad.HuidigeActie.IsUitgevoerd = true;
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = (toegangsCode == null || toegangsCode.Trim().Length == 0) ?  $"Je hebt geen toegangscode ingegeven" : $"{toegangsCode} is fout!";
            ViewData["fase"] = "actie";
            return View("Index", pad);
        }
    }
}