using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Contract
{
    public interface IAuth
    {
        bool LogIn(string username, string password);
        void LogOut();
        string GetToken();
    }
}
