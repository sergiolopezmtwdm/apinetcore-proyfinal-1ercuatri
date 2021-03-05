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
    public class CodeController : ControllerBase
    {

        readonly IConfiguration _configuration;
        public CodeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //https://localhost:44369/api/code
        // GET: api/<UserController>
        [HttpGet]
        //[Route("products")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.Code> GetCodes()
        {
            List<ApiProducts.Library.Models.Code> listCodes = new List<ApiProducts.Library.Models.Code>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (ICode code = Factorizador.CrearConexionServicioCode(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listCodes = code.GetCodes();
            }
            return listCodes;
        }

        [HttpPost]
        //[Route("")]
        //[Authorize]
        public IActionResult GetCode([FromBody] ApiProducts.Library.Models.Code value)
        {
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            //var ConnectionStringAzure = _configuration.GetValue<string>("ConnectionStringAzure");
            using (ApiProducts.Library.Interfaces.ICode codigo = ApiProducts.Library.Interfaces.Factorizador.CrearConexionServicioCode(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                ApiProducts.Library.Models.Code objusr = codigo.GetCode(value.Id);

                if (objusr.Id > 0)
                {
                    return Ok(new
                    {
                        Codigo = objusr
                    });
                }

                return NotFound();

            }
        }

        //[HttpPost]
        //[Route("insertcode")]
        //public IActionResult InsertCode([FromBody] ApiProducts.Library.Models.InvCodigos value)
        //{
        //    int id = 0;
        //    var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
        //    using (ICode Code = Factorizador.CrearConexionServicioCode(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
        //    {
        //        id = Code.InsertCode(value.IdPedido, value.IdProducto, value.Codigo);

        //        if (id > 0)
        //        {
        //            return Ok(new
        //            {
        //                Id = id,
        //                Estatus = "success",
        //                Code = 200,
        //                Msg = "Codigo insertado correctamente!!"

        //            });
        //        }
        //    }

        //    return NotFound();

        //}

        [HttpPost]
        [Route("pedido")]
        //[Authorize]
        public IActionResult GetCodePedido([FromBody] ApiProducts.Library.Models.Code value)
        {
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            //var ConnectionStringAzure = _configuration.GetValue<string>("ConnectionStringAzure");
            using (ApiProducts.Library.Interfaces.ICode codigo = ApiProducts.Library.Interfaces.Factorizador.CrearConexionServicioCode(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                ApiProducts.Library.Models.Code objusr = codigo.GetCodePedido(value.Id);

                if (objusr.Id > 0)
                {
                    return Ok(new
                    {
                        Codigo = objusr
                    });
                }

                return NotFound();

            }
        }

        //https://localhost:44369/api/code/plataforma
        // GET: api/<UserController>
        //[HttpPost]
        [HttpGet]
        [Route("cliente/{id}")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.Code> GetProductsPlataforma(int id)//[FromBody] ApiProducts.Library.Models.ProductoMin value
        {
            List<ApiProducts.Library.Models.Code> listCodes = new List<ApiProducts.Library.Models.Code>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (ICode codigo = Factorizador.CrearConexionServicioCode(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listCodes = codigo.GetCodesCliente(id);
            }
            return listCodes;
        }

    }
}
