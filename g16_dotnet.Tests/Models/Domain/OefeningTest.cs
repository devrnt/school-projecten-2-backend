using g16_dotnet.Models.Domain;
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
    }
}
