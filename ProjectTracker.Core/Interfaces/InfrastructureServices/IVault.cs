﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Interfaces.InfrastructureServices
{
    public interface IVault
    {
        Task<T?> GetSecretAsync<T>() where T: class;
    }
}
