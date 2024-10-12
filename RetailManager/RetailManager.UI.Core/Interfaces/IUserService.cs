using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface IUserService
    {
        Task LoadLoggedInUserInfoAsync();
    }
}
