using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultSharp;

namespace ProjectTracker.Infrastructure.Interfaces.DomainServicies
{
    public interface IVaultClientFactory
    {
        IVaultClient CreateClient();
    }
}
