using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Innovation.Mobile.App.Repository.Interface.Service
{
    public interface IAuthService
    {
        Task<bool> CheckAuth();
        Task<bool> Login();
        Task Logout();
    }
}
