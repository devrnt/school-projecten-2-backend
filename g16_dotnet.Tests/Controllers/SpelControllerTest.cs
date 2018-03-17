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
            _padRepository.Setup(m => m.GetById(5)).Returns(_context.PadMet1Opdracht);
            _spelController = new SpelController(_padRepository.Object) { TempData = new Mock<ITempDataDictionary>().Object};
        }

        #region === Index ===
        [Fact]
        public void Index_PassesPadToViewViaModel()
        {
            var result = _spelController.Index(_context.Pad.PadId) as ViewResult;
            Assert.Equal(1, (result?.Model as Pad).PadId);
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
        public void BeantwoordVraag_FoutAntwoordTeveelPogingen_SetsVergendeldPadState()
        {
            _spelController.BeantwoordVraag(1, "100");
            _spelController.BeantwoordVraag(1, "100");
            _spelController.BeantwoordVraag(1, "100");
            Assert.Equal("Vergrendeld", _context.Pad.PadState.StateName);
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
            Opdracht opdracht = _context.Pad.Opdrachten.First(po => !po.Opdracht.IsVoltooid).Opdracht;
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
            Assert.Equal(1, (result?.Model as Pad)?.PadId);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordNogOpdrachtenOver_PadHeeftActiePadState()
        {
            var result = _spelController.BeantwoordVraag(1, "98") as ViewResult;
            Assert.Equal("Actie", (result?.Model as Pad)?.PadState.StateName);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordGeenOpdrachtenOver_PadHeeftSchatkistPadState()
        {
            var result = _spelController.BeantwoordVraag(5, "98") as ViewResult;
            Assert.Equal("Schatkist", (result?.Model as Pad)?.PadState.StateName);
        }

        #endregion

        #region === VoerActieUit ===
        [Fact]
        public void VoerActieUit_JuisteCode_RedirectsToIndex()
        {
            _context.Pad.HuidigeOpdracht.IsVoltooid = true;
            _context.Pad.PadState = new ActiePadState("Actie");
            var result = _spelController.VoerActieUit(_context.Pad.PadId, "toegangsCode678") as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void VoerActieUit_FouteCode_ReturnsIndexView()
        {
            var result = _spelController.VoerActieUit(_context.Pad.PadId, "uvw") as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void VoerActieUit_FouteCode_PassesPadToViewViaModel()
        {
            var result = _spelController.VoerActieUit(_context.Pad.PadId, "uvw") as ViewResult;
            Assert.Equal(1, (result?.Model as Pad).PadId);
        } 
        #endregion
    }
}
