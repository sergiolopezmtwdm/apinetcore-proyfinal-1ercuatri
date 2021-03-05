using System;
using System.Collections.Generic;
using System.Text;

namespace ApiProducts.Library.Models
{
    public class ProductoMin
    {
        public int Id { get; set; }
    }

    public class CatProductos : ProductoMin
    {
        public string Sku { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int IdPlataforma { get; set; }
        public int IdGenero { get; set; }
        public int idClasificacion { get; set; }
        public string Imagen { get; set; }
        public string Imagen2 { get; set; }
        public string Imagen3 { get; set; }
        public string UrlVideo { get; set; }
        public decimal Costo { get; set; }
        public decimal PrecioVenta { get; set; }
        public string Edicion { get; set; }
        public string FechaLanzamiento { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class Producto : ProductoMin
    {
        public string Sku { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int IdPlataforma { get; set; }
        public string Plataforma { get; set; }
        public int IdGenero { get; set; }
        public string Genero { get; set; }
        public int idClasificacion { get; set; }
        public string Clasificacion { get; set; }
        public string Imagen { get; set; }
        public string Imagen2 { get; set; }
        public string Imagen3 { get; set; }
        public string UrlVideo { get; set; }
        public decimal Costo { get; set; }
        public decimal PrecioVenta { get; set; }
        public string Edicion { get; set; }
        public string FechaLanzamiento { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class ProductoMinRes
    {
        public int Id { get; set; }
        public string IdCarrito { get; set; }
    }

    public class ProductoRes : ProductoMinRes
    {
        public string Titulo { get; set; }
        public string Plataforma { get; set; }
        public string Imagen { get; set; }
        public string Imagen2 { get; set; }
        public string Imagen3 { get; set; }
        public decimal PrecioVenta { get; set; }
        public string Edicion { get; set; }
    }



    public class CatPlataforma
    {
        public int Id { get; set; }
        public string Plataforma { get; set; }
        public string Descripcion { get; set; }
        public bool Estatus { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class CatGenero
    {
        public int Id { get; set; }
        public string Genero { get; set; }
        public string Descripcion { get; set; }
        public bool Estatus { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class CatClasificacion
    {
        public int Id { get; set; }
        public string Clasificacion { get; set; }
        public string Descripcion { get; set; }
        public bool Estatus { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class WishListMin
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
    }

    public class Imagen : ProductoMin
    {
        public string Campo { get; set; }
        public string Ruta { get; set; }
    }

}
