using ApiProducts.Library.Helpers;
using ApiProducts.Library.Interfaces;
using ApiProducts.Library.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ApiProducts.Library.Services
{
    public class ProductService : IProduct, IDisposable
    {
        #region Constructor y Variables
        SqlConexion sql = null;
        ConnectionType type = ConnectionType.NONE;
        Functions functions = new Functions();

        public ProductService()
        {

        }

        public static ProductService CrearInstanciaSQL(SqlConexion sql)
        {
            ProductService log = new ProductService
            {
                sql = sql,
                type = ConnectionType.MSSQL
            };

            return log;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (sql != null)
                    {
                        sql.Desconectar();
                        sql.Dispose();
                    }// TODO: elimine el estado administrado (objetos administrados).
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~HidraService()
        // {
        // // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        // Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }
        #endregion

        public List<Producto> GetProducts()
        {
            List<Producto> list = new List<Producto>();
            Producto product = new Producto();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                sql.PrepararProcedimiento("dbo.[PRODUCT.GetAllJSON]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Productos"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new Producto()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    Titulo = jsonOperaciones["titulo"].ToString(),
                                    Sku = jsonOperaciones["sku"].ToString(),
                                    Descripcion = jsonOperaciones["descripcion"].ToString(),
                                    Plataforma = jsonOperaciones["plataforma"].ToString(),
                                    Genero = jsonOperaciones["genero"].ToString(),
                                    Clasificacion = jsonOperaciones["clasificacion"].ToString(),
                                    Imagen = jsonOperaciones["imagen"].ToString(),
                                    Imagen2 = jsonOperaciones["imagen2"].ToString(),
                                    Imagen3 = jsonOperaciones["imagen3"].ToString(),
                                    UrlVideo = jsonOperaciones["urlVideo"].ToString(),
                                    Costo = Convert.ToDecimal(jsonOperaciones["costo"].ToString()),
                                    PrecioVenta = Convert.ToDecimal(jsonOperaciones["precioVenta"].ToString()),
                                    Edicion = jsonOperaciones["edicion"].ToString(),
                                    FechaLanzamiento = jsonOperaciones["fechaLanzamiento"].ToString(),
                                    FechaActualizacion = DateTime.Parse(jsonOperaciones["fechaActualizacion"].ToString()),
                                    FechaCreacion = DateTime.Parse(jsonOperaciones["fechaCreacion"].ToString())
                                });

                            }

                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return list;
        }

        public Producto GetProduct(int id)
        {
            Producto producto = new Producto();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));               
                sql.PrepararProcedimiento("dbo.[PRODUCT.GetJSON]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Producto"].ToString();
                        if (Json != string.Empty)
                            producto = JsonConvert.DeserializeObject<Producto>(Json);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return producto;
        }



        public int InsertProduct(string sku, string titulo, string descripcion, int idPLataforma, int idGenero, int idClasificacion, string imagen, string imagen2, string imagen3, string urlVideo,decimal costo, decimal precioVenta, string edicion, string fechaLanzamiento)
        {
            int IdProduct = 0;
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            //sku = functions.RandomSku();
            try
            {
                _Parametros.Add(new SqlParameter("@sku", sku));
                _Parametros.Add(new SqlParameter("@titulo", titulo));
                _Parametros.Add(new SqlParameter("@descripcion", descripcion));
                _Parametros.Add(new SqlParameter("@idPlataforma", idPLataforma));
                _Parametros.Add(new SqlParameter("@idGenero", idGenero));
                _Parametros.Add(new SqlParameter("@idClasificacion", idClasificacion));
                _Parametros.Add(new SqlParameter("@imagen", imagen));
                _Parametros.Add(new SqlParameter("@imagen2", imagen2));
                _Parametros.Add(new SqlParameter("@imagen3", imagen3));
                _Parametros.Add(new SqlParameter("@urlVideo", urlVideo));
                _Parametros.Add(new SqlParameter("@costo", costo));
                _Parametros.Add(new SqlParameter("@precioVenta", precioVenta));
                _Parametros.Add(new SqlParameter("@edicion", edicion));
                _Parametros.Add(new SqlParameter("@fechaLanzamiento", fechaLanzamiento));
                SqlParameter valreg = new SqlParameter();
                valreg.ParameterName = "@Id";
                valreg.DbType = DbType.Int32;
                valreg.Direction = ParameterDirection.Output;
                _Parametros.Add(valreg);

                sql.PrepararProcedimiento("dbo.[PRODUCT.Insert]", _Parametros);
                IdProduct = int.Parse(sql.EjecutarProcedimientoOutput().ToString());
                return IdProduct;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<Producto> GetProductsPlataforma(int id)
        {
            List<Producto> list = new List<Producto>();
            Producto product = new Producto();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                sql.PrepararProcedimiento("dbo.[PRODUCT.GetAllJSONPlataforma]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Productos"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new Producto()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    Titulo = jsonOperaciones["titulo"].ToString(),
                                    Sku = jsonOperaciones["sku"].ToString(),
                                    Descripcion = jsonOperaciones["descripcion"].ToString(),
                                    Plataforma = jsonOperaciones["plataforma"].ToString(),
                                    Genero = jsonOperaciones["genero"].ToString(),
                                    Clasificacion = jsonOperaciones["clasificacion"].ToString(),
                                    Imagen = jsonOperaciones["imagen"].ToString(),
                                    Imagen2 = jsonOperaciones["imagen2"].ToString(),
                                    Imagen3 = jsonOperaciones["imagen3"].ToString(),
                                    UrlVideo = jsonOperaciones["urlVideo"].ToString(),
                                    Costo = Convert.ToDecimal(jsonOperaciones["costo"].ToString()),
                                    PrecioVenta = Convert.ToDecimal(jsonOperaciones["precioVenta"].ToString()),
                                    Edicion = jsonOperaciones["edicion"].ToString(),
                                    FechaLanzamiento = jsonOperaciones["fechaLanzamiento"].ToString(),
                                    FechaActualizacion = DateTime.Parse(jsonOperaciones["fechaActualizacion"].ToString()),
                                    FechaCreacion = DateTime.Parse(jsonOperaciones["fechaCreacion"].ToString())
                                });

                            }

                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return list;
        }

        public List<Producto> GetProductsPopulares()
        {
            List<Producto> list = new List<Producto>();
            Producto product = new Producto();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {               
                sql.PrepararProcedimiento("dbo.[PRODUCT.GetAllJSONPopulares]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Productos"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new Producto()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    Titulo = jsonOperaciones["titulo"].ToString(),
                                    Sku = jsonOperaciones["sku"].ToString(),
                                    Descripcion = jsonOperaciones["descripcion"].ToString(),
                                    Plataforma = jsonOperaciones["plataforma"].ToString(),
                                    Genero = jsonOperaciones["genero"].ToString(),
                                    Clasificacion = jsonOperaciones["clasificacion"].ToString(),
                                    Imagen = jsonOperaciones["imagen"].ToString(),
                                    Imagen2 = jsonOperaciones["imagen2"].ToString(),
                                    Imagen3 = jsonOperaciones["imagen3"].ToString(),
                                    UrlVideo = jsonOperaciones["urlVideo"].ToString(),
                                    Costo = Convert.ToDecimal(jsonOperaciones["costo"].ToString()),
                                    PrecioVenta = Convert.ToDecimal(jsonOperaciones["precioVenta"].ToString()),
                                    Edicion = jsonOperaciones["edicion"].ToString(),
                                    FechaLanzamiento = jsonOperaciones["fechaLanzamiento"].ToString(),
                                    FechaActualizacion = DateTime.Parse(jsonOperaciones["fechaActualizacion"].ToString()),
                                    FechaCreacion = DateTime.Parse(jsonOperaciones["fechaCreacion"].ToString())
                                });

                            }

                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return list;
        }

        public List<Producto> GetProductsCriterio(string criterio)
        {
            List<Producto> list = new List<Producto>();
            Producto product = new Producto();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@criterio", criterio));
                sql.PrepararProcedimiento("dbo.[PRODUCT.GetAllJSONCriterio]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Productos"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new Producto()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    Titulo = jsonOperaciones["titulo"].ToString(),
                                    Sku = jsonOperaciones["sku"].ToString(),
                                    Descripcion = jsonOperaciones["descripcion"].ToString(),
                                    Plataforma = jsonOperaciones["plataforma"].ToString(),
                                    Genero = jsonOperaciones["genero"].ToString(),
                                    Clasificacion = jsonOperaciones["clasificacion"].ToString(),
                                    Imagen = jsonOperaciones["imagen"].ToString(),
                                    Imagen2 = jsonOperaciones["imagen2"].ToString(),
                                    Imagen3 = jsonOperaciones["imagen3"].ToString(),
                                    UrlVideo = jsonOperaciones["urlVideo"].ToString(),
                                    Costo = Convert.ToDecimal(jsonOperaciones["costo"].ToString()),
                                    PrecioVenta = Convert.ToDecimal(jsonOperaciones["precioVenta"].ToString()),
                                    Edicion = jsonOperaciones["edicion"].ToString(),
                                    FechaLanzamiento = jsonOperaciones["fechaLanzamiento"].ToString(),
                                    FechaActualizacion = DateTime.Parse(jsonOperaciones["fechaActualizacion"].ToString()),
                                    FechaCreacion = DateTime.Parse(jsonOperaciones["fechaCreacion"].ToString())
                                });

                            }

                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return list;
        }

        public List<Producto> GetProductsWishListUser(int id)
        {
            List<Producto> list = new List<Producto>();
            Producto product = new Producto();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                sql.PrepararProcedimiento("dbo.[PRODUCT.GetAllJSONWhishListUser]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Productos"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new Producto()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    Titulo = jsonOperaciones["titulo"].ToString(),
                                    Sku = jsonOperaciones["sku"].ToString(),
                                    Descripcion = jsonOperaciones["descripcion"].ToString(),
                                    Plataforma = jsonOperaciones["plataforma"].ToString(),
                                    Genero = jsonOperaciones["genero"].ToString(),
                                    Clasificacion = jsonOperaciones["clasificacion"].ToString(),
                                    Imagen = jsonOperaciones["imagen"].ToString(),
                                    Imagen2 = jsonOperaciones["imagen2"].ToString(),
                                    Imagen3 = jsonOperaciones["imagen3"].ToString(),
                                    UrlVideo = jsonOperaciones["urlVideo"].ToString(),
                                    Costo = Convert.ToDecimal(jsonOperaciones["costo"].ToString()),
                                    PrecioVenta = Convert.ToDecimal(jsonOperaciones["precioVenta"].ToString()),
                                    Edicion = jsonOperaciones["edicion"].ToString(),
                                    FechaLanzamiento = jsonOperaciones["fechaLanzamiento"].ToString(),
                                    FechaActualizacion = DateTime.Parse(jsonOperaciones["fechaActualizacion"].ToString()),
                                    FechaCreacion = DateTime.Parse(jsonOperaciones["fechaCreacion"].ToString())
                                });

                            }

                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return list;
        }

        public int InsertProductWishList(int idCliente, int idProducto)
        {
            int IdProduct = 0;
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {                
                _Parametros.Add(new SqlParameter("@idCliente", idCliente));
                _Parametros.Add(new SqlParameter("@idProducto", idProducto));
                SqlParameter valreg = new SqlParameter();
                valreg.ParameterName = "@Id";
                valreg.DbType = DbType.Int32;
                valreg.Direction = ParameterDirection.Output;
                _Parametros.Add(valreg);

                sql.PrepararProcedimiento("dbo.[PRODUCT.InsertWishList]", _Parametros);
                IdProduct = int.Parse(sql.EjecutarProcedimientoOutput().ToString());
                return IdProduct;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int UpdateProduct(int id, string sku, string titulo, string descripcion, int idPLataforma, int idGenero, int idClasificacion, string urlVideo, decimal costo, decimal precioVenta, string edicion, string fechaLanzamiento)
        {
            int IdProduct = 0;
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            //sku = functions.RandomSku();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                _Parametros.Add(new SqlParameter("@sku", sku));
                _Parametros.Add(new SqlParameter("@titulo", titulo));
                _Parametros.Add(new SqlParameter("@descripcion", descripcion));
                _Parametros.Add(new SqlParameter("@idPlataforma", idPLataforma));
                _Parametros.Add(new SqlParameter("@idGenero", idGenero));
                _Parametros.Add(new SqlParameter("@idClasificacion", idClasificacion));
                _Parametros.Add(new SqlParameter("@urlVideo", urlVideo));
                _Parametros.Add(new SqlParameter("@costo", costo));
                _Parametros.Add(new SqlParameter("@precioVenta", precioVenta));
                _Parametros.Add(new SqlParameter("@edicion", edicion));
                _Parametros.Add(new SqlParameter("@fechaLanzamiento", fechaLanzamiento));
                //SqlParameter valreg = new SqlParameter();
                //valreg.ParameterName = "@Id";
                //valreg.DbType = DbType.Int32;
                //valreg.Direction = ParameterDirection.Output;
                //_Parametros.Add(valreg);

                sql.PrepararProcedimiento("dbo.[PRODUCT.Update]", _Parametros);
                IdProduct = int.Parse(sql.EjecutarProcedimiento().ToString());
                return IdProduct;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int DeleteProductWishlist(int id)
        {
            int IdProduct = 0;
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            //sku = functions.RandomSku();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                //SqlParameter valreg = new SqlParameter();
                //valreg.ParameterName = "@Id";
                //valreg.DbType = DbType.Int32;
                //valreg.Direction = ParameterDirection.Output;
                //_Parametros.Add(valreg);

                sql.PrepararProcedimiento("dbo.[PRODUCT.DeletetWishList]", _Parametros);
                IdProduct = int.Parse(sql.EjecutarProcedimiento().ToString());
                return IdProduct;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int DeleteProduct(int id)
        {
            int IdProduct = 0;
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            //sku = functions.RandomSku();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                //SqlParameter valreg = new SqlParameter();
                //valreg.ParameterName = "@Id";
                //valreg.DbType = DbType.Int32;
                //valreg.Direction = ParameterDirection.Output;
                //_Parametros.Add(valreg);

                sql.PrepararProcedimiento("dbo.[PRODUCT.Delete]", _Parametros);
                IdProduct = int.Parse(sql.EjecutarProcedimiento().ToString());
                return IdProduct;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int UpdateImage(int id, string campo, string ruta)
        {
            int IdProduct = 0;
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                _Parametros.Add(new SqlParameter("@Campo", campo));
                _Parametros.Add(new SqlParameter("@Ruta", ruta));
                //SqlParameter valreg = new SqlParameter();
                //valreg.ParameterName = "@Id";
                //valreg.DbType = DbType.Int32;
                //valreg.Direction = ParameterDirection.Output;
                //_Parametros.Add(valreg);

                sql.PrepararProcedimiento("dbo.[PRODUCT.UpdateImage]", _Parametros);
                IdProduct = int.Parse(sql.EjecutarProcedimiento().ToString());
                return IdProduct;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


    }
}
