using g16_dotnet.Controllers;
using g16_dotnet.Models.Domain;
using g16_dotnet.Models.SessieViewModel;
using g16_dotnet.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json;
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
            var result = _controller.ValideerSessiecode("123") as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void ValideerSessieCode_JuisteCode_PassesCodeIngegevenTrueViaViewData()
        {
            var result = _controller.ValideerSessiecode("123") as ViewResult;
            Assert.True((bool)result?.ViewData["codeIngegeven"]);
        }

        [Fact]
        public void ValideerSessieCode_JuisteCode_PassesSessieCodeInViewData()
        {
            var result = _controller.ValideerSessiecode("123") as ViewResult;
            Assert.Equal(_context.SessieAlleDeelnamesBevestigd.SessieCode, int.Parse(result?.ViewData["sessieCode"] as string));

        }

        [Fact]
        public void ValideerSessieCode_JuisteCode_PassesSessieDoelgroepInViewData()
        {
            var result = _controller.ValideerSessiecode("123") as ViewResult;
            Assert.Equal(_context.SessieAlleDeelnamesBevestigd.Doelgroep, JsonConvert.DeserializeObject<DoelgroepEnum>(result?.ViewData["Doelgroep"] as string));

        }

        [Fact]
        public void ValideerSessieCode_JuisteCode_PassesSessieOmschrijvingToViewViaViewData()
        {
            var result = _controller.ValideerSessiecode("123") as ViewResult;
            Assert.Equal(_context.SessieAlleDeelnamesBevestigd.Omschrijving, result?.ViewData["sessieOmschrijving"]);
        }

        [Fact]
        public void ValideerSessieCode_JuisteCode_PassesGroepenToViewViaModel()
        {
            var result = _controller.ValideerSessiecode("123") as ViewResult;
            Assert.Single(result?.Model as IEnumerable<Groep>);
        }

        [Fact]
        public void ValideerSessieCode_FouteCode_RedirectsToActionIndex()
        {
            var result = _controller.ValideerSessiecode("321") as RedirectToActionResult;
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
            var result = _controller.SelecteerSessie(123) as ViewResult;
            var svm = new SessieDetailViewModel(_context.SessieAlleDeelnamesBevestigd);
            Assert.Equal(svm.SessieNaam, (result?.Model as SessieDetailViewModel).SessieNaam);
        }

        [Fact]
        public void SelecteerSessie_SessieNotFound_ReturnsNotFoundResult()
        {
            _mockSessieRepository.Setup(m => m.GetById(2)).Returns(null as Sessie);
            var result = _controller.SelecteerSessie( 2);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void SelecteerSessie_ReturnsSessieDetailView()
        {
            var result = _controller.SelecteerSessie(123) as ViewResult;
            Assert.Equal("SessieDetail", result?.ViewName);
        }

        [Fact]
        public void SelecteerSessie_PassesSelectListInViewData()
        {
            var result = _controller.SelecteerSessie(123) as ViewResult;
            Assert.Equal(2, (result?.ViewData["Doelgroepen"] as SelectList).Count());
        }
        #endregion

        #region === ActiveerSessie ===
        [Fact]
        public void ActiveerSessie_RedirectsToSelecteerSessie()
        {
            var result = _controller.ActiveerSessie(123) as RedirectToActionResult;
            Assert.Equal("SelecteerSessie", result?.ActionName);
        }

        [Fact]
        public void ActiveerSessie_SessieNotFound_ReturnsNotFoundResult()
        {
            var result = _controller.ActiveerSessie( 2);
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region === WijzigGroepen ===
        [Fact]
        public void WijzigGroepen_ReturnsRedirectToActionSelecteerSessie()
        {
            var result = _controller.WijzigGroepen(123, 0) as RedirectToActionResult;
            Assert.Equal("SelecteerSessie", result?.ActionName);
        }

        [Fact]
        public void WijzigGroepen_SessieNotFound_ReturnsNotFoundResult()
        {
            var result = _controller.WijzigGroepen(321, 0);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void WijzigGroepen_PassesSessieIdToActionSelecteerSessie()
        {
            var result = _controller.WijzigGroepen(123, 0) as RedirectToActionResult;
            Assert.Equal(123, result?.RouteValues.Values.First());
        }

        [Fact]
        public void WijzigGroepen_ChangesAndPersistsData()
        {
            var result = _controller.WijzigGroepen(123, 0) as RedirectToActionResult;
            Assert.True(_context.SessieAlleDeelnamesBevestigd.Groepen.All(p => p.Pad.PadState.StateName == "Geblokkeerd"));
            _mockSessieRepository.Verify(m => m.SaveChanges(), Times.Once);
        }
        #endregion


        #region === CheckDeelnames ===
        [Fact]
        public void CheckDeelnames_SessieNotFound_ReturnsNotFoundResult()
        {
            var result = _controller.CheckDeelnames(321);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CheckDeelnames_SessieValid_ReturnsGroepenOverzichtPartialView()
        {
            var result = _controller.CheckDeelnames(123) as PartialViewResult;
            Assert.Equal("_GroepenOverzicht", result?.ViewName);
        }

        [Fact]
        public void CheckDeelnames_SessieValid_PassesSessieDetailViewModelToViewViaModel()
        {
            var result = _controller.CheckDeelnames(123) as PartialViewResult;
            Assert.Equal(123, (result?.Model as SessieDetailViewModel).SessieCode);
        }
        #endregion

        #region === SelecteerDoelgroep ===
        [Fact]
        public void SelecteerDoelGroep_SessieNotFound_ReturnsNotFoundResult()
        {
            var result = _controller.SelecteerDoelgroep(321, 0);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void SelecteerDoelGroep_SessieValidDoelGroepInvalid_DoesNotChangeOrPersistData()
        {
            var sessie = _context.SessieAlleDeelnamesBevestigd;
            var result = _controller.SelecteerDoelgroep(123, -1) as ViewResult;
            Assert.Equal(DoelgroepEnum.Jongeren, sessie.Doelgroep);
            _mockSessieRepository.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void SelecteerDoelGroep_SessieValidDoelGroepValid_DoesNotChangeOrPersistData()
        {
            var sessie = _context.SessieAlleDeelnamesBevestigd;
            var result = _controller.SelecteerDoelgroep(123, 1) as ViewResult;
            Assert.Equal(DoelgroepEnum.Volwassenen, sessie.Doelgroep);
            _mockSessieRepository.Verify(m => m.SaveChanges(), Times.Once);
        }
        #endregion
    }
}
