using g16_dotnet.Controllers;
using g16_dotnet.Models.Domain;
using g16_dotnet.Tests.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace g16_dotnet.Tests.Models.Domain
{
    public class SpelTest
    {
        private readonly SpelController _spelController;
        private readonly Mock<IPadRepository> _padRepostiory;
        private readonly DummyApplicationDbContext _dummyContext;
    }
}
