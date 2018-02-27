using g16_dotnet.Models.Domain;
using System;
using System.Collections.Generic;
using Xunit;

namespace g16_dotnet.Tests.Models.Domain
{
    public class SessieTest
    {
        private Sessie _sessieAlleDeelnamesBevestigd;
        private Sessie _sessieNogDeelnamesTeBevestigen;

        public SessieTest()
        {
            _sessieAlleDeelnamesBevestigd = new Sessie(123, "Test", "testen", new List<Groep> { new Groep() { DeelnameBevestigd = true } }, new Klas());
            _sessieNogDeelnamesTeBevestigen = new Sessie(321, "Test2", "testen2", new List<Groep> { new Groep() { DeelnameBevestigd = false } }, new Klas());
        }

        #region === ControleerSessie ===
        //[Fact]
        //public void ControleerSessieCode_JuisteCode_ReturnsTrue()
        //{
        //    Assert.True(_sessieAlleDeelnamesBevestigd.ControleerSessieCode(123));
        //}

        //[Fact]
        //public void ControleerSessieCode_FouteCode_ReturnsFalse()
        //{
        //    Assert.False(_sessieAlleDeelnamesBevestigd.ControleerSessieCode(321));
        //}

        //[Fact]
        //public void ControleerSessieCode_CodeIsNegatief_ThrowsArgumentException()
        //{
        //    Assert.Throws<ArgumentException>(() => _sessieAlleDeelnamesBevestigd.ControleerSessieCode(-123));
        //}
        #endregion

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


    }
}
