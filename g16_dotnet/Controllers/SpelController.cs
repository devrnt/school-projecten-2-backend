using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace g16_dotnet.Controllers
{
    [ServiceFilter(typeof(PadFilter))]
    public class SpelController : Controller
    {
        private readonly IPadRepository _padRepository;

        public SpelController(IPadRepository padRepository)
        {
            _padRepository = padRepository;
        }

        /// <summary>
        /// Geeft de Index pagina weer
        /// </summary>
        /// <param name="pad">Aangeleverd door PadSessionFilter</param>
        /// <returns>
        ///     Index View met als fase opdracht
        /// </returns>
        public IActionResult Index(Pad pad)
        {
            if (pad.Voortgang != pad.Acties.Count(pa => pa.Actie.IsUitgevoerd))
            {
                TempData["error"] = "Voer eerst de actie uit!";
                ViewData["fase"] = "actie";
            } else
            {
                ViewData["fase"] = "opdracht";
            }
            return View(pad);
        }

        /// <summary>
        /// Controleert of het groepsantwoord juist is 
        /// </summary>
        /// <param name="pad">Aangeleverd door de PadSessionFilter</param>
        /// <param name="groepsAntwoord">Het opgegeven antwoord als oplossing voor de huidige opdracht</param>
        /// <returns>
        ///     Bij een fout of geen antwoord RedirectToAction naar de Index
        ///     Bij een juist antwoord maar nog opdrachten over de Index view met als fase actie
        ///     Bij een juist antwoord en geen opdracht meer over de Index view met als fase schatkist
        /// </returns>
        public IActionResult BeantwoordVraag(int padId, string groepsAntwoord)
        {
            Pad pad = _padRepository.GetById(padId);
            if (pad == null)
                return NotFound();
            try
            {
                Opdracht huidig = pad.HuidigeOpdracht;
                if (huidig.ControleerAntwoord(int.Parse(groepsAntwoord)))
                {
                    huidig.IsVoltooid = true;
                    if (pad.Voortgang == pad.AantalOpdrachten)
                    {
                        ViewData["fase"] = "schatkist";
                        return View("Index", pad);
                    }
                    ViewData["fase"] = "actie";
                    return View("Index", pad);
                }
                huidig.AantalPogingen++;
                TempData["error"] = $"{groepsAntwoord} is fout!";
            }
            catch (System.ArgumentException e)
            {
                TempData["error"] = e.Message;
            }
            catch (System.FormatException)
            {
                TempData["error"] = "Je moet een getal invullen!";
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Controleert of de meegegeven toegangscode juist is
        /// </summary>
        /// <param name="pad">Aangeleverd door PadSessionFilter</param>
        /// <param name="toegangsCode">De opgegeven toegangscode</param>
        /// <returns>
        ///     Foute of geen code: RedirectToAction Index
        ///     Juiste code: View Index met als fase opdracht
        /// </returns>
        public IActionResult VoerActieUit(Pad pad, string toegangsCode)
        {
            if (pad.Voortgang <= pad.Acties.Where(pa => pa.Actie.IsUitgevoerd).Count())
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