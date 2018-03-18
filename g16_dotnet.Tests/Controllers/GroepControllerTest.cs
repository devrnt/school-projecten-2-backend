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
        public void StartSpel_RedirectsToActionIndexInSpelController()
        {
            var result = _groepController.StartSpel(1) as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Spel", result?.ControllerName);
        }

        [Fact]
        public void StartSpel_PassesPadIdToActionIndexInSpelController()
        {
            var result = _groepController.StartSpel(1) as RedirectToActionResult;
            Assert.Equal(1, (result?.RouteValues.Values.First()));
        }

        [Fact]
        public void StartSpel_GroepNietGevonden_ReturnsNotFoundResult()
        {
            var result = _groepController.StartSpel(2);
            Assert.IsType<NotFoundResult>(result);
        }


        #endregion

    }
}
