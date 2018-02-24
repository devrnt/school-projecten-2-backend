using g16_dotnet.Controllers;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace g16_dotnet.Tests.Controllers
{
    public class SpelControllerTest
    {
        private readonly SpelController _spelController;
        private Pad _pad;
        private IEnumerable<Opdracht> _lijstMet1Opdracht;
        private IEnumerable<Opdracht> _lijstMet2Opdrachten;

        public SpelControllerTest()
        {
            Oefening oefening = new Oefening("Opgave 1", "abc");
            GroepsBewerking groepsBewerking = new GroepsBewerking("def");
            string toegangsCode = "xyz";
            Opdracht testOpdracht = new Opdracht(toegangsCode, oefening, groepsBewerking) { VolgNr = 1 };
            Opdracht testOpdracht2 = new Opdracht(toegangsCode, oefening, groepsBewerking) { VolgNr = 2 };
            _lijstMet1Opdracht = new List<Opdracht> { testOpdracht };
            _lijstMet2Opdrachten = new List<Opdracht> { testOpdracht, testOpdracht2 };
            Actie testActie = new Actie("Ga naar afhaalchinees", testOpdracht);
            _pad = new Pad(_lijstMet1Opdracht, new List<Actie> { testActie }) { PadId = 1 };
            _spelController = new SpelController() { TempData = new Mock<ITempDataDictionary>().Object};
        }

        #region === Index ===
        [Fact]
        public void Index_PassesPadToViewViaModel()
        {
            var result = _spelController.Index(_pad) as ViewResult;
            Assert.Equal(1, (result?.Model as Pad).PadId);
        }

        [Fact]
        public void Index_PassesOpdrachtFaseInViewData()
        {
            var result = _spelController.Index(_pad) as ViewResult;
            Assert.Equal("opdracht", result?.ViewData["fase"]);
        }
        #endregion

        #region === BeantwoordVraag ===
        [Fact]
        public void BeantwoordVraag_FoutAntwoord_RedirectsToIndex()
        {
            var result = _spelController.BeantwoordVraag(_pad, "def") as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoord_MarksOpdrachtAsComplete()
        {
            Opdracht opdracht = _pad.Opdrachten.First(o => !o.isVoltooid);
            _spelController.BeantwoordVraag(_pad, "abc");
            Assert.True(opdracht.isVoltooid);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoord_ReturnsIndexView()
        {
            _pad.Opdrachten = _lijstMet2Opdrachten;
            var result = _spelController.BeantwoordVraag(_pad, "abc") as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordNogOpdrachtenOver_PassesPadToViewViaModel()
        {
            _pad.Opdrachten = _lijstMet2Opdrachten;
            var result = _spelController.BeantwoordVraag(_pad, "abc") as ViewResult;
            Assert.Equal(1, (result?.Model as Pad).PadId);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordNogOpdrachtenOver_PassesActieFaseInViewData()
        {
            _pad.Opdrachten = _lijstMet2Opdrachten;
            var result = _spelController.BeantwoordVraag(_pad, "abc") as ViewResult;
            Assert.Equal("actie", result?.ViewData["fase"]);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoordGeenOpdrachtenOver_PassesSchatkistFaseInViewData()
        {
            var result = _spelController.BeantwoordVraag(_pad, "abc") as ViewResult;
            Assert.Equal("schatkist", result?.ViewData["fase"]);
        }
        #endregion

        #region === VoerActieUit ===
        [Fact]
        public void VoerActieUit_JuisteCode_RedirectsToIndex()
        {
            var result = _spelController.VoerActieUit(_pad, "xyz") as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void VoerActieUit_FouteCode_ReturnsIndexView()
        {
            var result = _spelController.VoerActieUit(_pad, "uvw") as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void VoerActieUit_FouteCode_PassesActieFaseInViewData()
        {
            var result = _spelController.VoerActieUit(_pad, "uvw") as ViewResult;
            Assert.Equal("actie", result?.ViewData["fase"]);
        }

        [Fact]
        public void VoerActieUit_FouteCode_PassesPadToViewViaModel()
        {
            var result = _spelController.VoerActieUit(_pad, "uvw") as ViewResult;
            Assert.Equal(1, (result?.Model as Pad).PadId);
        } 

        [Fact]
        public void VoerActieUit_OpdrachtNietOpgelost_RedirectsToActionIndex()
        {
            var result = _spelController.VoerActieUit(_pad, "uvw") as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }
        #endregion
    }
}
