using g16_dotnet.Models.Domain;
using System;
using Xunit;

namespace g16_dotnet.Tests.Models.Domain
{
    public class OefeningTest
    {
        private Oefening _oefening;

        public OefeningTest()
        {
            _oefening = new Oefening("Opgave 1", "abc");
        }

        #region == ControleerAntwoord ===
        [Fact]
        public void ControleerAntwoord_JuistAntwoord_ReturnsTrue()
        {
            Assert.True(_oefening.ControleerAntwoord("abc"));
        }

        [Fact]
        public void ControleerAntwoord_FoutAntwoord_ReturnsFalse()
        {
            Assert.False(_oefening.ControleerAntwoord("def"));
        }

        [Fact]
        public void ControleerAntwoord_AntwoordIsNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _oefening.ControleerAntwoord(null));
        }

        [Fact]
        public void ControleerAntwoord_AntwoordIsEmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _oefening.ControleerAntwoord(String.Empty));
        }
        #endregion
    }
}
