using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace g16_dotnet.Controllers
{
    public class SpelController : Controller
    {
        private readonly Opdracht _testOpdracht;
        private readonly Actie _testActie;

        public SpelController()
        {
            Oefening oefening = new Oefening("Opgave 1", "abc");
            GroepsBewerking groepsBewerking = new GroepsBewerking("def");
            string toegangsCode = "xyz";
            _testOpdracht = new Opdracht(toegangsCode, oefening, groepsBewerking);
            _testActie = new Actie("Ga naar afhaalchinees", _testOpdracht);
        }

        public IActionResult Index()
        {
            return View(_testOpdracht.Oefening);
        }

        public IActionResult BeantwoordVraag(string groepsAntwoord)
        {
            if (_testOpdracht.Oefening.ControleerAntwoord(groepsAntwoord))
            {
                return View("Actie", _testActie);
            }
            TempData["error"] = $"{groepsAntwoord} is fout!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult VoerActieUit(string toegangsCode)
        {
            if (toegangsCode == _testOpdracht.ToegangsCode)
            {
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = $"{toegangsCode} is fout!";
            return View("Actie", _testActie);
        }
    }
}