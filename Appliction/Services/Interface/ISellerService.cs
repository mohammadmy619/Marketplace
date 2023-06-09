using MarcketDataLayer.DTOs.Common;
using MarcketDataLayer.DTOs.Post;
using MarcketDataLayer.DTOs.Seller;
using MarcketDataLayer.Entities.ProductOrder;
using MarcketDataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Interface
{
   public interface ISellerService: IAsyncDisposable
    {

        #region seller

        Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, long userId);
        Task<FilterSellerDTO> FilterSellers(FilterSellerDTO filter);
        Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId);
        Task<EditRequestSellerResult> EditRequestSeller(EditRequestSellerDTO request, long currentUserId);
        Task<bool> AcceptSellerRequest(long requestId);
        Task<bool> RejectSellerRequest(RejectItemDTO reject);
        Task<Seller> GetLastActiveSellerByUserId(long userId);
        Task<bool> HasUserAnyActiveSellerPanel(long userId);

        Task<FilterPostsdDto> FilterPosts(FilterPostsdDto filter, long SellerId);

        Task<DetailPostDTO> DetailPost(long PostId);
        Task<EditPostStatus> EditPostStatus(int Poststatus , long PostId);
        #endregion
    }
}
