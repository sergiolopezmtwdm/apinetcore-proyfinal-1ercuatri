using System;
using System.Collections.Generic;
using System.Text;

namespace ApiProducts.Library.Interfaces
{
    public interface IProduct : IDisposable
    {
        List<Models.Producto> GetProducts();
        Models.Producto GetProduct(int id);
        int InsertProduct(string sku, string titulo, string descripcion, int idPLataforma, int idGenero, int idClasificacion, string imagen, string imagen2, string imagen3, string urlVideo, decimal costo, decimal precioVenta, string edicion, string fechaLanzamiento);

        List<Models.Producto> GetProductsPlataforma(int id);

        List<Models.Producto> GetProductsPopulares();

        List<Models.Producto> GetProductsCriterio(string criterio);
        List<Models.Producto> GetProductsWishListUser(int id);

        int InsertProductWishList(int idCliente, int idProducto);

        int UpdateProduct(int id, string sku, string titulo, string descripcion, int idPLataforma, int idGenero, int idClasificacion, string urlVideo, decimal costo, decimal precioVenta, string edicion, string fechaLanzamiento);

        int DeleteProductWishlist(int id);

        int UpdateImage(int id, string campo, string ruta);

        int DeleteProduct(int id);
       

    }
}
