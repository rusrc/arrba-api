using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.Extensions;
using Arrba.Services;
using Arrba.Services.Configuration;
using Arrba.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly ApplicationConfiguration _configuration;

        public NewsController(ApplicationConfiguration configuration, ILogService logService)
        {
            _configuration = configuration;
            _logService = logService;
        }
        /// <summary>
        /// get news
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{count}", Name = "get news")]
        public async Task<ActionResult<Brand>> Get(int count)
        {
            const string newsKey = "newsKey";

            using (new MultiPointTimer("get news", _logService))
            {
                object newsFromCache = null;
                if (_configuration.UseMemaryCaching)
                {
                    newsFromCache = CacheService.GetData<object>(newsKey);
                }

                if (newsFromCache != null)
                {
                    return Ok(newsFromCache);
                }

                var uri = new Uri($"https://news.arrba.ru/wp-json/wp/v2/posts?per_page={count}&context=embed&_embed");
                using (var client = new WebClient())
                {
                    var result = await client.DownloadStringTaskAsync(uri);

                    var items = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(result)
                        .Select(item => new
                        {
                            item.date,
                            mediaId = item.featured_media,
                            item.link,
                            description = this.getShortDescription(item.excerpt?.rendered),
                            thumbnail = this.getSafe(() => item._embedded["wp:featuredmedia"][0].media_details.sizes.thumbnail.source_url),
                            imageMedium = this.getSafe(() => item._embedded["wp:featuredmedia"][0].media_details.sizes.medium.source_url)
                        });

                    if (_configuration.UseMemaryCaching)
                    {
                        await CacheService.SetDataAsync(newsKey, items, TimeSpan.FromHours(12));
                    }

                    return Ok(items);
                }
            }
        }

        private string getShortDescription(object obj)
        {
            var result = obj.ToString();
            result = new Regex("<\\/?p>")
                .Replace(result, "")
                .Left(40);

            return result;
        }

        private string getSafe(Func<string> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                // TODO log here
                return "";
            }
        }
    }
}
