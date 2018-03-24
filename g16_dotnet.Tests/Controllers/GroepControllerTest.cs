using g16_dotnet.Controllers;
using g16_dotnet.Models.Domain;
using g16_dotnet.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Linq;
using Xunit;

namespace g16_dotnet.Tests.Controllers
{
    public class GroepControllerTest {
        private readonly DummyApplicationDbContext _context;
        private GroepController _groepController;
        private readonly Mock<IGroepRepository> _mockGroepRepository;

        public GroepControllerTest() {
            _context = new DummyApplicationDbContext();
            _mockGroepRepository = new Mock<IGroepRepository>();
            _mockGroepRepository.Setup(m => m.GetById(1)).Returns(_context.Groep1);        
            _mockGroepRepository.Setup(m => m.GetById(2)).Returns(null as Groep);
            _mockGroepRepository.Setup(m => m.GetById(3)).Returns(_context.Groep2);
            _groepController = new GroepController(_mockGroepRepository.Object) { TempData = new Mock<ITempDataDictionary>().Object };
        }

        #region === Index ===
        [Fact]
        public void Index_ReturnsNotFoundResult()
        {
            var result = _groepController.Index();
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region === KiesGroep ===
        [Fact]
        public void KiesGroep_GeldigeGroep_ReturnsGroepOverzichtView() {
            var session = _context.SessieNogDeelnamesTeBevestigen;
            var result = _groepController.KiesGroep(1, 3) as ViewResult;
            Assert.Equal("GroepOverzicht", result?.ViewName);
        }

        [Fact]
        public void KiesGroep_GeldigeGroep_CallsSaveChanges() {
            var session = _context.SessieNogDeelnamesTeBevestigen;
            var result = _groepController.KiesGroep(1, 3) as ViewResult;
            _mockGroepRepository.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void KiesGroep_GeldigeGroep_PassesSessieIdInViewData()
        {
            var result = _groepController.KiesGroep(1, 3) as ViewResult;
            Assert.Equal(1, result?.ViewData["sessieId"]);
        }

        [Fact]
        public void KiesGroep_GeldigeGroep_DeelNameBevestigdIsFalse() {
            //groep 2 heeft nog niet bevestigd
            var groep = _context.Groep2;
            Assert.False(groep.DeelnameBevestigd);
        }

        [Fact]
        public void KiesGroep_OngeldigeGroep_ReturnsNotFound() {
            var session = _context.SessieNogDeelnamesTeBevestigen;
            var result = _groepController.KiesGroep(1, 1222);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void KiesGroep_GroepIsAlGekozen_RedirectsToIndexInSessieController()
        {
            var result = _groepController.KiesGroep(1, 1) as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Sessie", result?.ControllerName);
        }

        #endregion

        #region === StartSpel ===
        [Fact]
        public void StartSpel_SessieActief_RedirectsToActionIndexInSpelController()
        {
            _context.SessieAlleDeelnamesBevestigd.IsActief = true;
            var result = _groepController.StartSpel(_context.SessieAlleDeelnamesBevestigd, 1) as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Spel", result?.ControllerName);
        }

        [Fact]
        public void StartSpel_SessieActief_PassesPadIdToActionIndexInSpelController()
        {
            _context.SessieAlleDeelnamesBevestigd.IsActief = true;
            var result = _groepController.StartSpel(_context.SessieAlleDeelnamesBevestigd, 1) as RedirectToActionResult;
            Assert.Equal(1, result?.RouteValues.Values.First());
        }

        [Fact]
        public void StartSpel_SessieNietActief_ReturnsGroepOverzichtView()
        {
            var result = _groepController.StartSpel(_context.SessieAlleDeelnamesBevestigd, 1) as ViewResult;
            Assert.Equal("GroepOverzicht", result?.ViewName);
        }

        [Fact]
        public void StartSpel_SessieNietActief_PassesGroepToViewViaModel()
        {
            var result = _groepController.StartSpel(_context.SessieAlleDeelnamesBevestigd, 1) as ViewResult;
            Assert.Equal(1, (result?.Model as Groep).GroepId);
        }

        [Fact]
        public void StartSpel_GroepNietGevonden_ReturnsNotFoundResult()
        {
            var result = _groepController.StartSpel(_context.SessieAlleDeelnamesBevestigd, 2);
            Assert.IsType<NotFoundResult>(result);
        }


        #endregion

        [Fact]
        public void NeemDeel_GroepNotFound_ReturnsNotFoundResult()
        {
            var result = _groepController.NeemDeel(_context.SessieAlleDeelnamesBevestigd, _context.Leerling1, 2);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void NeemDeel_RedirectsToActionValideerSessieCodeInSessieController()
        {
            var result = _groepController.NeemDeel(_context.SessieNogDeelnamesTeBevestigen, _context.Leerling3, 3) as RedirectToActionResult;
            Assert.Equal("ValideerSessieCode", result?.ActionName);
            Assert.Equal("Sessie", result?.ControllerName);
        }

        [Fact]
        public void NeemDeel_PassesSessieCodeToRedirectToAction()
        {
            var result = _groepController.NeemDeel(_context.SessieNogDeelnamesTeBevestigen, _context.Leerling3, 3) as RedirectToActionResult;
            Assert.Equal("321", result?.RouteValues.Values.First());
        }

        [Fact]
        public void NeemDeel_LeerlingInKlas_AddsLeerlingToGroep()
        {
            var result = _groepController.NeemDeel(_context.SessieNogDeelnamesTeBevestigen, _context.Leerling3, 3);
            Assert.Contains(_context.Groep2.Leerlingen, l => l.LeerlingId == _context.Leerling3.LeerlingId);
            _mockGroepRepository.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void NeemDeel_LeerlingNietInKlas_DoesNotChangeNorPersistData()
        {
            var aantal = _context.Groep2.Leerlingen.Count;
            var result = _groepController.NeemDeel(_context.SessieNogDeelnamesTeBevestigen, _context.Leerling1, 3);
            Assert.Equal(aantal, _context.Groep2.Leerlingen.Count);
            _mockGroepRepository.Verify(m => m.SaveChanges(), Times.Never);
        }

    }
}
