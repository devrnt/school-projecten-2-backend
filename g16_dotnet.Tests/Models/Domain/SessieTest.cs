using g16_dotnet.Models.Domain;
using System;
using System.Collections.Generic;
using Xunit;

namespace g16_dotnet.Tests.Models.Domain
{
    public class SessieTest
    {
        private Sessie _sessie;

        public SessieTest()
        {
            _sessie = new Sessie("Test", "testen", 123, new List<Groep>(), new Klas());
        }

        [Fact]
        public void ControleerSessieCode_JuisteCode_ReturnsTrue()
        {
            Assert.True(_sessie.ControleerSessieCode(123));
        }

        [Fact]
        public void ControleerSessieCode_FouteCode_ReturnsFalse()
        {
            Assert.False(_sessie.ControleerSessieCode(321));
        }

        [Fact]
        public void ControleerSessieCode_CodeIsNegatief_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sessie.ControleerSessieCode(-123));
        }
    }
}
