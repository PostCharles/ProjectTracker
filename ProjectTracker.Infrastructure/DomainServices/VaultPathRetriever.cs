using ProjectTracker.Core.Enumerations;
using ProjectTracker.Core.Interfaces.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Infrastructure.DomainServices
{
    public class VaultPathRetriever : IVaultPathRetriever
    {
        public const string CONNECTION_STRINGS = "ConnectionStrings";

        private readonly Dictionary<VaultArea, string> _vaultAreaToPath;

        public VaultPathRetriever()
        {
            _vaultAreaToPath = new Dictionary<VaultArea, string>
            {
                {VaultArea.ConnectionStrings, CONNECTION_STRINGS}
            };
        }

        public string Get(VaultArea area)
        {
            if (!_vaultAreaToPath.ContainsKey(area)) throw new KeyNotFoundException($"Vault path {area} was not found");
            return _vaultAreaToPath[area];
        }
    }
}
