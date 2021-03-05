using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiProducts.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ApiDigitalGamesMx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //https://localhost:44369/api/products
        // GET: api/<UserController>
        [HttpGet]
        //[Route("products")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.Producto> GetProducts()
        {
            List<ApiProducts.Library.Models.Producto> listProducts = new List<ApiProducts.Library.Models.Producto>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = product.GetProducts();
            }
            return listProducts;
        }

        [HttpGet]
        [Route("{id}")]
        //[Authorize]
        public IActionResult GetProduct(int id)//[FromBody] ApiProducts.Library.Models.CatProductos value
        {
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            //var ConnectionStringAzure = _configuration.GetValue<string>("ConnectionStringAzure");
            using (IProduct producto = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                ApiProducts.Library.Models.Producto objusr = producto.GetProduct(id);

                if (objusr.Id > 0)
                {
                    return Ok(new
                    {
                        Producto = objusr
                    });
                }

                return NotFound();

            }
        }

        [HttpPost]
        [Route("insertproduct")]
        public IActionResult InsertProduct([FromBody] ApiProducts.Library.Models.CatProductos value)
        {
            int id = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct Product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Product.InsertProduct(value.Sku, value.Titulo, value.Descripcion, value.IdPlataforma, value.IdGenero, value.idClasificacion, value.Imagen, value.Imagen2, value.Imagen3, value.UrlVideo, value.Costo, value.PrecioVenta, value.Edicion, value.FechaLanzamiento.ToString());

                if (id > 0)
                {
                    return Ok(new
                    {
                        Id = id,
                        Estatus = "success",
                        Code = 200,
                        Msg = "Producto insertado correctamnete!!"

                    });
                }
            }

            return NotFound();

        }

        //https://localhost:44369/api/products/plataforma
        // GET: api/<UserController>
        //[HttpPost]
        [HttpGet]
        [Route("plataforma/{id}")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.Producto> GetProductsPlataforma(int id)//[FromBody] ApiProducts.Library.Models.ProductoMin value
        {
            List<ApiProducts.Library.Models.Producto> listProducts = new List<ApiProducts.Library.Models.Producto>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = product.GetProductsPlataforma(id);
            }
            return listProducts;
        }

        [HttpGet]
        [Route("populares")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.Producto> GetProductsPopulares(int id)//[FromBody] ApiProducts.Library.Models.ProductoMin value
        {
            List<ApiProducts.Library.Models.Producto> listProducts = new List<ApiProducts.Library.Models.Producto>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = product.GetProductsPopulares();
            }
            return listProducts;
        }

        //https://localhost:44369/api/products/plataforma
        // GET: api/<UserController>
        //[HttpPost]
        [HttpGet]
        [Route("titulo/{criterio}")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.Producto> GetProductsCriterio(string criterio)//[FromBody] ApiProducts.Library.Models.ProductoMin value
        {
            List<ApiProducts.Library.Models.Producto> listProducts = new List<ApiProducts.Library.Models.Producto>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = product.GetProductsCriterio(criterio);
            }
            return listProducts;
        }

        //https://localhost:44369/api/products/plataforma
        // GET: api/<UserController>
        //[HttpPost]
        [HttpGet]
        [Route("wishlist/user/{id}")]
        //[Authorize]
        public IEnumerable<ApiProducts.Library.Models.Producto> GetProductsWishListUser(int id)//[FromBody] ApiProducts.Library.Models.ProductoMin value
        {
            List<ApiProducts.Library.Models.Producto> listProducts = new List<ApiProducts.Library.Models.Producto>();
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                listProducts = product.GetProductsWishListUser(id);
            }
            return listProducts;
        }

        [HttpPost]
        [Route("wishlist")]
        public IActionResult InsertProductWishList([FromBody] ApiProducts.Library.Models.WishListMin value)
        {
            int id = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct Product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Product.InsertProductWishList(value.IdCliente, value.IdProducto);

                if (id > 0)
                {
                    return Ok(new
                    {
                        Id = id,
                        Estatus = "success",
                        Code = 200,
                        Msg = "Producto insertado a WishList correctamnete!!"

                    });
                }
            }

            return NotFound();

        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateProduct([FromBody] ApiProducts.Library.Models.CatProductos value)
        {
            int id = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct Product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Product.UpdateProduct(value.Id, value.Sku, value.Titulo, value.Descripcion, value.IdPlataforma, value.IdGenero, value.idClasificacion, value.UrlVideo, value.Costo, value.PrecioVenta, value.Edicion, value.FechaLanzamiento.ToString());

                if (id > 0)
                {
                    return Ok(new
                    {
                        Id = value.Id,
                        Estatus = "success",
                        Code = 200,
                        Msg = "Producto actualizado correctamente!!"

                    });
                }
            }

            return NotFound();

        }

        [HttpPost]
        [Route("byids")]
        public IEnumerable<ApiProducts.Library.Models.ProductoRes> GetProductsIds(object value)
        {            
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            List<ApiProducts.Library.Models.ProductoRes> list = new List<ApiProducts.Library.Models.ProductoRes>();           
            ApiProducts.Library.Models.Producto objProducto = new ApiProducts.Library.Models.Producto();      
            //string json = @"[ {""idPedido"": 1, ""idProducto"": 1, ""cantidad"": 1 }]";
            //Deserialize the data
            var obj = JsonConvert.DeserializeObject<List<ApiProducts.Library.Models.ProductoMinRes>>(value.ToString());
            //Loop thrrouch values and save the details into database
            foreach (ApiProducts.Library.Models.ProductoMinRes p in obj)
            {
                using (IProduct producto = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
                {
                    objProducto = producto.GetProduct(p.Id);

                    list.Add(new ApiProducts.Library.Models.ProductoRes()
                    {
                        Id = objProducto.Id,
                        Titulo = objProducto.Titulo,                      
                        Plataforma = objProducto.Plataforma,                        
                        Imagen = objProducto.Imagen,
                        Imagen2 = objProducto.Imagen2,
                        Imagen3 = objProducto.Imagen3,                       
                        PrecioVenta = objProducto.PrecioVenta,
                        Edicion = objProducto.FechaLanzamiento,
                        IdCarrito = p.IdCarrito

                    });
                }
            }
            return list;
        }

        [HttpPost]
        [Route("wishlist/delete")]
        public IActionResult DeleteProductWishList([FromBody] ApiProducts.Library.Models.WishListMin value)
        {
            int id = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct Product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Product.DeleteProductWishlist(value.Id);

                if (id > 0)
                {
                    return Ok(new
                    {
                        Id = id,
                        Estatus = "success",
                        Code = 200,
                        Msg = "Producto eliminado de WishList correctamnete!!"

                    });
                }
            }

            return NotFound();

        }

        [HttpPost]
        [Route("delete")]
        public IActionResult DeleteProduct([FromBody] ApiProducts.Library.Models.CatProductos value)
        {
            int id = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct Product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = Product.DeleteProduct(value.Id);

                if (id > 0)
                {
                    return Ok(new
                    {
                        Id = id,
                        Estatus = "success",
                        Code = 200,
                        Msg = "Producto eliminado correctamnete!!"

                    });
                }
            }

            return NotFound();

        }

        [HttpPost]
        [Route("image/update")]
        public IActionResult UpdateImage([FromBody] ApiProducts.Library.Models.Imagen value)
        {
            int id = 0;
            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            using (IProduct product = Factorizador.CrearConexionServicio(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                id = product.UpdateImage(value.Id, value.Campo, value.Ruta);

                if (id > 0)
                {
                    return Ok(new
                    {
                        Id = value.Id,
                        Estatus = "success",
                        Code = 200,
                        Msg = "Producto actualizado correctamente!!"

                    });
                }
            }

            return NotFound();

        }

    }
}
