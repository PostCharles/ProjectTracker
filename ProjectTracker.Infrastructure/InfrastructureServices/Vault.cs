using Microsoft.Extensions.Configuration;
using ProjectTracker.Core.Attributes;
using ProjectTracker.Core.Enumerations;
using ProjectTracker.Core.Interfaces.DomainServices;
using ProjectTracker.Core.Interfaces.InfrastructureServices;
using ProjectTracker.Infrastructure.Interfaces.InfrastructureServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;

namespace ProjectTracker.Infrastructure.InfrastructureServices
{
    public class Vault : IVault
    {
        private string? _mountPoint;
        private string MountPoint => _mountPoint ??= _configurationRetriever.Get(ConfigurationParameter.VaultMountPoint);

        private IVaultClient? _vaultClient;
        private IVaultClient VaultClient => _vaultClient ??= _vaultClientFactory!.CreateClient();

        private readonly IConfigurationRetriever _configurationRetriever;
        private readonly IVaultPathRetriever _vaultPathRetriever;
        private readonly IVaultClientFactory _vaultClientFactory;


        public Vault(IConfigurationRetriever configurationRetriever, 
                     IVaultClientFactory vaultClientFactory,
                     IVaultPathRetriever vaultPathRetriever)
        {
            _configurationRetriever = configurationRetriever;
            _vaultClientFactory = vaultClientFactory;
            _vaultPathRetriever = vaultPathRetriever;
        }


        public async Task<T?> GetSecretAsync<T>() where T: class
        {
            object? attribute = typeof(T).GetCustomAttributes(inherit: false)
                                        .FirstOrDefault(a => a.GetType() == typeof(VaultSecretAttribute));

            if (attribute == null) throw new InvalidOperationException($"{typeof(T).Name} does not use the VaultSecretAttribute");
            
            VaultArea area = ((VaultSecretAttribute)attribute).Area;
            string path = _vaultPathRetriever.Get(area);

            string mountPoint = _configurationRetriever.Get(ConfigurationParameter.VaultMountPoint);

            Secret<SecretData<T>>? secret = await VaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync<T>(path,mountPoint:mountPoint);

            return secret?.Data?.Data;

        }
    }
}
