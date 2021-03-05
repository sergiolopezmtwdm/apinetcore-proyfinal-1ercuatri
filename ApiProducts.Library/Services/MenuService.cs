using ApiProducts.Library.Helpers;
using ApiProducts.Library.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ApiProducts.Library.Interfaces;

namespace ApiProducts.Library.Services
{ 

    public class MenuService : IMenu, IDisposable
    {
        #region Constructor y Variables
        SqlConexion sql = null;
        ConnectionType type = ConnectionType.NONE;
        ProductService serviceProduct = new ProductService();
        Functions functions = new Functions();
        CodeService serviceCode = new CodeService();

        MenuService()
        {

        }

        public static MenuService CrearInstanciaSQL(SqlConexion sql)
        {
            MenuService
                log = new MenuService
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

        public List<Menu> GetMenus()
        {
            List<Menu> list = new List<Menu>();
            Menu menus = new Menu();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                sql.PrepararProcedimiento("dbo.[MENU.GetAllJSON]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Menus"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new Menu()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    Title = jsonOperaciones["title"].ToString(),
                                    Icon = jsonOperaciones["icon"].ToString(),
                                    Route = jsonOperaciones["route"].ToString(),
                                    Rol = jsonOperaciones["rol"].ToString()                                   
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

        public List<Menu> GetMenusRol(string rol)
        {
            List<Menu> list = new List<Menu>();
            Menu menus = new Menu();
            List<SqlParameter> _Parametros = new List<SqlParameter>();
            try
            {
                _Parametros.Add(new SqlParameter("@Rol", rol));
                sql.PrepararProcedimiento("dbo.[MENU.GetAllJSONRol]", _Parametros);
                DataTableReader dtr = sql.EjecutarTableReader(CommandType.StoredProcedure);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        var Json = dtr["Menus"].ToString();
                        if (Json != string.Empty)
                        {
                            JArray arr = JArray.Parse(Json);
                            foreach (JObject jsonOperaciones in arr.Children<JObject>())
                            {
                                //user = JsonConvert.DeserializeObject<User>(jsonOperaciones);
                                list.Add(new Menu()
                                {
                                    Id = Convert.ToInt32(jsonOperaciones["id"].ToString()),
                                    Title = jsonOperaciones["title"].ToString(),
                                    Icon = jsonOperaciones["icon"].ToString(),
                                    Route = jsonOperaciones["route"].ToString(),
                                    Rol = jsonOperaciones["rol"].ToString()
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
