using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain.DAL;
using Arrba.Domain.Models;
using Arrba.Domain.ModelsView;
using Arrba.ImageLibrary;
using Arrba.ImageLibrary.Json;
using Arrba.Repositories;

namespace Arrba.Domain.Repositories.EntityFramework.MSSQL
{
    using CONSTS = Constants.CONSTANT;
    using SETTTS = Constants.SETTINGS;
    public class AdHotRawRepository : IHotAdRepository
    {
        private readonly DbArrbaContext  _ctx;
        public AdHotRawRepository(DbArrbaContext ctx)
        {
            this._ctx = ctx;
        }
        public async Task<IEnumerable<HotAdMV>> GetHotAds(long countryId, long regionId, long cityId)
        {
            using (var conn = new SqlConnection(this._ctx.Database.Connection.ConnectionString))
            {
                if(conn.State == ConnectionState.Closed) conn.Open();

                var query = $@"SELECT * FROM {CONSTS.DB_FUN_GETHOTADVEHICLE}
                                (
                                    {cityId}, 
                                    {regionId}, 
                                    {countryId}, 
                                    '{ServiceEnum.IsHot}', 
                                    {SETTTS.HotAdLimit}
                                )";

                var list = new List<HotAdVehicle>();
                using (var reader = new SqlCommand(query, conn).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new HotAdVehicle
                        {
                            Id = reader.GetInt64(0),
                            CategId = reader.GetInt64(1),
                            CityName =  reader.GetString(2),
                            Title = reader.GetString(3),
                            Price = reader.GetDouble(4),
                            Symbol = reader.GetString(5),
                            ImgJson = reader.GetString(6),
                            FolderImgName = reader.GetString(7),
                            BoughtDate = reader.GetDateTime(8),
                            Text = reader.GetString(9),
                            Year = reader.GetString(10)
                        });
                    }
                }

                if (conn.State == ConnectionState.Open) conn.Close();

                var result = list.Select(item => new HotAdMV
                    {
                        ID = item.Id,
                        CategID =  item.CategId,
                        CityName = item.CityName,
                        ModelName = item.Title,
                        Title = item.Title,
                        FolderImgName = item.FolderImgName,
                        ImgJson = ImgManager.GetImgPath(item.CategId.ToString(), item.FolderImgName, item.ImgJson, CONSTS.SMALL_FILE_NAME_PREFIX),
                        Price = item.Price,
                        Year = item.Year,
                        CurrencyName = item.Symbol,
                        ImgJsonObject = ImgJson.Parse(item.ImgJson),
                        Text = string.Concat(new string(item.Text.Take(50).ToArray()), "..."),
                        TypeName = ""
                    })
                    .ToList();

                return await Task.FromResult(result);

            }
        }
    }


  internal class HotAdVehicle
    {
        public long Id { get; set; }
        public long CategId { get; set; }
        public string CityName { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Symbol { get; set; }
        public string ImgJson { get; set; }
        public string FolderImgName { get; set; }
        public DateTime BoughtDate { get; set; }
        public string Text { get; set; }
        public string Year { get; set; }
    }
}
