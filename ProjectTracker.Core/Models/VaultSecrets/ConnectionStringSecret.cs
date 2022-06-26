using ProjectTracker.Core.Attributes;
using ProjectTracker.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Models.VaultSecrets
{
    [VaultSecret(VaultArea.ConnectionStrings)]
    public class ConnectionStringSecret
    {
        public string? UserDb { get; set; }
    }
}
