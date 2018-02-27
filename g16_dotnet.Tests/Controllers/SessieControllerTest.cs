using g16_dotnet.Controllers;
using g16_dotnet.Models.Domain;
using g16_dotnet.Models.SessieViewModel;
using g16_dotnet.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace g16_dotnet.Tests.Controllers
{
    public class SessieControllerTest
    {
        private readonly SessieController _controller;
        private readonly Mock<ISessieRepository> _mockSessieRepository;
        private readonly DummyApplicationDbContext _context;
        private Leerkracht _leerkracht;

        public SessieControllerTest()
        {
            _context = new DummyApplicationDbContext();
            _leerkracht = _context.Leerkracht1;
            _mockSessieRepository = new Mock<ISessieRepository>();
            _mockSessieRepository.Setup(m => m.GetById(123)).Returns(_context.SessieAlleDeelnamesBevestigd);
            _mockSessieRepository.Setup(m => m.GetById(321)).Returns(null as Sessie);
            _mockSessieRepository.Setup(m => m.GetById(456)).Returns(_context.SessieNogDeelnamesTeBevestigen);
            _mockSessieRepository.Setup(m => m.GetAll()).Returns(new List<Sessie> { _context.SessieAlleDeelnamesBevestigd });
            _controller = new SessieController(_mockSessieRepository.Object) { TempData = new Mock<ITempDataDictionary>().Object };
        }

        #region === Index ===
        [Fact]
        public void Index_PassesCodeIngegevenFalseViaViewData()
        {
            var result = _controller.Index() as ViewResult;
            Assert.False((bool)result?.ViewData["codeIngegeven"]);
        }
        #endregion

        #region === ValideerSessieCode ===
        [Fact]
        public void ValideerSessieCode_JuisteCode_ReturnsIndexView()
        {
            var result = _controller.ValideerSessiecode(123) as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void ValideerSessieCode_JuisteCode_PassesCodeIngegevenTrueViaViewData()
        {
            var result = _controller.ValideerSessiecode(123) as ViewResult;
            Assert.True((bool)result?.ViewData["codeIngegeven"]);
        }

        [Fact]
        public void ValideerSessieCode_JuisteCode_PassesGroepenToViewViaModel()
        {
            var result = _controller.ValideerSessiecode(123) as ViewResult;
            Assert.Single(result?.Model as IEnumerable<Groep>);
        }

        [Fact]
        public void ValideerSessieCode_FouteCode_RedirectsToActionIndex()
        {
            var result = _controller.ValideerSessiecode(321) as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }
        #endregion

        #region === BeheerSessies ===
        [Fact]
        public void BeheerSessies_PassesSessiesToViewViaModel()
        {
            var result = _controller.BeheerSessies(_leerkracht) as ViewResult;
            Assert.Single((result?.Model as IEnumerable<SessieLijstViewModel>));
        }

        #endregion

        #region === SelecteerSessie ===
        [Fact]
        public void SelecteerSessie_PassesSessieViewModelToViewViaModel()
        {
            var result = _controller.SelecteerSessie(_leerkracht, 123) as ViewResult;
            var svm = new SessieDetailViewModel(_context.SessieAlleDeelnamesBevestigd);
            Assert.Equal(svm.SessieNaam, (result?.Model as SessieDetailViewModel).SessieNaam);
        }

        [Fact]
        public void SelecteerSessie_SessieNotFound_ReturnsNotFoundResult()
        {
            _mockSessieRepository.Setup(m => m.GetById(2)).Returns(null as Sessie);
            var result = _controller.SelecteerSessie(_leerkracht, 2);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void SelecteerSessie_ReturnsSessieDetailView()
        {
            var result = _controller.SelecteerSessie(_leerkracht, 123) as ViewResult;
            Assert.Equal("SessieDetail", result?.ViewName);
        }
        #endregion

        #region === ActiveerSessie ===
        [Fact]
        public void ActiveerSessie_AlleDeelnamesBevestigd_RedirectsToBeheerSessies()
        {
            var result = _controller.ActiveerSessie(_leerkracht, 123) as RedirectToActionResult;
            Assert.Equal("BeheerSessies", result?.ActionName);
        }

        [Fact]
        public void ActiveerSessie_SessieNotFound_ReturnsNotFoundResult()
        {
            var result = _controller.ActiveerSessie(_leerkracht, 2);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ActiveerSessie_NogDeelnamesTeBevestigen_ReturnsSessieDetailView()
        {
            var result = _controller.ActiveerSessie(_leerkracht, 456) as ViewResult;
            Assert.Equal("SessieDetail", result?.ViewName);
        }

        [Fact]
        public void ActiveerSessie_NogDeelnamesTeBevestigen_PassesSessieViewModelToViewViaModel()
        {
            var result = _controller.ActiveerSessie(_leerkracht, 456) as ViewResult;
            var svm = new SessieDetailViewModel(_context.SessieNogDeelnamesTeBevestigen);
            Assert.Equal(svm.SessieNaam, (result?.Model as SessieDetailViewModel).SessieNaam);
        }

        #endregion
    }
}
