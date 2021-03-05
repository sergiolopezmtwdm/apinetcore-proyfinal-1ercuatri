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
    public class UserService: IUser, IDisposable
    {

        #region Constructor y Variables
        SqlConexion sql = null;      
        ConnectionType type = ConnectionType.NONE;

        UserService()
        {

        }

        public static UserService CrearInstanciaSQL(SqlConexion sql)
        {
            UserService log = new UserService
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

        public List<User> GetUsers()
        {
            List<User> list = new List<User>();
            User user = new User();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                sql.PrepararProcedimiento("dbo.[USER.GetAllJSON]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Usuarios"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new User()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    Email = jsonOperaciones["email"].ToString(),
                                    Contrasenia = jsonOperaciones["contrasenia"].ToString(),
                                    NombreCompleto = jsonOperaciones["nombreCompleto"].ToString(),
                                    Rol = jsonOperaciones["rol"].ToString(),
                                    FechaCreacion = DateTime.Parse(jsonOperaciones["fechaCreacion"].ToString()),
                                    RefreshToken = jsonOperaciones["refreshToken"].ToString()
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

        public int InsertUser(string email, string password, string nombreCompleto, string rol)
        {
            int IdUser = 0;
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@Email", email));
                _Parametros.Add(new SqlParameter("@Contrasenia", password));
                _Parametros.Add(new SqlParameter("@NombreCompleto", nombreCompleto));
                _Parametros.Add(new SqlParameter("@Rol", rol));
                SqlParameter valreg = new SqlParameter();
                valreg.ParameterName = "@Id";
                valreg.DbType = DbType.Int32;
                valreg.Direction = ParameterDirection.Output;
                _Parametros.Add(valreg);

                sql.PrepararProcedimiento("dbo.[USER.Insert]", _Parametros);
                IdUser = int.Parse(sql.EjecutarProcedimientoOutput().ToString());
                return IdUser;
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

        public User GetUser(int id)
        {
            User user = new User();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@id", id));
                sql.PrepararProcedimiento("dbo.[USER.GetJSONId]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Usuario"].ToString();
                        if (Json != string.Empty)
                            user = JsonConvert.DeserializeObject<User>(Json);
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

            return user;
        }

       
        public int UpdateRefreshTokenExpiryTime(User user)
        {
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@Id", user.Id));
                _Parametros.Add(new SqlParameter("@RefreshToken", user.RefreshToken));
                _Parametros.Add(new SqlParameter("@RefreshTokenExpiryTime", user.RefreshTokenExpiryTime));
                sql.PrepararProcedimiento("dbo.[USER.UpdateRefreshTokenExpiryTime]", _Parametros);
                return int.Parse(sql.EjecutarProcedimiento().ToString());
                //return 0;
                
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

        public int UpdateRefreshToken(User user)
        {
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@Id", user.Id));
                _Parametros.Add(new SqlParameter("@RefreshToken", (object)user.RefreshToken ?? DBNull.Value));
                sql.PrepararProcedimiento("dbo.[USER.UpdateRefreshToken]", _Parametros);
                
                //return 0;
                return int.Parse(sql.EjecutarProcedimiento().ToString());
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
