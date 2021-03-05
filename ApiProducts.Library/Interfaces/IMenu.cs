using System;
using System.Collections.Generic;
using System.Text;

namespace ApiProducts.Library.Interfaces
{
    public interface IMenu : IDisposable
    {
        List<Models.Menu> GetMenus();
        List<Models.Menu> GetMenusRol(string rol);
    }
}
