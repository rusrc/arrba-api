using System;
using System.Configuration;
using System.Linq;
using Arrba.Enumerations;
using Arrba.Exceptions;

namespace Arrba.Constants
{
    public static partial class SETTINGS
    {
        /// <summary>
        /// Tuple.item1 = banner width int
        /// Tuple.item2 = banner height int
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="BusinessCriticalLogicException">BusinessCriticalLogicException</exception>
        public static Tuple<int, int> GetBannerSize(BannerType type)
        {
            switch (type)
            {
                case BannerType.TopBanner:
                    return Tuple.Create(1400, 80); //"1400x80";
                case BannerType.HotBanner:
                    return Tuple.Create(250, 190); //"250x190";
                case BannerType.SidebarBanner:
                    return Tuple.Create(380, 300); //"380x300";
            }

            throw new BusinessCriticalLogicException(
            @"Ups! No variants for banner type, 
            please check the quantity of variants,
            perhaps someone added additional type?");
        }
    }
}
