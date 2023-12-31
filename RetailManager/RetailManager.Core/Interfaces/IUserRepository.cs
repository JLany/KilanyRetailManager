﻿using RetailManager.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
    }
}
