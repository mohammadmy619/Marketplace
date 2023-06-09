using MarcketAppliction.Utils;
using MarcketDataLayer.Entities.Slider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.EntitiesExtensions
{
    public static class SliderExtensions
    {
        public static string GetSliderImageAddress(this Slider slider)
        {
            return PathExtension.SliderOrigin + slider.ImageName;
        }
    }
}
