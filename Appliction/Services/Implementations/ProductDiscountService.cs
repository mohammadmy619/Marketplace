using MarcketAppliction.Services.Interface;
using MarcketAppliction.Utils;
using MarcketDataLayer.DTOs.Discount;
using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.DTOs.ProductDiscount;
using MarcketDataLayer.Entities.Products;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Implementations
{
    public  class ProductDiscountService: IProductDiscountService
    {

        #region constructor

        private readonly IGenericRepository<ProductDiscount> _productDiscountRepository;
        private readonly IGenericRepository<ProductDiscountUse> _productDiscountUseRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public ProductDiscountService(IGenericRepository<ProductDiscount> productDiscountRepository, IGenericRepository<ProductDiscountUse> productDiscountUseRepository, IGenericRepository<Product> productRepository)
        {
            _productDiscountRepository = productDiscountRepository;
            _productDiscountUseRepository = productDiscountUseRepository;
            _productRepository = productRepository;
        }

        #endregion

        #region product discount

        public async Task<FilterProductDiscountDTO> FilterProductDiscount(FilterProductDiscountDTO filter)
        {
            var query = _productDiscountRepository.GetQuery()
                .Include(s => s.Product).AsNoTracking()
                .AsQueryable();

            #region filter

            if (filter.ProductId != null && filter.ProductId != 0)
                query = query.Where(s => s.ProductId == filter.ProductId.Value);

            if (filter.SellerId != null && filter.SellerId != 0)
                query = query.Where(s => s.Product.SellerId == filter.SellerId.Value);

            #endregion

            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetDiscounts(allEntities);
        }

        public async Task<CreateDiscountResult> CreateProductDiscount(CreateProductDiscountDto discount, long sellerId)
        {
            var product = await _productRepository.GetEntityById(discount.ProductId);
            if (product == null) return CreateDiscountResult.ProductNotFound;
            if (product.SellerId != sellerId) return CreateDiscountResult.ProductIsNotForSeller;

            //discount.ExpireDate.ToMiladi();

            var newDiscount = new ProductDiscount
            {
                ProductId = product.Id,
                DiscountNumber = discount.DiscountNumber,
                Percentage = discount.Percentage,
                ExpireDate = discount.ExpireDate.ToMiladiDateTime().ToMiladi(),
        };

            await _productDiscountRepository.AddEntity(newDiscount);
            await _productDiscountRepository.SaveChanges();

            return CreateDiscountResult.Success;
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _productDiscountRepository.DisposeAsync();
            await _productDiscountUseRepository.DisposeAsync();
        }

        #endregion
    }
}
