using g16_dotnet.Controllers;
using g16_dotnet.Models.Domain;
using g16_dotnet.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace g16_dotnet.Tests.Controllers {
    public class GroepControllerTest {
        private readonly DummyApplicationDbContext _context;
        private GroepController _groepController;
        private readonly Mock<IGroepRepository> _mockGroepRepository;

        public GroepControllerTest() {
            _context = new DummyApplicationDbContext();
            _mockGroepRepository = new Mock<IGroepRepository>();
            _mockGroepRepository.Setup(m => m.GetById(1)).Returns(_context.Groep1);
            _mockGroepRepository.Setup(m => m.GetById(2)).Returns(null as Groep);
            _groepController = new GroepController(_mockGroepRepository.Object) { TempData = new Mock<ITempDataDictionary>().Object };
        }

        #region === KiesGroep ===
        [Fact]
        public void KiesGroep_GeldigeGroep_ReturnsGroepOverzichtView() {
            var session = _context.SessieNogDeelnamesTeBevestigen;
            var result = _groepController.KiesGroep(session, 1) as ViewResult;
            Assert.Equal("GroepOverzicht", result?.ViewName);
        }

        [Fact]
        public void KiesGroep_GeldigeGroep_CallsSaveChanges() {
            var session = _context.SessieNogDeelnamesTeBevestigen;
            var result = _groepController.KiesGroep(session, 1) as ViewResult;
            _mockGroepRepository.Verify(m => m.SaveChanges(), Times.Once());
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
            var result = _groepController.KiesGroep(session, 1222);
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region === StartSpel ===
        [Fact]
        public void StartSpel_SessieActief_RedirectsToActionIndexInSpelController()
        {
            var sessie = _context.SessieAlleDeelnamesBevestigd;
            sessie.IsActief = true;
            var result = _groepController.StartSpel(sessie, 1) as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Spel", result?.ControllerName);
        }

        [Fact]
        public void StartSpel_SessieNietActief_ReturnsGroepOverzichtView()
        {
            var result = _groepController.StartSpel(_context.SessieNogDeelnamesTeBevestigen, 1) as ViewResult;
            Assert.Equal("GroepOverzicht", result?.ViewName);
        }

        [Fact]
        public void StartSpel_SessieNietActief_PassesGroepToViewViaModel()
        {
            var result = _groepController.StartSpel(_context.SessieNogDeelnamesTeBevestigen, 1) as ViewResult;
            Assert.Equal(_context.Groep1.Groepsnaam, (result?.Model as Groep).Groepsnaam);
        }

        [Fact]
        public void StartSpel_GroepNietGevonden_ReturnsNotFoundResult()
        {
            var result = _groepController.StartSpel(_context.SessieAlleDeelnamesBevestigd, 2);
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

    }
}
