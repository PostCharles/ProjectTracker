using ProjectTracker.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Interfaces.DomainServices
{
    public interface IConfigurationRetriever
    {
        string Get(ConfigurationParameter parameter);
    }
}
