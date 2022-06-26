using ProjectTracker.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Attributes
{
    [AttributeUsage(System.AttributeTargets.Class)]
    public class VaultSecretAttribute : Attribute
    {
        public VaultArea Area { get; set; }
        public VaultSecretAttribute(VaultArea area)
        {
            Area = area;
        }
    }
}
