using g16_dotnet.Models.Domain;
using System;
using Xunit;

namespace g16_dotnet.Tests.Models.Domain
{
    public class OpdrachtTest
    {
        private readonly Opdracht _opdracht;

        public OpdrachtTest()
        {
            _opdracht = new Opdracht("xyz", new Oefening("Opgave 1", 50), new GroepsBewerking("Vermeningvuldig met 2", 2, Operator.vermeningvuldigen));
        }

        #region == ControleerToegangsCode ===
        [Fact]
        public void ControleerToegangsCode_JuisteCode_ReturnsTrue()
        {
            Assert.True(_opdracht.ControleerToegangsCode("xyz"));
        }

        [Fact]
        public void ControleerToegangsCode_FouteCode_ReturnsFalse()
        {
            Assert.False(_opdracht.ControleerToegangsCode("abc"));
        }

        [Fact]
        public void ControleerToegangsCode_CodeIsNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _opdracht.ControleerToegangsCode(null));
        }

        [Fact]
        public void ControleerToegangsCode_CodeIsEmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _opdracht.ControleerToegangsCode(String.Empty));
        }
        #endregion

        #region === ControleerAntwoord ===
        [Fact]
        public void ControleerAntwoord_AntwoordJuist_ReturnsTrue()
        {
            Assert.True(_opdracht.ControleerAntwoord(100));
        }

        [Fact]
        public void ControleerAntwoord_AntwoordFout_ReturnsFalse()
        {
            Assert.False(_opdracht.ControleerAntwoord(90));
        }
        #endregion


    }
}
