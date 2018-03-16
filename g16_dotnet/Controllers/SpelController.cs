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
        /// Geeft de Index pagina weer
        /// </summary>
        /// <param name="pad">Aangeleverd door PadSessionFilter</param>
        /// <returns>
        ///     Index View met als fase opdracht
        /// </returns>
        public IActionResult Index(int padId)
        {
           Pad pad = _padRepository.GetById(padId);
            
            if (pad.PadState == null)
            {
                if (pad.IsGeblokkeerd)
                    pad.PadState = new GeblokkeerdPadState("Geblokkeerd");
                else if (pad.Voortgang == pad.AantalOpdrachten)
                    pad.PadState = new SchatkistPadState("Schatkist");
                else if (pad.IsVergrendeld)
                    pad.PadState = new VergrendeldPadState("Vergrendeld");
                else if (pad.Voortgang <= pad.Acties.Count(a => a.Actie.IsUitgevoerd))
                    pad.PadState = new OpdrachtPadState("Opdracht");
                else
                    pad.PadState = new ActiePadState("Actie");
            }

            return View(pad);
        }

        /// <summary>
        /// Controleert of het groepsantwoord juist is 
        /// </summary>
        /// <param name="padId">Id van het huidige pad</param>
        /// <param name="groepsAntwoord">Het opgegeven antwoord als oplossing voor de huidige opdracht</param>
        /// <returns>
        ///     Bij een fout of geen antwoord RedirectToAction naar de Index
        ///     Bij een juist antwoord maar nog opdrachten over de Index view met als fase actie
        ///     Bij een juist antwoord en geen opdracht meer over de Index view met als fase schatkist
        /// </returns>
        public IActionResult BeantwoordVraag(int padId, string groepsAntwoord)
        {
            Pad pad = _padRepository.GetById(padId);

            if (pad.PadState == null)
            {
                if (pad.IsGeblokkeerd)
                    pad.PadState = new GeblokkeerdPadState("Geblokkeerd");
                else if (pad.Voortgang == pad.AantalOpdrachten)
                    pad.PadState = new SchatkistPadState("Schatkist");
                else if (pad.IsVergrendeld)
                    pad.PadState = new VergrendeldPadState("Vergrendeld");
                else 
                    pad.PadState = new OpdrachtPadState("Opdracht");

            }

            if (pad == null)
                return NotFound();
            if (groepsAntwoord == null || groepsAntwoord.Trim().Length == 0)
                TempData["error"] = "Geef een antwoord in!";
            else
            {
                try
                {
                    Opdracht huidig = pad.HuidigeOpdracht;
                    if (pad.ControleerAntwoord(int.Parse(groepsAntwoord)))
                    {
                        TempData["message"] = "Juist antwoord, goed zo!";
                        _padRepository.SaveChanges();
                        return View("Index", pad);
                    }
                    TempData["error"] = $"{groepsAntwoord} is fout!";
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

            return View("Index",pad);
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
        public IActionResult VoerActieUit(int padId, string toegangsCode)
        {
            Pad pad = _padRepository.GetById(padId);

            if (pad.PadState == null)
            {
                if (pad.IsGeblokkeerd)
                    pad.PadState = new GeblokkeerdPadState("Geblokkeerd");
                else if (pad.Voortgang == pad.AantalOpdrachten)
                    pad.PadState = new SchatkistPadState("Schatkist");
                else if (pad.IsVergrendeld)
                    pad.PadState = new VergrendeldPadState("Vergrendeld");
                else 
                    pad.PadState = new ActiePadState("Actie");
               
            }

            try
            {
                if (pad.ControleerToegangsCode(toegangsCode))
                {
                    pad.HuidigeActie.IsUitgevoerd = true;
                    TempData["message"] = "De code is juist, de zoektocht gaat verder!";
                    _padRepository.SaveChanges();
                    return View("Index",pad);
                }
                TempData["error"] = $"{toegangsCode} is fout!";
            }
            catch (InvalidOperationException e)
            {
                TempData["error"] = e.Message;
            } catch (ArgumentException e)
            {
                TempData["error"] = e.Message;
            }
            
            return View("Index", pad);
        }

        [HttpGet]
        public JsonResult IsPadGeblokkeerd(Pad pad)
        {
            return Json(new { isGeblokkeerd = (pad.PadState.StateName == "Geblokkeerd") });
        }

        [HttpGet]
        public JsonResult IsPadVergendeld(Pad pad)
        {
            return Json(new { isVergrendeld = (pad.PadState.StateName == "Vergrendeld") });
        }
    }
}