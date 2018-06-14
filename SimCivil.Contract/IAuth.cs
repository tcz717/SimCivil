using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Contract
{
    public interface IAuth
    {
        [Obsolete]
        bool LogIn(string username, string password);
        Task<bool> LogInAsync(string username, string password);
        [Obsolete]
        void LogOut();
        Task LogOutAsync();
        [Obsolete]
        string GetToken();
        Task<string> GetTokenAsync();
    }
}
