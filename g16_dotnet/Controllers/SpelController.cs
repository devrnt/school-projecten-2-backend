using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Controllers {
    //[ServiceFilter(typeof(PadSessionFilter))]
    // Uncomment indien je wenst gebruik te maken van deze sessionfilter als er een pad in de session storage
    // moet opgeslaan worden. De filter is alvast aangemaakt en dan hoef je slechts enkel nog de jsonannotaties
    // in de klasse Pad uncommenten.
    public class SpelController : Controller {
        private readonly IPadRepository _padRepository;
        private Pad _pad;
        public SpelController(IPadRepository padRepository) {
            _padRepository = padRepository;
            _pad = _padRepository.GetById(1);

            //Oefening oefening = new Oefening("Opgave 1", "abc");
            //GroepsBewerking groepsBewerking = new GroepsBewerking("def");
            //string toegangsCode = "xyz";
            //Opdracht testOpdracht = new Opdracht(toegangsCode, oefening, groepsBewerking);
            //Actie testActie = new Actie("Ga naar afhaalchinees", testOpdracht);
            //_pad = new Pad(new List<Opdracht> { testOpdracht }, new List<Actie> { testActie });
        }

        public IActionResult Index() {
            ViewData["voortgang"] = $"{_pad.Voortgang:N0}/{_pad.AantalOpdrachten:N0}";
            return View(_pad.Opdrachten.First().Oefening);
        }

        public IActionResult BeantwoordVraag(string groepsAntwoord) {
            if (_pad.Opdrachten.First().Oefening.ControleerAntwoord(groepsAntwoord)) {
                return View("Actie", _pad.Acties.First());
            }
            TempData["error"] = $"{groepsAntwoord} is fout!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult VoerActieUit(string toegangsCode) {
            if (_pad.Opdrachten.First().ControleerToegangsCode(toegangsCode)) {
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = $"{toegangsCode} is fout!";
            return View("Actie", _pad.Acties.First());
        }
    }
}