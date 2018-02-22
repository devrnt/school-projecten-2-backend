using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace g16_dotnet.Controllers
{
    public class SpelController : Controller
    {
        private Oefening _testOefening = new Oefening("Opgave 1", "abc");

        public IActionResult Index()
        {
            return View(_testOefening);
        }

        public IActionResult BeantwoordVraag(string groepsAntwoord)
        {
            if (_testOefening.ControleerAntwoord(groepsAntwoord))
            {
                return View("Actie");
            }
            TempData["error"] = $"{groepsAntwoord} is fout!";
            return RedirectToAction(nameof(Index));
        }
    }
}