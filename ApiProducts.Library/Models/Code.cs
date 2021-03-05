using System;
using System.Collections.Generic;
using System.Text;

namespace ApiProducts.Library.Models
{
    public class CodigoMin
    {
        public int Id { get; set; }
    }

    public class InvCodigos : CodigoMin
    {
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }        
        public string Codigo { get; set; }
        public bool Estatus { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class Code : CodigoMin
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public string Titulo { get; set; }
        public string Plataforma { get; set; }
        public string Genero { get; set; }
        public string Codigo { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal Costo { get; set; }
        public string Edicion { get; set; }
        public bool Estatus { get; set; }
        public string Imagen { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
