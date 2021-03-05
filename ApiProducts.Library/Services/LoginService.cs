using ApiProducts.Library.Helpers;
using ApiProducts.Library.Interfaces;
using ApiProducts.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ApiProducts.Library.Services
{
    class LoginService: ILogin, IDisposable
    {


        #region Constructor y Variables
        SqlConexion sql = null;
        ConnectionType type = ConnectionType.NONE;

        LoginService()
        {

        }

        public static LoginService CrearInstanciaSQL(SqlConexion sql)
        {
            LoginService log = new LoginService
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

        public User ObtenerLogin(string email, string password)
        {
            User user = new User();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@Email", email));
                _Parametros.Add(new SqlParameter("@Contrasenia", password));
                sql.PrepararProcedimiento("dbo.[USER.GetJSON]", _Parametros);
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


    }
}
