using MarcketAppliction.Services.Interface;
using MarcketDataLayer.DTOs.Common;
using MarcketDataLayer.DTOs.Paging;
using MarcketDataLayer.DTOs.Post;
using MarcketDataLayer.DTOs.Seller;
using MarcketDataLayer.Entities.ProductOrder;
using MarcketDataLayer.Entities.Store;
using MarketPlace.DataLayer.Entities.Accuont.User;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Implementations
{
  public  class SellerService: ISellerService
    {
        #region constcutor

        private readonly IGenericRepository<Seller> _sellerRepository;
        private readonly IGenericRepository<User> _userRepository;
        private  IGenericRepository<Posts> _PostsRepository;
        private IGenericRepository<OrderIsPaidDetials> _OrderIsPaidDetialsRepository;



        public SellerService(IGenericRepository<Seller> sellerRepository, IGenericRepository<User> userRepository, IGenericRepository<Posts> PostsRepository, IGenericRepository<OrderIsPaidDetials> OrderIsPaidDetialsRepository)
        {
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
            _PostsRepository = PostsRepository;
            _OrderIsPaidDetialsRepository = OrderIsPaidDetialsRepository;
        }

        #endregion


        #region seller

        public async Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, long userId)
        {
            var user = await _userRepository.GetEntityById(userId);

            if (user.IsBlocked) return RequestSellerResult.HasNotPermission;

            var hasUnderProgressRequest = await _sellerRepository.GetQuery().AsQueryable().AnyAsync(s =>
                s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.UnderProgress);


            if (hasUnderProgressRequest) return RequestSellerResult.HasUnderProgressRequest;

            var newSeller = new Seller
            {
                UserId = userId,
                StoreName = seller.StoreName,
                Address = seller.Address,
                Phone = seller.Phone,
                StoreAcceptanceState = StoreAcceptanceState.UnderProgress
            };

            await _sellerRepository.AddEntity(newSeller);
            await _sellerRepository.SaveChanges();

            return RequestSellerResult.Success;
        }



        public async Task<FilterSellerDTO> FilterSellers(FilterSellerDTO filter)
        {
            var query = _sellerRepository.GetQuery()
                .Include(s => s.User)
                .AsQueryable();

            #region state

            switch (filter.State)
            {
                case FilterSellerState.All:
                    query = query.Where(s => !s.IsDelete);
                    break;
                case FilterSellerState.Accepted:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.Accepted && !s.IsDelete);
                    break;

                case FilterSellerState.UnderProgress:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.UnderProgress && !s.IsDelete);
                    break;
                case FilterSellerState.Rejected:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.Rejected && !s.IsDelete);
                    break;
            }

            #endregion

            #region filter

            if (filter.UserId != null && filter.UserId != 0)
                query = query.Where(s => s.UserId == filter.UserId);

            if (!string.IsNullOrEmpty(filter.StoreName))
                query = query.Where(s => EF.Functions.Like(s.StoreName, $"%{filter.StoreName}%"));

            if (!string.IsNullOrEmpty(filter.Phone))
                query = query.Where(s => EF.Functions.Like(s.Phone, $"%{filter.Phone}%"));

            if (!string.IsNullOrEmpty(filter.Mobile))
                query = query.Where(s => EF.Functions.Like(s.Mobile, $"%{filter.Mobile}%"));

            if (!string.IsNullOrEmpty(filter.Address))
                query = query.Where(s => EF.Functions.Like(s.Address, $"%{filter.Address}%"));

            #endregion

            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetSellers(allEntities);
        }
        public async Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId)
        {
            var seller = await _sellerRepository.GetEntityById(id);
            if (seller == null || seller.UserId != currentUserId) return null;

            return new EditRequestSellerDTO
            {
                Id = seller.Id,
                Phone = seller.Phone,
                Address = seller.Address,
                StoreName = seller.StoreName
            };
        }

        public async Task<EditRequestSellerResult> EditRequestSeller(EditRequestSellerDTO request, long currentUserId)
        {
            var seller = await _sellerRepository.GetEntityById(request.Id);
            if (seller == null || seller.UserId != currentUserId) return EditRequestSellerResult.NotFound;

            seller.Phone = request.Phone;
            seller.Address = request.Address;
            seller.StoreName = request.StoreName;
            seller.StoreAcceptanceState = StoreAcceptanceState.UnderProgress;
            _sellerRepository.EditEntity(seller);
            await _sellerRepository.SaveChanges();

            return EditRequestSellerResult.Success;
        }


        public async Task<bool> AcceptSellerRequest(long requestId)
        {
            var sellerRequest = await _sellerRepository.GetEntityById(requestId);
            if (sellerRequest != null)
            {
                sellerRequest.StoreAcceptanceState = StoreAcceptanceState.Accepted;
                sellerRequest.StoreAcceptanceDescription = "اطلاعات پنل فروشندگی شما تایید شده است";
                _sellerRepository.EditEntity(sellerRequest);
                await _sellerRepository.SaveChanges();

                return true;
            }

            return false;
        }

        public async Task<bool> RejectSellerRequest(RejectItemDTO reject)
        {
            var seller = await _sellerRepository.GetEntityById(reject.Id);
            if (seller != null)
            {
                seller.StoreAcceptanceState = StoreAcceptanceState.Rejected;
                seller.StoreAcceptanceDescription = reject.RejectMessage;
                _sellerRepository.EditEntity(seller);
                await _sellerRepository.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<Seller> GetLastActiveSellerByUserId(long userId)
        {
            return await _sellerRepository.GetQuery()
                .OrderBy(s => s.CreateDate)
                .FirstOrDefaultAsync(s =>
                    s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.Accepted);
        }

        public async Task<bool> HasUserAnyActiveSellerPanel(long userId)
        {
            return await _sellerRepository.GetQuery()
                .OrderByDescending(s => s.CreateDate)
                .AnyAsync(s =>
                    s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.Accepted);
        }

        public async Task<FilterPostsdDto> FilterPosts(FilterPostsdDto filter,long SellerId)
        {
            var query = _PostsRepository.GetQuery().Where(x=>x.SellerId==SellerId)
             .Include(s => s.User)
             .OrderBy(x=>x.CreateDate)
             .AsQueryable();


            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

           

            return filter.SetPaging(pager).SetPost(allEntities);
        }

        public async Task<DetailPostDTO> DetailPost(long PostId)
        {
            var result = await _PostsRepository.GetQuery().Where(s => s.Id == PostId).FirstOrDefaultAsync();
            var user =await _userRepository.GetQuery().Where(X => X.Id == result.UserId).FirstOrDefaultAsync();

            var newPostDetail = new DetailPostDTO
            {
                ColorName=result.ColorName,
                Count=result.Count,
                DiscountPercentage=result.DiscountPercentage,
                Poststatus=result.Poststatus,
                ProductImageName= result.ProductImageName,
                ProductTitle= result.ProductTitle,
                ProductPrice= result.ProductPrice,
                ProductColorPrice= result.ProductColorPrice,
                ProductColorId= result.ProductColorId,
                Email=user.Email,
                FirstName= user.FirstName,
                LastName= user.LastName,
                Mobile= user.Mobile,
                UserId=result.UserId,Id=result.Id
            };

            return newPostDetail;
        }

        public async Task<EditPostStatus> EditPostStatus(int Poststatus, long PostId )
        {
            var post =await _PostsRepository.GetQuery().Where(s=>s.Id== PostId).FirstOrDefaultAsync();
            if (post == null) return MarcketDataLayer.DTOs.Post.EditPostStatus.Error;
            post.Poststatus = (Poststatus)Poststatus;
            _PostsRepository.EditEntity(post);
            var OrderIsPaidDetials = await _OrderIsPaidDetialsRepository.GetQuery().Where(s => s.OrderId == post.OrderId && s.ProductId == post.ProductId && s.SellerId == s.SellerId && s.UserId == post.UserId).FirstOrDefaultAsync();
            OrderIsPaidDetials.PoststatusForUser = (PoststatusForUser)Poststatus;
            _OrderIsPaidDetialsRepository.EditEntity(OrderIsPaidDetials);
           await _PostsRepository.SaveChanges();

            return MarcketDataLayer.DTOs.Post.EditPostStatus.Success;

        }
        #endregion



        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _sellerRepository.DisposeAsync();
        }

       



        #endregion

    }
}
