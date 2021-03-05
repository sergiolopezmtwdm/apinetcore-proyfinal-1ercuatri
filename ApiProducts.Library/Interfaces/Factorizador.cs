
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiProducts.Library.Interfaces
{

    using ApiProducts.Library.Helpers;
    using ApiProducts.Library.Services;
    public static class Factorizador
    {
        public static IProduct CrearConexionServicio(Models.ConnectionType type, string connectionString)
        {
            IProduct nuevoMotor = null; ;
            switch (type)
            {
                case Models.ConnectionType.NONE:
                    break;
                case Models.ConnectionType.MSSQL:
                    SqlConexion sql = SqlConexion.Conectar(connectionString);
                    nuevoMotor = ProductService.CrearInstanciaSQL(sql);
                    break;
                case Models.ConnectionType.MYSQL:

                    break;
                default:
                    break;
            }

            return nuevoMotor;
        }

        public static ICode CrearConexionServicioCode(Models.ConnectionType type, string connectionString)
        {
            ICode nuevoMotor = null; ;
            switch (type)
            {
                case Models.ConnectionType.NONE:
                    break;
                case Models.ConnectionType.MSSQL:
                    SqlConexion sql = SqlConexion.Conectar(connectionString);
                    nuevoMotor = CodeService.CrearInstanciaSQL(sql);
                    break;
                case Models.ConnectionType.MYSQL:

                    break;
                default:
                    break;
            }

            return nuevoMotor;
        }

        public static IOrder CrearConexionServicioOrder(Models.ConnectionType type, string connectionString)
        {
            IOrder nuevoMotor = null; ;
            switch (type)
            {
                case Models.ConnectionType.NONE:
                    break;
                case Models.ConnectionType.MSSQL:
                    SqlConexion sql = SqlConexion.Conectar(connectionString);
                    nuevoMotor = OrderService.CrearInstanciaSQL(sql);
                    break;
                case Models.ConnectionType.MYSQL:

                    break;
                default:
                    break;
            }

            return nuevoMotor;
        }

        public static IUser CrearConexionServicioUser(Models.ConnectionType type, string connectionString)
        {
            IUser nuevoMotor = null; ;
            switch (type)
            {
                case Models.ConnectionType.NONE:
                    break;
                case Models.ConnectionType.MSSQL:
                    SqlConexion sql = SqlConexion.Conectar(connectionString);
                    nuevoMotor = UserService.CrearInstanciaSQL(sql);
                    break;
                case Models.ConnectionType.MYSQL:

                    break;
                default:
                    break;
            }

            return nuevoMotor;
        }

        public static ILogin CrearConexionServicioLogin(Models.ConnectionType type, string connectionString)
        {
            ILogin nuevoMotor = null; ;
            switch (type)
            {
                case Models.ConnectionType.NONE:
                    break;
                case Models.ConnectionType.MSSQL:
                    SqlConexion sql = SqlConexion.Conectar(connectionString);
                    nuevoMotor = LoginService.CrearInstanciaSQL(sql);
                    break;
                case Models.ConnectionType.MYSQL:

                    break;
                default:
                    break;
            }

            return nuevoMotor;
        }

        public static IMenu CrearConexionServicioMenu(Models.ConnectionType type, string connectionString)
        {
            IMenu nuevoMotor = null; ;
            switch (type)
            {
                case Models.ConnectionType.NONE:
                    break;
                case Models.ConnectionType.MSSQL:
                    SqlConexion sql = SqlConexion.Conectar(connectionString);
                    nuevoMotor = MenuService.CrearInstanciaSQL(sql);
                    break;
                case Models.ConnectionType.MYSQL:

                    break;
                default:
                    break;
            }

            return nuevoMotor;
        }
    }
}
