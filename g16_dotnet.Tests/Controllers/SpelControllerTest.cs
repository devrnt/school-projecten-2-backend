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
    public class SpelControllerTest
    {
        private readonly SpelController _spelController;
        private readonly DummyApplicationDbContext _context;
        private readonly Mock<IPadRepository> _padRepository;

        public SpelControllerTest()
        {
            _context = new DummyApplicationDbContext();
            _padRepository = new Mock<IPadRepository>();
            _padRepository.Setup(m => m.GetById(1)).Returns(_context.Pad);
            _spelController = new SpelController(_padRepository.Object) { TempData = new Mock<ITempDataDictionary>().Object};
        }

        #region === Index ===
        [Fact]
        public void Index_PassesPadToViewViaModel()
        {
            var result = _spelController.Index(_context.Pad) as ViewResult;
            Assert.Equal(1, (result?.Model as Pad).PadId);
        }

        [Fact]
        public void Index_PassesOpdrachtFaseInViewData()
        {
            var result = _spelController.Index(_context.Pad) as ViewResult;
            Assert.Equal("opdracht", result?.ViewData["fase"]);
        }
        #endregion

        #region === BeantwoordVraag ===
        [Fact]
        public void BeantwoordVraag_FoutAntwoord_RedirectsToIndex()
        {
            var result = _spelController.BeantwoordVraag(1, "100") as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void BeantwoordVraag_AntwoordIsGeenGetal_RedirectsToIndex()
        {
            var result = _spelController.BeantwoordVraag(1, "abc") as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoord_MarksOpdrachtAsComplete()
        {
            Opdracht opdracht = _context.Pad.Opdrachten.First(o => !o.IsVoltooid);
            _spelController.BeantwoordVraag(1, "98");
            Assert.True(opdracht.IsVoltooid);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoord_ReturnsIndexView()
        {
            var result = _spelController.BeantwoordVraag(1, "98") as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordNogOpdrachtenOver_PassesPadToViewViaModel()
        {
            var result = _spelController.BeantwoordVraag(1, "98") as ViewResult;
            Assert.Equal(1, (result?.Model as Pad).PadId);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordNogOpdrachtenOver_PassesActieFaseInViewData()
        {
            var result = _spelController.BeantwoordVraag(1, "98") as ViewResult;
            Assert.Equal("actie", result?.ViewData["fase"]);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordGeenOpdrachtenOver_PassesSchatkistFaseInViewData()
        {
            _spelController.BeantwoordVraag(1, "98");
            var result = _spelController.BeantwoordVraag(1, "70") as ViewResult;
            Assert.Equal("schatkist", result?.ViewData["fase"]);
        }

        #endregion

        #region === VoerActieUit ===
        [Fact]
        public void VoerActieUit_JuisteCode_RedirectsToIndex()
        {
            _context.Pad.Opdrachten.First().IsVoltooid = true;
            var result = _spelController.VoerActieUit(_context.Pad, "toegangsCode678") as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void VoerActieUit_FouteCode_ReturnsIndexView()
        {
            _context.Pad.Opdrachten.First().IsVoltooid = true;
            var result = _spelController.VoerActieUit(_context.Pad, "uvw") as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void VoerActieUit_FouteCode_PassesActieFaseInViewData()
        {
            _context.Pad.Opdrachten.First().IsVoltooid = true;
            var result = _spelController.VoerActieUit(_context.Pad, "uvw") as ViewResult;
            Assert.Equal("actie", result?.ViewData["fase"]);
        }

        [Fact]
        public void VoerActieUit_FouteCode_PassesPadToViewViaModel()
        {
            _context.Pad.Opdrachten.First().IsVoltooid = true;
            var result = _spelController.VoerActieUit(_context.Pad, "uvw") as ViewResult;
            Assert.Equal(1, (result?.Model as Pad).PadId);
        } 

        [Fact]
        public void VoerActieUit_OpdrachtNietOpgelost_RedirectsToActionIndex()
        {                      
            var result = _spelController.VoerActieUit(_context.Pad, null) as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }
        #endregion
    }
}
