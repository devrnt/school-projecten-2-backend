using g16_dotnet.Filters;
using g16_dotnet.Models.Domain;
using g16_dotnet.Models.SessieViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace g16_dotnet.Controllers
{
    [ServiceFilter(typeof(LeerkrachtFilter))]
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
            catch (ArgumentException e)
            {
                TempData["error"] = e.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult BeheerSessies(Leerkracht leerkracht)
        {
            ICollection<SessieLijstViewModel> sessieLijst = new List<SessieLijstViewModel>();
            foreach (var sessie in leerkracht.InactieveSessies)
                sessieLijst.Add(new SessieLijstViewModel(sessie));
            return View(sessieLijst);
        }

        public IActionResult SelecteerSessie(Leerkracht leerkracht, int sessieId)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();
            return View("SessieDetail", new SessieDetailViewModel(sessie));
        }

        public IActionResult ActiveerSessie(Leerkracht leerkracht, int sessieId)
        {
            Sessie sessie = _sessieRepository.GetById(sessieId);
            if (sessie == null)
                return NotFound();
            try
            {
                sessie.ActiveerSessie();
                _sessieRepository.SaveChanges();
                TempData["message"] = "Sessie is succesvol geactiveerd.";
                return RedirectToAction(nameof(BeheerSessies));
            }
            catch (InvalidOperationException e)
            {
                TempData["error"] = e.Message;
            }
            return View("SessieDetail", new SessieDetailViewModel(sessie));
        }
    }
}