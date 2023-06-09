using MarcketDataLayer.DTOs.Discount;
using MarcketDataLayer.DTOs.ProductDiscount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Interface
{
    public interface IProductDiscountService : IAsyncDisposable
    {
        #region product discount

        Task<FilterProductDiscountDTO> FilterProductDiscount(FilterProductDiscountDTO filter);

        Task<CreateDiscountResult> CreateProductDiscount(CreateProductDiscountDto discount, long sellerId);

        #endregion
    }
}
