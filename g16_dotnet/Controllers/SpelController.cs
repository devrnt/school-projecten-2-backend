using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace g16_dotnet.Controllers
{
    public class SpelController : Controller
    {
        private readonly IPadRepository _padRepository;

        public SpelController(IPadRepository padRepository)
        {
            _padRepository = padRepository;
        }

        /// <summary>
        ///     Geeft de Index pagina weer, deze rendert een
        ///     PartialView o.b.v. de PadState van het huidige Pad
        /// </summary>
        /// <param name="padId">Id van het huidige Pad</param>
        /// <returns>
        /// Index View met als Model een Pad
        /// NotFoundResult indien er geen Pad gevonden wordt met meegegeven id
        /// </returns>
        public IActionResult Index(int padId)
        {
            Pad pad = _padRepository.GetById(padId);
            if (pad == null)
                return NotFound();

            return View(pad);
        }

        /// <summary>
        /// Controleert of het groepsantwoord juist is 
        /// </summary>
        /// <param name="padId">Id van het huidige pad</param>
        /// <param name="groepsAntwoord">Het opgegeven antwoord als oplossing voor de huidige opdracht</param>
        /// <returns>
        ///     RedirectToAction Index
        ///     NotFoundResult indien er geen Pad wordt gevonden met meegegeven id
        /// </returns>
        public IActionResult BeantwoordVraag(int padId, string groepsAntwoord)
        {
            Pad pad = _padRepository.GetById(padId);

            if (pad == null)
                return NotFound();

            if (groepsAntwoord == null || groepsAntwoord.Trim().Length == 0)
                TempData["error"] = "Geef een antwoord in!";
            else
            {
                try
                {
                    PadOpdracht huidig = pad.HuidigeOpdracht;
                    if (pad.ControleerAntwoord(int.Parse(groepsAntwoord)))
                    {
                        TempData["message"] = "Juist antwoord, goed zo!";
                    }
                    else
                    {
                        TempData["error"] = $"{groepsAntwoord} is fout!";
                    }
                    _padRepository.SaveChanges();
                }
                catch (InvalidOperationException e)
                {
                    TempData["error"] = e.Message;
                }
                catch (FormatException)
                {
                    TempData["error"] = "Je moet een getal invullen!";
                }
            }

            return RedirectToAction(nameof(Index), new { padId });
        }

        /// <summary>
        /// Controleert of de toegangscode voor de volgende Opdracht juist is 
        /// </summary>
        /// <param name="padId">Id van het huidige pad</param>
        /// <param name="toegangsCode">De opgegeven toegangscode</param>
        /// <returns>
        ///     RedirectToAction Index
        ///     NotFoundResult indien er geen Pad wordt gevonden met meegegeven id
        /// </returns>
        public IActionResult VoerActieUit(int padId, string toegangsCode)
        {
            Pad pad = _padRepository.GetById(padId);
            if (pad == null)
                return NotFound();

            try
            {
                if (pad.ControleerToegangsCode(toegangsCode))
                {
                    pad.HuidigeActie.IsUitgevoerd = true;
                    TempData["message"] = "De code is juist, de zoektocht gaat verder!";
                    _padRepository.SaveChanges();
                } else
                {
                    TempData["error"] = $"{toegangsCode} is fout!";
                }
            }
            catch (InvalidOperationException e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction(nameof(Index), new { padId });

        }

        /// <summary>
        ///     Controleert het meegegeven pad
        /// </summary>
        /// <param name="padId">Id van het te te controleren Pad</param>
        /// <returns>een Json object met isGeblokkeerd en isVergrendeld properties</returns>
        [HttpGet]
        public JsonResult CheckPad(string padId)
        {
            int id = 0;
            Pad pad = null;
            if (int.TryParse(padId, out id))
                pad = _padRepository.GetById(int.Parse(padId));

            return Json(new {
                isGeblokkeerd = (pad?.PadState.StateName == "Geblokkeerd"),
                isVergrendeld = (pad?.PadState.StateName == "Vergrendeld")
            });
        }
    }
}