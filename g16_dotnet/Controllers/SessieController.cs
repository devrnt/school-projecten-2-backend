using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace g16_dotnet.Controllers
{
    public class SessieController : Controller {
        private readonly ISessieRepository _sessieRepository;

        public SessieController(ISessieRepository sessieRepository) {
            _sessieRepository = sessieRepository;
        }

        public IActionResult Index() {
            ViewData["codeIngegeven"] = false;
            return View();
        }

        public IActionResult ValideerSessiecode(int code)
        {
            Sessie sessie = _sessieRepository.GetById(1);
            try
            {
                if (sessie.ControleerSessieCode(code))
                {
                    ViewData["codeIngegeven"] = true;
                    return View("Index", sessie.Groepen);
                } else
                {
                    TempData["error"] = $"{code} is niet juist!";
                }
            }
            catch (System.ArgumentException e)
            {
                TempData["error"] = e.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}