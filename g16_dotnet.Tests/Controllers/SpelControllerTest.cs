using g16_dotnet.Controllers;
using g16_dotnet.Models.Domain;
using g16_dotnet.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json;
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
            _padRepository.Setup(m => m.GetById(2)).Returns(null as Pad);
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

        [Fact]
        public void Index_PadNotFound_ReturnsNotFoundResult()
        {
            var result = _spelController.Index(2);
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region === BeantwoordVraag ===
        [Fact]
        public void BeantwoordVraag_SessieNotFound_ReturnsNotFoundResult()
        {
            var result = _spelController.BeantwoordVraag(2, "1");
            Assert.IsType<NotFoundResult>(result);
        }

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
            PadOpdracht opdracht = _context.Pad.Opdrachten.First(po => !po.IsVoltooid);
            _spelController.BeantwoordVraag(1, "98");
            Assert.True(opdracht.IsVoltooid);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoord_RedirectsToActionIndex()
        {
            var result = _spelController.BeantwoordVraag(1, "98") as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordNogOpdrachtenOver_PassesPadIdToIndex()
        {
            var result = _spelController.BeantwoordVraag(1, "98") as RedirectToActionResult;
            Assert.Equal(1, result?.RouteValues.Values.First());
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordNogOpdrachtenOver_PadHeeftActiePadState()
        {
            _spelController.BeantwoordVraag(1, "98");
            Assert.Equal("Actie", _context.Pad.PadState.StateName);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordGeenOpdrachtenOver_PadHeeftSchatkistPadState()
        {
            _spelController.BeantwoordVraag(5, "98");
            Assert.Equal("Schatkist", _context.PadMet1Opdracht.PadState.StateName);
        }

        [Fact]
        public void BeantwoordVraag_GeenAntwoord_DoesNotChangeOrPersistData()
        {
            PadOpdracht opdracht = _context.Pad.HuidigeOpdracht;
            _spelController.BeantwoordVraag(1, string.Empty);
            Assert.Equal(0, _context.Pad.HuidigeOpdracht.AantalPogingen);
            Assert.False(opdracht.IsVoltooid);
            _padRepository.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void BeantwoordVraag_PadNietInOpdrachtPadState_DoesNotChangeOrPersistData()
        {
            PadOpdracht opdracht = _context.Pad.HuidigeOpdracht;
            _context.Pad.PadState = new ActiePadState("Actie");
            _spelController.BeantwoordVraag(1, "2");
            Assert.Equal(0, _context.Pad.HuidigeOpdracht.AantalPogingen);
            Assert.False(opdracht.IsVoltooid);
            _padRepository.Verify(m => m.SaveChanges(), Times.Never);
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
        public void VoerActieUit_FouteCode_RedirectsToIndex()
        {
            _context.Pad.HuidigeOpdracht.IsVoltooid = true;
            _context.Pad.PadState = new ActiePadState("Actie");
            var result = _spelController.VoerActieUit(_context.Pad.PadId, "uvw") as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);

        }

        [Fact]
        public void VoerActieUit_JuisteCode_PassesPadIdToIndexAction()
        {
            _context.Pad.HuidigeOpdracht.IsVoltooid = true;
            _context.Pad.PadState = new ActiePadState("Actie");
            var result = _spelController.VoerActieUit(_context.Pad.PadId, "toegangsCode678") as RedirectToActionResult;
            Assert.Equal(1, result?.RouteValues.Values.First());
        }

        [Fact]
        public void VoerActieUit_FouteCode_PassesPadIdToIndexAction()
        {
            _context.Pad.HuidigeOpdracht.IsVoltooid = true;
            _context.Pad.PadState = new ActiePadState("Actie");
            var result = _spelController.VoerActieUit(_context.Pad.PadId, "uvw") as RedirectToActionResult;
            Assert.Equal(1, result?.RouteValues.Values.First());
        }

        [Fact]
        public void VoerActieUit_PadNietInActiePadState_DoesNotChangeOrPersistData()
        {
            _context.Pad.PadState = new OpdrachtPadState("Opdracht");
            PadActie actie = _context.Pad.HuidigeActie;
            _spelController.VoerActieUit(1, "abc");
            Assert.False(actie.IsUitgevoerd);
            _padRepository.Verify(m => m.SaveChanges(), Times.Never);
        }
        #endregion
    }
}
