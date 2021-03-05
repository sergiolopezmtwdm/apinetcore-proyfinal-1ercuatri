using System;
using System.Collections.Generic;
using System.Text;

namespace ApiProducts.Library.Interfaces
{
    public interface ILogin : IDisposable
    {
        Models.User ObtenerLogin(string email, string password);
        //List<Models.User> ObtenerUsers();
    }
}
