using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiProducts.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApiDigitalGamesMx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {

        readonly IConfiguration _configuration;
        public MenuController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //https://localhost:44369/api/menu
        // GET: api/<UserController>
        [HttpGet]
        //[Route("products")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.Menu> GetMenus()
        {
            List<ApiProducts.Library.Models.Menu> listMenus = new List<ApiProducts.Library.Models.Menu>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IMenu menu = Factorizador.CrearConexionServicioMenu(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listMenus = menu.GetMenus();
            }
            return listMenus;
        }

        //https://localhost:44369/api/menu/manager
        // GET: api/<UserController>
        [HttpGet]
        [Route("{rol}")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.Menu> GetOrderDetail(string rol)//[FromBody] ApiProducts.Library.Models.PedidoCabMin value
        {
            List<ApiProducts.Library.Models.Menu> listMenus = new List<ApiProducts.Library.Models.Menu>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IMenu menu = Factorizador.CrearConexionServicioMenu(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listMenus = menu.GetMenusRol(rol);
            }
            return listMenus;
        }
    }
}
