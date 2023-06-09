using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Orders;
using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.Entities.ProductOrder;
using MarcketDataLayer.Entities.Products;
using MarcketDataLayer.Entities.Store;
using MarcketDataLayer.Entities.Wallet;
using MarcketDataLayer.Repository;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Implementations
{
    public class OrderService : IOrderService
    {

        #region constructor

        private IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;
        private readonly ISellerWalletService _sellerWalletService;
        private readonly IGenericRepository<ProductDiscount> _productDiscountRepository;
        private readonly IGenericRepository<ProductDiscountUse> _productDiscountUseRepository;
        private IGenericRepository<Product> _productRepository;
        private IGenericRepository<OrderIsPaidDetials> _OrderIsPaidDetialsRepository;
        private DapperUtility _DapperUtility;


        private IGenericRepository<Posts> _PostsRepository;
        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<OrderDetail> orderDetailRepository, ISellerWalletService sellerWalletService, IGenericRepository<ProductDiscount> productDiscountRepository, IGenericRepository<ProductDiscountUse> productDiscountUseRepository, IGenericRepository<Product> ProductRepository, IGenericRepository<OrderIsPaidDetials> OrderIsPaidDetialsRepository, IGenericRepository<Posts> PostsRepository, DapperUtility DapperUtility)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _sellerWalletService = sellerWalletService;
            _productDiscountRepository = productDiscountRepository;
            _productDiscountUseRepository = productDiscountUseRepository;
            _productRepository = ProductRepository;
            _OrderIsPaidDetialsRepository = OrderIsPaidDetialsRepository;
            _PostsRepository = PostsRepository;
            _DapperUtility = DapperUtility;
        }

        #endregion

        #region order

        public async Task<long> AddOrderForUser(long userId)
        {
            var order = new Order { UserId = userId };

            await _orderRepository.AddEntity(order);

            await _orderRepository.SaveChanges();

            return order.Id;
        }

        public async Task<Order> GetUserLatestOpenOrder(long userId)
        {
            if (!await _orderRepository.GetQuery().AnyAsync(s => s.UserId == userId && !s.IsPaid))
                await AddOrderForUser(userId);


            var userOpenOrder = await _orderRepository.GetQuery()
                .Include(s => s.OrderDetails)
                .ThenInclude(s => s.ProductColor)
                .Include(s => s.OrderDetails.Where(s => s.IsDelete == false))
                .ThenInclude(s => s.Product)
                .ThenInclude(s => s.ProductDiscounts.Where(s => s.ExpireDate > DateTime.Now && s.DiscountNumber > 0).OrderBy(c => c.CreateDate))
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.UserId == userId && !s.IsPaid);

            return userOpenOrder;
        }

        public async Task<int> GetTotalOrderPriceForPayment(long userId)

        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);
            int totalPrice = 0;
            int discount = 0;


            foreach (var detail in userOpenOrder.OrderDetails)
            {
                var oneProductPrice = 0;
                if (detail.ProductColor == null)
                {
                    oneProductPrice = detail.Product.Price;
                }
                else
                {
                    oneProductPrice =
                    detail.Product.Price + detail.ProductColor.Price;

                }

                //var oneProductPrice = detail.ProductColor != null
                //    ? detail.Product.Price + detail.ProductColor.Price
                //    : detail.Product.Price;


                var productDiscount = await _productDiscountRepository.GetQuery().Where(s => s.ProductId == detail.ProductId && s.DiscountNumber > 0 && s.ExpireDate > DateTime.Now && s.IsDelete == false)
                    .Include(s => s.ProductDiscountUses)
                    .OrderByDescending(s => s.CreateDate)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (productDiscount != null)
                {
                    discount = (int)Math.Ceiling(oneProductPrice * productDiscount.Percentage / (decimal)100);
                    //discount = oneProductPrice * (productDiscount.Percentage / 100);

                }

                //totalPrice += detail.Count * (oneProductPrice - discount);
                totalPrice = detail.Count * (oneProductPrice - discount);


            }

            return totalPrice;
        }

        public async Task PayOrderProductPriceToSeller(long userId, long refId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);


            foreach (var detail in openOrder.OrderDetails)
            {
                var productPrice = detail.Product.Price;
                var productColorPrice = detail.ProductColor?.Price ?? 0;
                var discount = 0;
                var totalPrice = detail.Count * (productPrice + productColorPrice);
                var productDiscount = await _productDiscountRepository.GetQuery().Where(s =>
                        s.ProductId == detail.ProductId && s.DiscountNumber > 0 && s.ExpireDate > DateTime.Now)
                    .Include(s => s.ProductDiscountUses)
                    .OrderByDescending(s => s.CreateDate)
                    .FirstOrDefaultAsync();

                if (productDiscount != null)
                {
                    discount = (int)Math.Ceiling(totalPrice * productDiscount.Percentage / (decimal)100);

                    var newDiscountUse = new ProductDiscountUse
                    {
                        UserId = userId,
                        ProductDiscountId = productDiscount.Id,
                    };
                    productDiscount.DiscountNumber -= 1;

                    _productDiscountRepository.EditEntity(productDiscount);
                    await _productDiscountUseRepository.AddEntity(newDiscountUse);
                    await _productDiscountRepository.SaveChanges();

                }

                var totalPriceWithDiscount = totalPrice - discount;

                await _sellerWalletService.AddWallet(new SellerWallet
                {
                    SellerId = detail.Product.SellerId,
                    Price = (int)Math.Ceiling(totalPriceWithDiscount * (100 - detail.Product.SiteProfit) / (double)100),
                    TransactionType = TransactionType.Deposit,
                    Description = $"پرداخت مبلغ {totalPriceWithDiscount} تومان جهت فروش {detail.Product.Title} به تعداد {detail.Count} عدد با سهم تهیین شده ی {100 - detail.Product.SiteProfit} درصد"
                });

                detail.ProductPrice = totalPriceWithDiscount;

                detail.ProductColorPrice = productColorPrice;

                _orderDetailRepository.EditEntity(detail);

                var Product = await _productRepository.GetQuery().Where(x => x.Id == detail.ProductId).FirstOrDefaultAsync();

                Product.Salescount += detail.Count;

                _productRepository.EditEntity(Product);


                var orderIsPaidDetials = new OrderIsPaidDetials
                {
                    CreateDate = DateTime.Now,
                    ProductPrice = Product.Price,
                    ProductImageName = Product.ImageName,
                    Count = detail.Count,
                    ColorName = detail.ProductColor?.ColorName,
                    IsDelete = false,
                    ProductTitle = Product.Title,
                    OrderId = detail.OrderId,
                    UserId = userId,
                    SellerId = Product.SellerId,
                    DiscountPercentage = productDiscount?.Percentage ?? 0,
                    ProductId = detail.Product.Id,
                    ProductColorId = detail?.ProductColorId,
                    ProductColorPrice = detail.ProductColorPrice,
                    LastUpdateDate = DateTime.Now,
                    PoststatusForUser = PoststatusForUser.UnderProgress


                };



                await _OrderIsPaidDetialsRepository.AddEntity(orderIsPaidDetials);

                var newPost = new Posts
                {
                    Count = detail.Count,
                    SellerId = Product.SellerId,
                    CreateDate = DateTime.Now,
                    ProductImageName = Product.ImageName,
                    ProductTitle = Product.Title,
                    ProductId = Product.Id,
                    IsDelete = false,
                    ProductPrice = Product.Price,
                    LastUpdateDate = DateTime.Now,
                    DiscountPercentage = productDiscount?.Percentage ?? 0,
                    UserId = userId,
                    ColorName = detail.ProductColor?.ColorName,
                    ProductColorPrice = detail.ProductColorPrice,
                    ProductColorId = detail?.ProductColorId,
                    OrderId = detail.OrderId,
                    Poststatus = Poststatus.UnderProgress
                };



                await _PostsRepository.AddEntity(newPost);

                await _productRepository.SaveChanges();

            }

            openOrder.IsPaid = true;
            openOrder.TracingCode = refId.ToString();
            openOrder.PaymentDate = DateTime.Now;



            _orderRepository.EditEntity(openOrder);




            await _orderRepository.SaveChanges();

        }

        public async Task ChangeOrderDetailCount(long detailId, long userId, int count)
        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);
            var detail = userOpenOrder.OrderDetails.SingleOrDefault(s => s.Id == detailId);
            if (detail != null)
            {
                if (count > 0)
                {
                    detail.Count = count;
                }
                else
                {
                    _orderDetailRepository.DeleteEntity(detail);
                }
                await _orderDetailRepository.SaveChanges();
            }
        }

        #endregion

        #region order detail

        public async Task AddProductToOpenOrder(long userId, AddProductToOrderDTO order)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);

            var similarOrder = openOrder.OrderDetails.SingleOrDefault(s =>
                s.ProductId == order.ProductId && s.ProductColorId == order.ProductColorId && !s.IsDelete);

            if (similarOrder == null)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = openOrder.Id,
                    ProductId = order.ProductId,
                    ProductColorId = order.ProductColorId,
                    Count = order.Count
                };

                await _orderDetailRepository.AddEntity(orderDetail);
                await _orderDetailRepository.SaveChanges();
            }
            else
            {
                similarOrder.Count += order.Count;
                await _orderDetailRepository.SaveChanges();
            }
        }



        public async Task<UserOpenOrderDTO> GetUserOpenOrderDetail(long userId)
        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);







            return new UserOpenOrderDTO
            {
                UserId = userId,
                Description = userOpenOrder.Description,
                Details = userOpenOrder.OrderDetails
                    .Where(s => !s.IsDelete)
                    .Select(s => new UserOpenOrderDetailItemDTO
                    {
                        DetailId = s.Id,
                        Count = s.Count,
                        ColorName = s.ProductColor?.ColorName,
                        ProductColorId = s.ProductColorId,
                        ProductColorPrice = s.ProductColor?.Price ?? 0,
                        ProductId = s.ProductId,
                        ProductPrice = s.Product.Price,
                        ProductTitle = s.Product.Title,
                        ProductImageName = s.Product.ImageName,
                        DiscountPercentage = s.Product.ProductDiscounts.Where(s => s.ExpireDate > DateTime.Now && s.DiscountNumber > 0)
                        .OrderByDescending(a => a.CreateDate)
                        .FirstOrDefault(a => a.ExpireDate > DateTime.Now)?.Percentage
                    }).ToList()
            };
        }

        public async Task<bool> RemoveOrderDetail(long detailId, long userId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);
            var orderDetail = openOrder.OrderDetails.SingleOrDefault(s => s.Id == detailId);
            if (orderDetail == null) return false;

            _orderDetailRepository.DeleteEntity(orderDetail);
            await _orderDetailRepository.SaveChanges();

            return true;
        }

        #endregion

        #region Finalpurchases
        public async Task<FilterFinalpurchasesDTO> GetAllFinalpurchasesbyuserId(FilterFinalpurchasesDTO filter, long UserId)
        {
            var query = _orderRepository.GetQuery().Where(c => c.UserId == UserId && c.IsPaid == true && c.IsDelete == false)
              .Include(s => s.OrderDetails)
              .OrderByDescending(x => x.Id)
              .AsNoTracking()
              .AsQueryable();






            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetFinalpurchases(allEntities);
        }


        public async Task<Order> GetUserOrderIsPaid(long id, long userId)
        {
            //var userOrderIsPaid = await _orderRepository.GetQuery().Where(s => s.Id == id && s.UserId == userId && s.IsPaid).FirstOrDefaultAsync();
            //await AddOrderForUser(userId);

            var userOrderIsPaid = await _orderRepository.GetQuery().Where(x => x.Id == id)
                .Include(s => s.OrderDetails)
                .ThenInclude(s => s.ProductColor)
                .Include(s => s.OrderDetails)
                .ThenInclude(s => s.Product)
                .ThenInclude(s => s.ProductDiscounts)
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.UserId == userId && s.IsPaid);

            return userOrderIsPaid;
        }

        public async Task<GetUserOrderIsPaid> DetailFinalpurchases(long id, long userId)
        {

            var userOpenOrder = await GetUserOrderIsPaid(id, userId);

            var getoederispaid = await _OrderIsPaidDetialsRepository.GetQuery().Where(x => x.OrderId == id).ToListAsync();

            var res = new GetUserOrderIsPaid
            {

                UserId = userId,
                Description = userOpenOrder.Description,
                Details = getoederispaid
                        .Where(s => s.OrderId == id)
                        .Select(s => new DetailFinalpurchases
                        {
                            DetailId = s.Id,
                            Count = s.Count,
                            ColorName = s.ColorName,
                            ProductColorId = s.ProductColorId,
                            ProductColorPrice = s.ProductColorPrice,
                            ProductId = s.ProductId,
                            ProductPrice = s.ProductPrice,
                            ProductTitle = s.ProductTitle,
                            ProductImageName = s.ProductImageName,
                            DiscountPercentage = s.DiscountPercentage,
                            PoststatusForUser = s.PoststatusForUser


                        }).ToList()
            };

            return res;
            //return new GetUserOrderIsPaid
            //{
            //    UserId = userId,
            //    Description = userOpenOrder.Description,
            //    Details = userOpenOrder.OrderDetails
            //        .Where(s => s.OrderId==id)
            //        .Select(s => new DetailFinalpurchases
            //        {
            //            DetailId = s.Id,
            //            Count = s.Count,
            //            ColorName = s.ProductColor?.ColorName,
            //            ProductColorId = s.ProductColorId,
            //            ProductColorPrice = s.ProductColor?.Price ?? 0,
            //            ProductId = s.ProductId,
            //            ProductPrice = s.Product.Price,
            //            ProductTitle = s.Product.Title,
            //            ProductImageName = s.Product.ImageName,
            //            //DiscountPercentage = s.Product.ProductDiscounts
            //            //.OrderByDescending(a => a.CreateDate)
            //            //.FirstOrDefault(a => a.ExpireDate > DateTime.Now)?.Percentage
            //        }).ToList()
            //};
        }

        #endregion

    }

}

