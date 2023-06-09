using MarcketAppliction.Utils;
using MarcketDataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.EntitiesExtensions
{
    public static class BannerExtension
    {
        public static string GetBannerMainImageAddress(this SiteBanner banner)
        {
            return PathExtension.BannerOrigin + banner.ImageName;
        }
    }
}
