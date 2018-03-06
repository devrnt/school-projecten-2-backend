using g16_dotnet.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace g16_dotnet.Tests.Models.Domain
{
    public class SessieTest
    {
        private Sessie _sessieAlleDeelnamesBevestigd;
        private Sessie _sessieNogDeelnamesTeBevestigen;

        public SessieTest()
        {
            _sessieAlleDeelnamesBevestigd = new Sessie(123, "Test", "testen", new List<Groep> { new Groep() { DeelnameBevestigd = true, Pad = new Pad() } }, new Klas());
            _sessieNogDeelnamesTeBevestigen = new Sessie(321, "Test2", "testen2", new List<Groep> { new Groep() { DeelnameBevestigd = false, Pad = new Pad() } }, new Klas());
        }

        #region === ActiveerSessie ===
        [Fact]
        public void ActiveerSessie_AlleGroepenDeelgenomen_SetsIsActiefTrue()
        {
            _sessieAlleDeelnamesBevestigd.ActiveerSessie();
            Assert.True(_sessieAlleDeelnamesBevestigd.IsActief);
        }

        [Fact]
        public void ActiveerSessie_NogGroepenTeBevestigen_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _sessieNogDeelnamesTeBevestigen.ActiveerSessie());
        }
        #endregion

        #region === BlokkeerAlleGroepen ===
        [Fact]
        public void BlokkeerAlleGroepen_SetsIsGeblokkeerdInPadOfAllGroepToTrue()
        {
            _sessieAlleDeelnamesBevestigd.BlokkeerAlleGroepen();
            Assert.True(_sessieAlleDeelnamesBevestigd.Groepen.All(g => g.Pad.IsGeblokkeerd));
        }
        #endregion

        #region === DeblokkeerAlleGroepen ===
        [Fact]
        public void DeblokkeerAlleGroepen_SetsIsGeblokkeerdInPadOfAllGroepToFalse()
        {
            _sessieAlleDeelnamesBevestigd.DeblokkeerAlleGroepen();
            Assert.True(_sessieAlleDeelnamesBevestigd.Groepen.All(g => !g.Pad.IsGeblokkeerd));
        }
        #endregion
    }
}
