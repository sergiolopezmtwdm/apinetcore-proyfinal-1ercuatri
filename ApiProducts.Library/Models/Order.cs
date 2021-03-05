using System;
using System.Collections.Generic;
using System.Text;

namespace ApiProducts.Library.Models
{
    public class PedidoCabMin
    {
        public int Id { get; set; }
    }

    public class opePedidoCab : PedidoCabMin
    {
        public int IdCliente { get; set; }
        public decimal Total { get; set; }
        public string FormPago { get; set; }
        public bool Estatus { get; set; }
        public bool Cancelado { get; set; }
        public DateTime? FechaCancelacion { get; set; }
        public string MotivoCancelacion { get; set; }
        public string Nota { get; set; }
        public DateTime? FechaPedido { get; set; }
    }

    public class PedidoCab : PedidoCabMin
    {
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public string FormPago { get; set; }
        public bool Estatus { get; set; }
        public bool Cancelado { get; set; }
        public DateTime? FechaCancelacion { get; set; }
        public string MotivoCancelacion { get; set; }
        public string Nota { get; set; }
        public DateTime? FechaPedido { get; set; }
    }

    public class PedidoDetMin
    {
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }

    public class opePedidoDet : PedidoDetMin
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public int IdCodigo { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime? Fecha { get; set; }
    }

    public class PedidoDet : PedidoDetMin
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Codigo { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime? Fecha { get; set; }
    }

    public class PedidoCabDet
    {
        public int ClienteId { get; set; }
        public List<int> ListaProductos { get; set; }
        public decimal Total { get; set; }
    }
}
