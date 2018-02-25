using g16_dotnet.Controllers;
using g16_dotnet.Models.Domain;
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

        public SessieControllerTest()
        {
            _context = new DummyApplicationDbContext();
            _mockSessieRepository = new Mock<ISessieRepository>();
            _mockSessieRepository.Setup(m => m.GetById(1)).Returns(_context.Sessie1);
            _controller = new SessieController(_mockSessieRepository.Object) { TempData = new Mock<ITempDataDictionary>().Object };
        }

        [Fact]
        public void Index_PassesCodeIngegevenFalseViaViewData()
        {
            var result = _controller.Index() as ViewResult;
            Assert.False((bool)result?.ViewData["codeIngegeven"]);
        }

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
    }
}
