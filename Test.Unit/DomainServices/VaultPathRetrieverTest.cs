using FluentAssertions;
using ProjectTracker.Core.Enumerations;
using ProjectTracker.Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.Unit.DomainServices
{
    [Trait("sut", nameof(VaultPathRetriever))]
    public class VaultPathRetrieverTest
    {
        private readonly VaultPathRetriever _sut;

        public VaultPathRetrieverTest()
        {
            _sut = new VaultPathRetriever();
        }

        [Fact]
        public void Get_VaultPathNotFound_Throws()
        {
            _sut.Invoking(s => s.Get((VaultArea)byte.MaxValue))
                .Should()
                .Throw<KeyNotFoundException>()
                .WithMessage($"Vault path {byte.MaxValue} was not found");
        }

        [Theory]
        [InlineData(VaultArea.ConnectionStrings, VaultPathRetriever.CONNECTION_STRINGS)]
        public void Get_ValidPathFound_ReturnsPath(VaultArea area, string expectedResult)
        {
            _sut.Get(area).Should().Be(expectedResult);
        }
    }
}
