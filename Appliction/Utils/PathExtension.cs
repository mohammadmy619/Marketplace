using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Utils
{
    public static class PathExtension
    {
        #region domain address

        public static string DomainAddress = "https://homayonmusic.ir";

        #endregion

        #region Articles
        public static string ArticlesOrigin = "/img/Articles/";
        public static string ArticlesOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Articles/");
        public static string Articlesmp3Origin = "/mp3/Articles/";
        public static string Articlesmp3OriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/mp3/Articles/");
        public static string ArticlesVideoOrigin = "wwwroot/video/Articles/";
        public static string ArticlesVideoOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Video/Articles/");
        public static string ArticlesThumb = "/img/Articles/Thumb/";
        public static string ArticlesThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Articles/Thumb/");
        #endregion


        #region ArticlesGroups
        public static string ArticlesGroupsOrigin = "/img/ArticlesGroups/";
        public static string ArticlesGroupsOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/ArticlesGroups/");
        public static string ArticlesGroupsThumb = "/img/ArticlesGroups/Thumb/";
        public static string ArticlesGroupsThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/ArticlesGroups/Thumb/");
        #endregion

        #region default images

        public static string DefaultAvatar = "/img/defaults/avatar.jpg";

        #endregion 
        #region slider

        public static string SliderOrigin = "/img/slider/";
        public static string SliderOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/slider/");
        public static string SliderThumb = "/img/slider/Thumb/";
        public static string SliderThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/slider/Thumb/");



        #endregion
        #region banner

        public static string BannerOrigin = "/img/bg/";
        public static string BannerOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/bg/");
        public static string Logo = "/img/logo/";
        public static string LogoServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/logo/");


        #endregion
        #region user avatar

        public static string UserAvatarOrigin = "/Content/Images/UserAvatar/origin/";
        public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/origin/");

        public static string UserAvatarThumb = "/Content/Images/UserAvatar/Thumb/";
        public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/Thumb/");


        #endregion
        #region uploader

        public static string UploadImage = "/img/upload/";
        public static string UploadImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/upload/");

        #endregion
        #region products

        public static string ProductImage = "/content/images/product/origin/";

        public static string ProductImageImageServer =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/product/origin/");

        public static string ProductThumbnailImage = "/content/images/product/thumb/";

        public static string ProductThumbnailImageImageServer =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/product/thumb/");

        #endregion
        public static string ProductGalleryImage = "/content/images/product-gallery/origin/";

        public static string ProductGalleryImageServer =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/product-gallery/origin/");

        public static string ProductGalleryThumbnailImage = "/Content/Images/product-gallery/thumb/";

        public static string ProductGalleryThumbnailImageServer =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/product-gallery/thumb/");
    }
}
