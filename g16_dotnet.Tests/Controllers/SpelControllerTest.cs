﻿using g16_dotnet.Controllers;
using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace g16_dotnet.Tests.Controllers
{
    public class SpelControllerTest
    {
        private readonly SpelController _spelController;

        public SpelControllerTest()
        {
            _spelController = new SpelController() { TempData = new Mock<ITempDataDictionary>().Object};
        }

        [Fact]
        public void Index_PassesOefeningToViewViaModel()
        {
            var result = _spelController.Index() as ViewResult;
            Assert.Equal("Opgave 1", (result?.Model as Oefening).Opgave);
        }

        [Fact]
        public void BeantwoordVraag_FoutAntwoord_RedirectsToIndex()
        {
            var result = _spelController.BeantwoordVraag("def") as RedirectToActionResult;
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void BeantwoordVraag_JuistAntwoord_ReturnsActieView()
        {
            var result = _spelController.BeantwoordVraag("abc") as ViewResult;
            Assert.Equal("Actie", result?.ViewName);
        }
    }
}