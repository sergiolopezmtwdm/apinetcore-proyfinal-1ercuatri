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
    public class CodeService : ICode, IDisposable
    {

        #region Constructor y Variables
        SqlConexion sql = null;
        ConnectionType type = ConnectionType.NONE;

        public CodeService()
        {

        }

        public static CodeService CrearInstanciaSQL(SqlConexion sql)
        {
            CodeService log = new CodeService
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

        public List<Code> GetCodes()
        {
            List<Code> list = new List<Code>();
            Code code = new Code();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                sql.PrepararProcedimiento("dbo.[CODE.GetAllJSON]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Codigos"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new Code()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    IdPedido = Convert.ToInt32(jsonOperaciones["idPedido"].ToString()),
                                    IdCliente = Convert.ToInt32(jsonOperaciones["idCliente"].ToString()),
                                    Titulo = jsonOperaciones["titulo"].ToString(),                                   
                                    Plataforma = jsonOperaciones["plataforma"].ToString(),
                                    Genero = jsonOperaciones["genero"].ToString(),
                                    Codigo = jsonOperaciones["codigo"].ToString(),
                                    Costo = Convert.ToDecimal(jsonOperaciones["costo"].ToString()),
                                    PrecioVenta  = Convert.ToDecimal(jsonOperaciones["precioVenta"].ToString()),
                                    Edicion = jsonOperaciones["edicion"].ToString(),
                                    Estatus = Convert.ToBoolean(jsonOperaciones["estatus"].ToString()),
                                    FechaActualizacion = DateTime.Parse(jsonOperaciones["fechaActualizacion"].ToString()),
                                    FechaCreacion = DateTime.Parse(jsonOperaciones["fechaCreacion"].ToString()),
                                    Imagen = jsonOperaciones["imagen"].ToString()
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

        public Code GetCode(int id)
        {
            Code code = new Code();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                sql.PrepararProcedimiento("dbo.[CODE.GetJSON]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Codigo"].ToString();
                        if (Json != string.Empty)
                            code = JsonConvert.DeserializeObject<Code>(Json);
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

            return code;
        }



        public int InsertCode(int idPedido, int idProducto, string codigo)
         {
            int IdCode = 0;
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@idPedido", idPedido));
                _Parametros.Add(new SqlParameter("@idProducto", idProducto));
                _Parametros.Add(new SqlParameter("@codigo", codigo));
                //_Parametros.Add(new SqlParameter("@estatus", estatus));  
                SqlParameter valreg = new SqlParameter();
                valreg.ParameterName = "@Id";
                valreg.DbType = DbType.Int32;
                valreg.Direction = ParameterDirection.Output;
                _Parametros.Add(valreg);

                sql.PrepararProcedimiento("dbo.[CODE.Insert]", _Parametros);
                IdCode = int.Parse(sql.EjecutarProcedimientoOutput().ToString());
                return IdCode;
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

        public Code GetCodePedido(int id)
        {
            Code code = new Code();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@idPedido", id));
                sql.PrepararProcedimiento("dbo.[CODE.GetJSONPedido]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Codigo"].ToString();
                        if (Json != string.Empty)
                            code = JsonConvert.DeserializeObject<Code>(Json);
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

            return code;
        }

        public List<Code> GetCodesCliente(int id)
        {
            List<Code> list = new List<Code>();
            Code code = new Code();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                sql.PrepararProcedimiento("dbo.[CODE.GetAllJSONCliente]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Codigos"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new Code()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    IdPedido = Convert.ToInt32(jsonOperaciones["idPedido"].ToString()),
                                    IdCliente = Convert.ToInt32(jsonOperaciones["idCliente"].ToString()),
                                    Titulo = jsonOperaciones["titulo"].ToString(),
                                    Plataforma = jsonOperaciones["plataforma"].ToString(),
                                    Genero = jsonOperaciones["genero"].ToString(),
                                    Codigo = jsonOperaciones["codigo"].ToString(),
                                    Costo = Convert.ToDecimal(jsonOperaciones["costo"].ToString()),
                                    PrecioVenta = Convert.ToDecimal(jsonOperaciones["precioVenta"].ToString()),
                                    Edicion = jsonOperaciones["edicion"].ToString(),
                                    Estatus = Convert.ToBoolean(jsonOperaciones["estatus"].ToString()),
                                    FechaActualizacion = DateTime.Parse(jsonOperaciones["fechaActualizacion"].ToString()),
                                    FechaCreacion = DateTime.Parse(jsonOperaciones["fechaCreacion"].ToString()),
                                    Imagen = jsonOperaciones["imagen"].ToString()
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

    }
}
