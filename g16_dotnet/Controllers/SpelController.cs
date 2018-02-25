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
            try
            {
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
                TempData["error"] = $"{groepsAntwoord} is fout!";
            }
            catch (System.ArgumentException e)
            {
                TempData["error"] = e.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult VoerActieUit(Pad pad, string toegangsCode)
        {
            if (pad.Voortgang <= pad.Acties.Where(a => a.IsUitgevoerd).Count())
            {
                TempData["error"] = "Los eerst de opdracht op!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (pad.HuidigeOpdracht.ControleerToegangsCode(toegangsCode))
                {
                    pad.HuidigeActie.IsUitgevoerd = true;
                    return RedirectToAction(nameof(Index));
                }
                TempData["error"] = $"{toegangsCode} is fout!";
            }
            catch (System.ArgumentException e)
            {
                TempData["error"] = e.Message;
            }
            ViewData["fase"] = "actie";
            return View("Index", pad);
        }
    }
}