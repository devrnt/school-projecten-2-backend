using g16_dotnet.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace g16_dotnet.Tests.Models.Domain
{
    public class OpdrachtTest
    {
        private readonly Opdracht _opdracht;

        public OpdrachtTest()
        {
            _opdracht = new Opdracht("xyz", null, null);
        }

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
    }
}
