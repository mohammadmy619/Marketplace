using MarcketDataLayer.Entities.Products;
using MarketPlace.DataLayer.Entities.Commen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketDataLayer.Entities.ProductOrder
{
   public class OrderDetail :BaseEntity
    {

        //public OrderDetail(long orderId, long productId, long? productColorId, int count, int productPrice, int productColorPrice, Order order, Product product, ProductColor productColor)
        //{
        //    OrderId = orderId;
        //    ProductId = productId;
        //    ProductColorId = productColorId;
        //    Count = count;
        //    ProductPrice = productPrice;
        //    ProductColorPrice = productColorPrice;
        //    Order = order;
        //    Product = product;
        //    ProductColor = productColor;
        //}


        //public OrderDetail()
        //{
        //    OrderId = OrderId;
        //    ProductId = ProductId;
        //    ProductColorId = ProductColorId;
        //    Count = Count;
        //    ProductPrice = ProductPrice;
        //    ProductColorPrice = ProductColorPrice;


        //    Order = new Order();

        //    Product = new Product();

        //    ProductColor = new ProductColor();

        //}
        #region properties

        public long OrderId { get; set; }

        public long ProductId { get; set; }

        public long? ProductColorId { get; set; }

        public int Count { get; set; }

        public int ProductPrice { get; set; }

        public int ProductColorPrice { get; set; }

        #endregion

        #region relations

        public Order Order { get; set; }

        public Product Product { get; set; }

        public ProductColor ProductColor { get; set; }

        #endregion
    }
}
