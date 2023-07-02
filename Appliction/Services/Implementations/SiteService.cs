using MarcketAppliction.Extensions;
using MarcketAppliction.Services.Interface;
using MarcketAppliction.Utils;
using MarcketDataLayer.DTOs.Slider;
using MarcketDataLayer.Entities.Site;
using MarcketDataLayer.Entities.Slider;
using MarketPlace.DataLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Implementations
{
    public class SiteService : ISiteService
    {
        #region constructor
        private readonly IGenericRepository<Slider> _sliderRepository;

        private readonly IGenericRepository<SiteSetting> _siteSettingRepository;
        private readonly IGenericRepository<SiteBanner> _bannerRepository;

        public SiteService(IGenericRepository<SiteSetting> siteSettingRepository, IGenericRepository<Slider> sliderRepository, IGenericRepository<SiteBanner> bannerRepository)
        {
            _siteSettingRepository = siteSettingRepository;
            _sliderRepository = sliderRepository;
            _bannerRepository = bannerRepository;
        }

        #endregion

        #region site settings

        public async Task<SiteSetting> GetDefaultSiteSetting()
        {
            
            return await _siteSettingRepository.GetQuery().AsNoTracking().AsQueryable()
                .SingleOrDefaultAsync(s => s.IsDefault && !s.IsDelete);
        }
        public async Task<bool> EditDefaultSiteSetting(SiteSetting site, IFormFile Logo)
        {

            var getSiteSettingByid = await _siteSettingRepository.GetEntityById(site.Id);
            if (getSiteSettingByid != null)
            {
                getSiteSettingByid.Phone = site.Phone;
                getSiteSettingByid.Mobile = site.Mobile;
                getSiteSettingByid.MapScript = site.MapScript;
                getSiteSettingByid.IsDelete = false;
                getSiteSettingByid.IsDefault = true;
                getSiteSettingByid.AboutUs = site.AboutUs;
                getSiteSettingByid.Address = site.Address;
                getSiteSettingByid.CopyRight = site.CopyRight;
                getSiteSettingByid.FooterText = site.FooterText;
                getSiteSettingByid.Email = site.Email;
                getSiteSettingByid.FaceBook = site.FaceBook;
                getSiteSettingByid.twitter = site.twitter;
                getSiteSettingByid.Telegram = site.Telegram;
                getSiteSettingByid.Instgram = site.Instgram;
                getSiteSettingByid.Sitetitle = site.Sitetitle;


                if (Logo != null && Logo.IsImage() &&Logo.FileName != getSiteSettingByid.Sitelogo)
                {
                    getSiteSettingByid.Sitelogo = Guid.NewGuid().ToString("N") + Path.GetExtension(Logo.FileName);

                    Logo.AddImageToServer(getSiteSettingByid.Sitelogo, PathExtension.LogoServer, 0, 0);

                    
                }

                _siteSettingRepository.EditEntity(getSiteSettingByid);
                await _siteSettingRepository.SaveChanges();
                return true;

            }

            return false;
        }
        #endregion

        #region slider

        public async Task<List<Slider>> GetAllActiveSliders()
        {
            return await _sliderRepository.GetQuery().AsQueryable()
                .Where(s =>!s.IsDelete&& s.IsActive==true).AsNoTracking().ToListAsync();
        }
        public async Task<CreateSliderResult> CreateSlider(CraeteSliderDto sliderDto, IFormFile Img)
        {
            if (Img == null && !Img.IsImage()) return CreateSliderResult.HasNoImage;

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(Img.FileName);

            var res = Img.AddImageToServer(imageName, PathExtension.SliderOriginServer, 150, 150, PathExtension.SliderThumbServer);
            if (sliderDto != null)
            {
                var createSlider = new Slider
                {
                    Description = sliderDto.Description,
                    SecondHeader = sliderDto.SecondHeader,
                    IsActive = sliderDto.IsActive,
                    MainHeader = sliderDto.MainHeader,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    Link = sliderDto.Link,
                    ImageName = imageName,
                    LastUpdateDate = DateTime.Now
                };
                await _sliderRepository.AddEntity(createSlider);
                await _sliderRepository.SaveChanges();
                return CreateSliderResult.Sucsses;
            }
            return CreateSliderResult.error;


        }
        public async Task<EditSliderDto> GetSliderforEdit(long id)
        {
            var getSlider = await _sliderRepository.GetQuery().Where(c => c.Id == id).SingleOrDefaultAsync();

            if (getSlider == null) return null;
            var slider = new EditSliderDto
            {
                Editsliderid = getSlider.Id,
                SecondHeader = getSlider.SecondHeader,
                Description = getSlider.Description,
                IsActive = getSlider.IsActive,
                Link = getSlider.Link,
                MainHeader = getSlider.MainHeader,
               ImageName=getSlider.ImageName,
                
            };

            return slider;

        }

        public async Task<EditSliderResult> EditSliderResults(EditSliderDto edit, IFormFile img)
        {
            var getSlider = await _sliderRepository.GetQuery().Where(c => c.Id == edit.Editsliderid).SingleOrDefaultAsync();
            if (getSlider == null) return EditSliderResult.error;

           if(img != null && img.IsImage())
            {

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(img.FileName);

                img.AddImageToServer(imageName, PathExtension.SliderOriginServer, 150, 150, PathExtension.SliderThumbServer, getSlider.ImageName);
                getSlider.ImageName = imageName;
            }

            getSlider.IsActive = edit.IsActive;
            getSlider.IsDelete = false;
            getSlider.Link = edit.Link;
            getSlider.SecondHeader = edit.SecondHeader;
            getSlider.MainHeader = edit.MainHeader;
            getSlider.Description = edit.Description;


             _sliderRepository.EditEntity(getSlider);

            await _sliderRepository.SaveChanges();

            return EditSliderResult.Sucsses;

        }

        public async Task<bool> DeleteSlider(long id)
        {
            var get = await _sliderRepository.GetEntityById(id);
            if (get == null) return false;
            _sliderRepository.DeleteEntity(get);
            UploadImageExtension.DeleteImage(get.ImageName, PathExtension.SliderOriginServer, PathExtension.SliderThumbServer);
            await _sliderRepository.SaveChanges();
            return true;
        }
        #endregion
        #region site banners

        public async Task<List<SiteBanner>> GetSiteBannersByPlacement(List<BannerPlacement> placements)
        {
            return await _bannerRepository.GetQuery().AsQueryable()
                .Where(s => placements.Contains(s.BannerPlacement ) && s.IsDelete == false).ToListAsync();
        }

        public async Task<bool> CreateBanner(long Bpid, string url, IFormFile img)
        {
            if (img != null && !img.IsImage())
            {
                return false;
            }

            if (img != null && img.IsImage())
            {
                var newBanner = new SiteBanner
                {
                    BannerPlacement = (BannerPlacement)Bpid,
                    Url = url,
                    IsDelete = false,
                    CreateDate = DateTime.Now

                };
                newBanner.ImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(img.FileName);

                img.AddImageToServer(newBanner.ImageName, PathExtension.BannerOriginServer, 0, 0);

                await _bannerRepository.AddEntity(newBanner);

                await _bannerRepository.SaveChanges();
            }
            return true;


        }

        public async Task<bool> DeleteBanner(long id)
        {
            var getbaner = await _bannerRepository.GetQuery().Where(x=>x.Id==id).SingleOrDefaultAsync();
            if (getbaner == null) return false;


            _bannerRepository.DeleteEntity(getbaner);

            UploadImageExtension.DeleteImage(getbaner.ImageName, PathExtension.BannerOriginServer,null);

            await _bannerRepository.SaveChanges();

            return true;

        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            if (_siteSettingRepository != null) await _siteSettingRepository.DisposeAsync();
            if (_sliderRepository != null) await _sliderRepository.DisposeAsync();
            if (_bannerRepository != null) await _bannerRepository.DisposeAsync();
        }






        #endregion
    }
}
