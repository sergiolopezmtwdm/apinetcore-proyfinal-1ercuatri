using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDigitalGamesMx.Helpers;
using ApiProducts.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApiDigitalGamesMx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //https://localhost:44369/api/user
        // GET: api/<UserController>
        [HttpGet]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.User> GetUsers()
        {

            List<ApiProducts.Library.Models.User> listUsers = new List<ApiProducts.Library.Models.User>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IUser User = Factorizador.CrearConexionServicioUser(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listUsers = User.GetUsers();
            }
            return listUsers;
        }

        [HttpPost]
        public IActionResult InsertUser([FromBody] ApiProducts.Library.Models.User value)
        {
            int id = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IUser User = Factorizador.CrearConexionServicioUser(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = User.InsertUser(value.Email,  Functions.GetSHA256(value.Contrasenia), value.NombreCompleto, value.Rol);

                if (id > 0)
                {
                    return Ok(new
                    {
                        Id = id,
                        Estatus = "success",
                        Code = 200,
                        Msg = "Usuario insertado correctamente!!"

                    });
                }

            }

            return NotFound();
        }

        [HttpPost]
        [Route("detail")]
        public IActionResult GetUserId([FromBody] ApiProducts.Library.Models.UserMin value)
        {
            
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IUser user = Factorizador.CrearConexionServicioUser(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                ApiProducts.Library.Models.User objusr = user.GetUser(value.Id);

                if (objusr.Id > 0)
                {
                    return Ok(new
                    {
                        User = objusr
                    });
                }

                
            }

            return NotFound();

        }
    }
}
