using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using ProjectTracker.Core.Enumerations;
using ProjectTracker.Infrastructure.Api.DomainService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.Unit.DomainServices
{
    [Trait("sut", nameof(ProjectTracker.Infrastructure.Api.DomainService.ApiConfigurationRetriever))]
    public class ApiConfigurationRetrieverTest
    {
        ApiConfigurationRetriever _sut;
        Mock<IConfiguration> _configuration;

        public ApiConfigurationRetrieverTest()
        {
            _configuration = new Mock<IConfiguration>();
            _sut = new ApiConfigurationRetriever(_configuration.Object);
        }

        [Fact]
        public void Get_StartupParameterNotFound_ThrowsNotFoundException()
        {
            ConfigurationParameter parameter = (ConfigurationParameter)byte.MaxValue;

            _sut.Invoking(s => s.Get(parameter))
                .Should()
                .Throw<KeyNotFoundException>()
                .WithMessage($"The startup parameter {parameter} was not found.");
        }

        [Fact]
        public void Get_ConfigurationNotFound_ThrowsNotFoundException()
        {
            ConfigurationParameter parameter = ConfigurationParameter.VaultMountPoint;
            _sut.Invoking(s => s.Get(parameter))
                .Should()
                .Throw<KeyNotFoundException>()
                .WithMessage($"No configuration found for startup parameter {parameter}.");
        }

        [Theory]
        [InlineData(ConfigurationParameter.VaultToken, ApiConfigurationRetriever.VAULT_TOKEN, "vault-token")]
        [InlineData(ConfigurationParameter.VaultUrl, ApiConfigurationRetriever.VAULT_URL, "vault-url")]
        [InlineData(ConfigurationParameter.VaultMountPoint, ApiConfigurationRetriever.VAULT_MOUNT_POINT, "vault-mount-point")]
        public void Get_ParameterFound_ReturnsConfigurationValue(ConfigurationParameter parameter, string configKey, string returnValue)
        {
            _configuration.Setup(c => c[configKey]).Returns(returnValue);

            _sut.Get(parameter).Should().Be(returnValue);
        }
    }
}
