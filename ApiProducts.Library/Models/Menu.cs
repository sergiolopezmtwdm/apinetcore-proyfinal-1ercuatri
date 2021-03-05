using System;
using System.Collections.Generic;
using System.Text;

namespace ApiProducts.Library.Models
{
    public class MenuMin
    {
        public int Id { get; set; }
    }

    public class Menu : MenuMin
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
        public string Rol { get; set; }
    }

    
}
