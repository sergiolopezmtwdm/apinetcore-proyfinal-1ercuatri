using System;
using System.Collections.Generic;
using System.Text;

namespace ApiProducts.Library.Interfaces
{
    public interface IOrder : IDisposable
    {
        List<Models.PedidoCab> GetOrders();
        Models.PedidoCab GetOrder(int id);
        int InsertOrder(int idCliente, decimal total, string formaPago, string nota);
        List<Models.PedidoDet> GetOrderDetail(int id);
        int InsertDetail(int idPedido, int idProducto, int idCodigo, int cantidad, decimal precioVenta);
    }
}
