using g16_dotnet.Models.Domain;
using g16_dotnet.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace g16_dotnet.Tests.Models.Domain
{
    public class SessieTest
    {
        private readonly DummyApplicationDbContext _context;
        private readonly Sessie _sessie;

        public SessieTest()
        {
            _context = new DummyApplicationDbContext();
            _sessie = _context.SessieAlleDeelnamesBevestigd;
        }

        #region === WijzigGroepen ===
        [Fact]
        public void WijzigGroepen_BlokkeerBehaviourBehaviourId0_SetsPadGeblokkeerdInAllGroep()
        {
            _sessie.WijzigGroepen(0, 0);
            Assert.True(_sessie.Groepen.All(g => g.Pad.PadState.StateName == "Geblokkeerd"));
        }

        [Fact]
        public void WijzigGroepen_BlokkeerBehaviourBehaviourId1_SetsPadGedeblokkeerdInAllGroep()
        {
            _sessie.Groepen.All(g => { g.BlokkeerPad(); return true;  });
            _sessie.WijzigGroepen(1, 0);
            Assert.True(_sessie.Groepen.All(g => g.Pad.PadState.StateName == "Opdracht"));
        }

        [Fact]
        public void WijzigGroepen_BlokkeerBehaviourBehaviourId2_SetsPadOntgrendeldInAllGroep()
        {
            _sessie.Groepen.All(g => { g.Pad.PadState = new VergrendeldPadState("Vergrendeld"); return true; });
            _sessie.WijzigGroepen(2, 0);
            Assert.True(_sessie.Groepen.All(g => g.Pad.PadState.StateName == "Opdracht"));
        }
        #endregion

    }
}
