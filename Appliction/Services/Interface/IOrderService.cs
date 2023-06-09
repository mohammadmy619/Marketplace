using MarcketDataLayer.DTOs.Orders;
using MarcketDataLayer.Entities.ProductOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Interface
{
   public  interface  IOrderService 
    {
        #region order

        Task<long> AddOrderForUser(long userId);
        Task<Order> GetUserLatestOpenOrder(long userId);

        Task<int> GetTotalOrderPriceForPayment(long userId);
        
        Task PayOrderProductPriceToSeller(long userId, long refId);

        #endregion

        #region order detail
        Task ChangeOrderDetailCount(long detailId, long userId, int count);
        Task AddProductToOpenOrder(long userId, AddProductToOrderDTO order);
        Task<UserOpenOrderDTO> GetUserOpenOrderDetail(long userId);
        Task<bool> RemoveOrderDetail(long detailId, long userId);

        #endregion

        #region Finalpurchases


        Task<FilterFinalpurchasesDTO> GetAllFinalpurchasesbyuserId(FilterFinalpurchasesDTO filter, long UserId);

        Task<Order> GetUserOrderIsPaid(long id ,long userId);

        Task<GetUserOrderIsPaid> DetailFinalpurchases(long id ,long userId);
        #endregion

    }
}
