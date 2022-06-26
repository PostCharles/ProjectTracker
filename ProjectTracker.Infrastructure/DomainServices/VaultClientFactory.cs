using ProjectTracker.Core.Enumerations;
using ProjectTracker.Core.Interfaces.DomainServices;
using ProjectTracker.Infrastructure.Interfaces.DomainServicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace ProjectTracker.Infrastructure.DomainServices
{
    public class VaultClientFactory : IVaultClientFactory
    {
        private readonly IConfigurationRetriever _configurationRetriever;

        public VaultClientFactory(IConfigurationRetriever configurationRetriever)
        {
            _configurationRetriever = configurationRetriever;
        }
        public IVaultClient CreateClient()
        {
            string vaultUrl = _configurationRetriever.Get(ConfigurationParameter.VaultUrl);
            string vaultToken = _configurationRetriever.Get(ConfigurationParameter.VaultToken);

            var authMethod = new TokenAuthMethodInfo(vaultToken);
            var settings = new VaultClientSettings(vaultUrl, authMethod);

            return new VaultClient(settings);
            
        }
    }
}
