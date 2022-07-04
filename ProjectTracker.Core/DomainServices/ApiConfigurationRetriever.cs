using Microsoft.Extensions.Configuration;
using ProjectTracker.Core.Enumerations;
using ProjectTracker.Core.Interfaces.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.DomainServices
{
    public class ApiConfigurationRetriever : IConfigurationRetriever
    {
        public const string VAULT_TOKEN = "VAULT_TOKEN";
        public const string VAULT_URL = "VAULT_URL";
        public const string VAULT_MOUNT_POINT = "VAULT_MOUNT_POINT";

        private readonly Dictionary<ConfigurationParameter, string> _parametersToConfigKey;
        private readonly IConfiguration _configuration;

        public ApiConfigurationRetriever(IConfiguration configuration)
        {
            _configuration = configuration;
            _parametersToConfigKey = new()
            {
                { ConfigurationParameter.VaultMountPoint, VAULT_MOUNT_POINT },
                { ConfigurationParameter.VaultToken, VAULT_TOKEN },
                { ConfigurationParameter.VaultUrl, VAULT_URL },
            };
        }

        public string Get(ConfigurationParameter parameter)
        {
            if (!_parametersToConfigKey.ContainsKey(parameter)) throw new KeyNotFoundException($"The startup parameter {parameter} was not found.");
            
            string configKey = _parametersToConfigKey[parameter];
                        
            string result = _configuration[configKey];
            if (result == null) throw new KeyNotFoundException($"No configuration found for startup parameter {parameter}.");

            return result;
        }
    }
}
