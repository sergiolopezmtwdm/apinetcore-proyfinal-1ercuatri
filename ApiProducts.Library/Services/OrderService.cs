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
    public class OrderService : IOrder, IDisposable
    {
        #region Constructor y Variables
        SqlConexion sql = null;
        ConnectionType type = ConnectionType.NONE;
        ProductService serviceProduct = new ProductService();
        Functions functions = new Functions();
        CodeService serviceCode = new CodeService();

        OrderService()
        {

        }

        public static OrderService CrearInstanciaSQL(SqlConexion sql)
        {
            OrderService log = new OrderService
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

        public List<PedidoCab> GetOrders()
        {
            List<PedidoCab> list = new List<PedidoCab>();
            PedidoCab pedidoCab = new PedidoCab();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                sql.PrepararProcedimiento("dbo.[ORDER.GetAllJSONCab]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Pedidos"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new PedidoCab()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    Cliente = jsonOperaciones["cliente"].ToString(),
                                    Total = Convert.ToDecimal(jsonOperaciones["total"].ToString()),
                                    FormPago = jsonOperaciones["formaPago"].ToString(),
                                    Estatus = Convert.ToBoolean(jsonOperaciones["estatus"].ToString()),
                                    Cancelado = Convert.ToBoolean(jsonOperaciones["cancelado"].ToString()),
                                    FechaCancelacion = DateTime.Parse(jsonOperaciones["fechaCancelacion"].ToString()),
                                    MotivoCancelacion = jsonOperaciones["motivoCancelacion"].ToString(),
                                    Nota = jsonOperaciones["nota"].ToString(),
                                    FechaPedido = DateTime.Parse(jsonOperaciones["fechaPedido"].ToString())
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

        public PedidoCab GetOrder(int id)
        {
            PedidoCab pedidoCab = new PedidoCab();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                sql.PrepararProcedimiento("dbo.[ORDER.GetJSONCab]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Pedido"].ToString();
                        if (Json != string.Empty)
                            pedidoCab = JsonConvert.DeserializeObject<PedidoCab>(Json);
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

            return pedidoCab;
        }

        public int InsertOrder(int idCliente, decimal total, string formaPago, string nota)
        {
            int IdOrder = 0;
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@idCliente", idCliente));
                _Parametros.Add(new SqlParameter("@total", total));
                _Parametros.Add(new SqlParameter("@formaPago", formaPago));
                _Parametros.Add(new SqlParameter("@nota", nota));
                SqlParameter valreg = new SqlParameter();
                valreg.ParameterName = "@Id";
                valreg.DbType = DbType.Int32;
                valreg.Direction = ParameterDirection.Output;
                _Parametros.Add(valreg);

                sql.PrepararProcedimiento("dbo.[ORDER.InsertCab]", _Parametros);
                IdOrder = int.Parse(sql.EjecutarProcedimientoOutput().ToString());
                return IdOrder;
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



        public List<PedidoDet> GetOrderDetail(int id)
        {
            List<PedidoDet> list = new List<PedidoDet>();
            PedidoDet pedidoDet = new PedidoDet();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                sql.PrepararProcedimiento("dbo.[ORDER.GetJSONDet]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["DetallePedido"].ToString();


                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new PedidoDet()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    IdProducto = Convert.ToInt32(jsonOperaciones["idProducto"].ToString()),
                                    IdPedido = Convert.ToInt32(jsonOperaciones["idPedido"].ToString()),
                                    Titulo = jsonOperaciones["titulo"].ToString(),
                                    Codigo = jsonOperaciones["codigo"].ToString(),
                                    SubTotal = Convert.ToDecimal(jsonOperaciones["subtotal"].ToString()),
                                    Fecha = DateTime.Parse(jsonOperaciones["fecha"].ToString()),
                                    Cantidad = Convert.ToInt32(jsonOperaciones["cantidad"].ToString()),
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

        public int InsertDetail(int idPedido, int idProducto, int idCodigo, int cantidad, decimal precioVenta)
        {
            int IdOrder = 0;
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@IdPedido", idPedido));
                _Parametros.Add(new SqlParameter("@IdProducto", idProducto));
                _Parametros.Add(new SqlParameter("@IdCodigo", idCodigo));
                _Parametros.Add(new SqlParameter("@Cantidad", cantidad));
                _Parametros.Add(new SqlParameter("@Subtotal", precioVenta));
                SqlParameter valreg = new SqlParameter();
                valreg.ParameterName = "@Id";
                valreg.DbType = DbType.Int32;
                valreg.Direction = ParameterDirection.Output;
                _Parametros.Add(valreg);

                sql.PrepararProcedimiento("dbo.[ORDER.InsertDet]", _Parametros);
                IdOrder = int.Parse(sql.EjecutarProcedimientoOutput().ToString());



                return IdOrder;


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
