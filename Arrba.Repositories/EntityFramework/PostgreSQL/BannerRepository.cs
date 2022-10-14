using System;
using System.Collections.Generic;
using Arrba.Domain;
using Arrba.Enumerations;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class BannerRepository
    {
        DbArrbaContext ctx;

        public BannerRepository(DbArrbaContext ctx)
        {
            this.ctx = ctx;
        }

        private static void MakeMixList<t>(IList<t> list)
        {
            Random random = new Random();
            SortedList<int, t> mixedList = new SortedList<int, t>();

            foreach (t item in list)
                mixedList.Add(random.Next(), item);

            list.Clear();

            for (int i = 0; i < mixedList.Count; i++)
            {
                list.Add(mixedList.Values[i]);
            } 
        }

    }

    public class SelectedBanners
    {
        public string Title { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string BannerLink { get; set; }
        public bool DefaultBanner { get; set; }
        public BannerType BannerType { get; set; }
    }
}
