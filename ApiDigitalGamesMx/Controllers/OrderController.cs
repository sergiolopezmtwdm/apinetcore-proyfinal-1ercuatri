using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDigitalGamesMx.Helpers;
using ApiProducts.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiDigitalGamesMx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //https://localhost:44369/api/code
        // GET: api/<UserController>
        [HttpGet]
        //[Route("order")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.PedidoCab> GetOrders()
        {
            List<ApiProducts.Library.Models.PedidoCab> listOrders = new List<ApiProducts.Library.Models.PedidoCab>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IOrder order = Factorizador.CrearConexionServicioOrder(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listOrders = order.GetOrders();
            }
            return listOrders;
        }

        [HttpGet]
        [Route("{id}")]
        //[Authorize]
        public IActionResult GetOrder(int id)//[FromBody] ApiProducts.Library.Models.PedidoCab value
        {
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            //var ConnectionStringAzure = _configuration.GetValue<string>("ConnectionStringAzure");
            using (ApiProducts.Library.Interfaces.IOrder order = ApiProducts.Library.Interfaces.Factorizador.CrearConexionServicioOrder(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                ApiProducts.Library.Models.PedidoCab objusr = order.GetOrder(id);

                if (objusr.Id > 0)
                {
                    return Ok(new
                    {
                        Pedido = objusr
                    });
                }

                return NotFound();

            }
        }

        [HttpPost]
        [Route("insertorder")]
        public IActionResult InsertOrder([FromBody] ApiProducts.Library.Models.opePedidoCab value)
        {
            int id = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IOrder Order = Factorizador.CrearConexionServicioOrder(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Order.InsertOrder(value.IdCliente, value.Total, value.FormPago, value.Nota);

                if (id > 0)
                {
                    return Ok(new
                    {
                        Id = id,
                        Estatus = "success",
                        Code = 200,
                        Msg = "Pedido insertado correctamente!!"

                    });
                }
            }

            return NotFound();

        }

        //https://localhost:44369/api/code
        // GET: api/<UserController>
        [HttpGet]
        [Route("detail/{id}")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.PedidoDet> GetOrderDetail(int id)//[FromBody] ApiProducts.Library.Models.PedidoCabMin value
        {
            List<ApiProducts.Library.Models.PedidoDet> listOrders = new List<ApiProducts.Library.Models.PedidoDet>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IOrder order = Factorizador.CrearConexionServicioOrder(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listOrders = order.GetOrderDetail(id);
            }
            return listOrders;
        }

        [HttpPost]
        [Route("cart")]
        public IActionResult InsertOrderDetail(ApiProducts.Library.Models.PedidoCabDet value)
        {
            int id = 0;
            int idPedido = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(value);

            int insertCodigo = 0;
            ApiProducts.Library.Models.Producto objProducto = new ApiProducts.Library.Models.Producto();
            ApiProducts.Library.Models.PedidoCab objPedido = new ApiProducts.Library.Models.PedidoCab();
            //string json = @"[ {""idPedido"": 1, ""idProducto"": 1, ""cantidad"": 1 }]";
            //Deserialize the data
            //var obj = JsonConvert.DeserializeObject<List<ApiProducts.Library.Models.PedidoCabDet>>(value.ToString());
            //Loop thrrouch values and save the details into database
          

            using (IOrder Order = Factorizador.CrearConexionServicioOrder(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                idPedido = Order.InsertOrder(value.ClienteId, value.Total,"TC", "");
            }

            foreach (int p in value.ListaProductos)
            {              

                using (ICode Code = Factorizador.CrearConexionServicioCode(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
                {
                    insertCodigo = Code.InsertCode(idPedido, p , Functions.RandomCodigo());

                }

                using (IProduct producto = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
                {
                    objProducto = producto.GetProduct(p);

                }

                using (IOrder Order = Factorizador.CrearConexionServicioOrder(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
                {
                    id = Order.InsertDetail(idPedido, p, insertCodigo, 1, objProducto.PrecioVenta);


                }

                using (IOrder order = Factorizador.CrearConexionServicioOrder(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
                {
                    objPedido = order.GetOrder(idPedido);

                }

            }

            if (id > 0)
            {
                return Ok(new
                {
                    Id = objPedido,
                    Estatus = "success",
                    Code = 200,
                    Msg = "Detalle del pedido insertado correctamente!!"

                });
            }

            return NotFound();

        }


    }
}
