using FluentAssertions;
using Moq;
using ProjectTracker.Core.Attributes;
using ProjectTracker.Core.Enumerations;
using ProjectTracker.Core.Interfaces.DomainServices;
using ProjectTracker.Infrastructure.InfrastructureServices;
using ProjectTracker.Infrastructure.Interfaces.InfrastructureServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.KeyValue;
using VaultSharp.V1.SecretsEngines.KeyValue.V1;
using VaultSharp.V1.SecretsEngines.KeyValue.V2;
using Xunit;

namespace Test.Unit.DomainServices
{
    [Trait("sut", nameof(Vault))]
    public class VaultTest
    {
        private const VaultArea TEST_AREA = VaultArea.ConnectionStrings;

        private readonly Mock<IConfigurationRetriever> _configurationRetreiver;
        private readonly Mock<IVaultClient> _vaultClient;
        private readonly Mock<IVaultClientFactory> _vaultClientFactory;
        private readonly Mock<IVaultClientV1> _vaultV1Client;
        private readonly Mock<ISecretsEngine> _secretsEngine;
        private readonly Mock<IKeyValueSecretsEngine> _keyValueSecretsEngine;
        private readonly Mock<IKeyValueSecretsEngineV1> _keyValueSecretsEngineV1;
        private readonly Mock<IKeyValueSecretsEngineV2> _keyValueSecretsEngineV2;
        private readonly Mock<IVaultPathRetriever> _vaultPathRetreiver;

        private readonly Vault _sut;

        public VaultTest()
        {
            _configurationRetreiver = new Mock<IConfigurationRetriever>();
            _vaultClient = new Mock<IVaultClient>();
            _vaultClientFactory = new Mock<IVaultClientFactory>();
            _vaultV1Client = new Mock<IVaultClientV1>();
            _secretsEngine = new Mock<ISecretsEngine>();
            _keyValueSecretsEngine = new Mock<IKeyValueSecretsEngine>();
            _keyValueSecretsEngineV1 = new Mock<IKeyValueSecretsEngineV1>();
            _keyValueSecretsEngineV2 = new Mock<IKeyValueSecretsEngineV2>();
            _vaultPathRetreiver = new Mock<IVaultPathRetriever>();

            _vaultClient.Setup(c => c.V1).Returns(_vaultV1Client.Object);
            _vaultClientFactory.Setup(c => c.CreateClient()).Returns(_vaultClient.Object);
            _vaultV1Client.Setup(c => c.Secrets).Returns(_secretsEngine.Object);
            _secretsEngine.Setup(e => e.KeyValue).Returns(_keyValueSecretsEngine.Object);
            _keyValueSecretsEngine.Setup(e => e.V1).Returns(_keyValueSecretsEngineV1.Object);
            _keyValueSecretsEngine.Setup(e => e.V2).Returns(_keyValueSecretsEngineV2.Object);
        
            _sut = new Vault(_configurationRetreiver.Object, _vaultClientFactory.Object, _vaultPathRetreiver.Object);
        }

        [Fact]
        public async Task GetSecretAsync_SecretDoesNotUseVaultSecretAttribute_ThrowsInvalidOperationException()
        {
            await _sut.Invoking(async s => await s.GetSecretAsync<NotUsingAttributeSecret>())
                      .Should()
                      .ThrowAsync<InvalidOperationException>()
                      .WithMessage($"{nameof(NotUsingAttributeSecret)} does not use the VaultSecretAttribute");
        }

        [Fact]
        public async Task GetSecretAsync_UsesRetreivedPathForArea()
        {
            string testPath = "test-path";
            _vaultPathRetreiver.Setup(r => r.Get(TEST_AREA))
                               .Returns(testPath);

            UsingAttributeSecret? secret = await _sut.GetSecretAsync<UsingAttributeSecret>();
            _vaultPathRetreiver.Verify(r => r.Get(TEST_AREA));

            _keyValueSecretsEngineV2.Verify(e => e.ReadSecretAsync<UsingAttributeSecret>(testPath,
                                                                             It.IsAny<int?>(),
                                                                             It.IsAny<string>(),
                                                                             It.IsAny<string>()));
        }

        [Fact]
        public async Task GetSecretAsync_UsesMountPointFromConfiguration()
        {
            string mountPoint = "mount-point";
            _configurationRetreiver.Setup(r => r.Get(ConfigurationParameter.VaultMountPoint))
                                   .Returns(mountPoint);


            UsingAttributeSecret? secret = await _sut.GetSecretAsync<UsingAttributeSecret>();
            _vaultPathRetreiver.Verify(r => r.Get(TEST_AREA));
            _keyValueSecretsEngineV2.Verify(e => e.ReadSecretAsync<UsingAttributeSecret>(It.IsAny<string>(),
                                                                                         It.IsAny<int?>(),
                                                                                         mountPoint,
                                                                                         It.IsAny<string>()));
        }

        [Fact]
        public async Task GetSecretAsync_SecretExist_ReturnsSecret()
        {
            string testPath = "test-path";
            _vaultPathRetreiver.Setup(r => r.Get(TEST_AREA))
                               .Returns(testPath);

            string mountPoint = "mount-point";
            _configurationRetreiver.Setup(r => r.Get(ConfigurationParameter.VaultMountPoint))
                                   .Returns(mountPoint);

            var secret = new UsingAttributeSecret();
            Secret<SecretData<UsingAttributeSecret>> secretData = new Secret<SecretData<UsingAttributeSecret>>();
            secretData.Data = new SecretData<UsingAttributeSecret>();
            secretData.Data.Data = secret;
            
            _keyValueSecretsEngineV2.Setup(e => e.ReadSecretAsync<UsingAttributeSecret>(testPath,
                                                                                        null,
                                                                                        mountPoint,
                                                                                        null))
                                    .ReturnsAsync(secretData);

            UsingAttributeSecret? result = await _sut.GetSecretAsync<UsingAttributeSecret>();

            Assert.Equal(secret, result);
        }


        public class NotUsingAttributeSecret
        {
        }

        [VaultSecret(TEST_AREA)]
        public class UsingAttributeSecret
        {

        }

    }
}
