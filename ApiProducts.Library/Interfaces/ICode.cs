using ApiProducts.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ApiProducts.Library.Interfaces
{
    public interface ICode : IDisposable
    {
        List<Models.Code> GetCodes();
        Models.Code GetCode(int id);

        int InsertCode(int idPedido, int idProducto, string codigo);

        Models.Code GetCodePedido(int id);

        List<Models.Code> GetCodesCliente(int id);
    }
}
