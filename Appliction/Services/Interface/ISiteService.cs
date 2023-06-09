using MarcketDataLayer.DTOs.Slider;
using MarcketDataLayer.Entities.Site;
using MarcketDataLayer.Entities.Slider;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Interface
{
   public interface ISiteService:IAsyncDisposable
    {

        #region site settings

        Task<SiteSetting> GetDefaultSiteSetting();
        Task<bool> EditDefaultSiteSetting(SiteSetting site, IFormFile Logo);

        #endregion
        #region slider

        Task<List<Slider>> GetAllActiveSliders();

        Task<CreateSliderResult> CreateSlider(CraeteSliderDto sliderDto, IFormFile Img);

        Task<EditSliderDto> GetSliderforEdit(long id);

        Task<EditSliderResult> EditSliderResults(EditSliderDto edit, IFormFile img); 
        Task<bool> DeleteSlider(long id);

        #endregion
        #region site banners

        Task<List<SiteBanner>> GetSiteBannersByPlacement(List<BannerPlacement> placements);

        #endregion

        Task<bool> CreateBanner(long Bpid, string url, IFormFile img);
        Task<bool> DeleteBanner(long id);

    }
}
