using FluentAssertions;
using Moq;
using ProjectTracker.Core.Enumerations;
using ProjectTracker.Core.Interfaces.DomainServices;
using ProjectTracker.Core.Interfaces.InfrastructureServices;
using ProjectTracker.Infrastructure.InfrastructureServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultSharp;
using Xunit;

namespace Test.Unit.DomainServices
{
    [Trait("sut", nameof(VaultClientFactory))]
    public class VaultClientFactoryTest
    {
        private const string TEST_TOKEN = "TOKEN";
        private const string TEST_URL = "https://127.0.0.1:4000";

        private readonly Mock<IConfigurationRetriever> _configurationRetreiver;
        private readonly VaultClientFactory _sut;

        public VaultClientFactoryTest()
        {
            _configurationRetreiver = new Mock<IConfigurationRetriever>();
            _configurationRetreiver.Setup(c => c.Get(ConfigurationParameter.VaultToken)).Returns(TEST_TOKEN);
            _configurationRetreiver.Setup(c => c.Get(ConfigurationParameter.VaultUrl)).Returns(TEST_URL);

            _sut = new VaultClientFactory(_configurationRetreiver.Object);
        }

        [Fact]
        public void CreateClient_UsesUrlFromConfiguration()
        {
            IVaultClient client = _sut.CreateClient();
            client.Settings.VaultServerUriWithPort.Should().Be(TEST_URL);
        }

        [Fact]
        public void CreateClient_UsesTokenFromConfiguration()
        {
            IVaultClient client = _sut.CreateClient();
            _configurationRetreiver.Verify(c => c.Get(ConfigurationParameter.VaultToken));
        }

    }
}
