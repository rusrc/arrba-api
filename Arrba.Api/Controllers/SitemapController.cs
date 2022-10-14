using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Arrba.Domain.Models;
using Arrba.Repositories;
using Arrba.Services.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SitemapController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationConfiguration _configuration;

        public SitemapController(IUnitOfWork unitOfWork, ApplicationConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // TODO add unit test
        /// <summary>
        /// generate sitemap.xml
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("xml", Name = "generate sitemap.xml")]
        [Produces("application/xml")]
        public async Task<ActionResult<string[]>> Get()
        {
            var nodes = new List<SiteMapNode>();
            var vehicles = await _unitOfWork.AdVehicleRepository
                .GetForSiteMapAsync(v => v.AdStatus == AdStatus.IsOk && v.ModirationStatus == ModirationStatus.Ok);

            // TODO BUG
            // https://arrba.ru/cities/Voronezh/categories/Pricepy/AvtoS---92/item
            foreach (var v in vehicles)
            {
                var itemName = $"{v.Brand.Name} {v.Model?.Name} {v.Year} {v.ID}";

                itemName = Regex.Replace(itemName, @"\s", "-");
                itemName = Regex.Replace(itemName, @"\-{2,10}", "-");

                nodes.Add(new SiteMapNode
                {
                    Url = $"{_configuration.UiHost}/cities/{v.City?.Alias}/categories/{v.Categ?.Alias}/{itemName}/item",
                    Priority = 0.9,
                    LastModified = v.LastModified ?? v.AddDate,
                    Frequency = SitemapFrequency.Weekly
                });
            }

            var xml = GetSiteMapDocument(nodes);

            return new ContentResult
            {
                ContentType = "application/xml",
                Content = xml,
                StatusCode = 200
            };
        }

        private string GetSiteMapDocument(IEnumerable<SiteMapNode> siteMapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");

            foreach (var siteMapNode in siteMapNodes)
            {
                XElement urlxXElement =
                    new XElement(xmlns + "url",
                        new XElement(xmlns + "loc", Uri.EscapeUriString(siteMapNode.Url)),
                    siteMapNode.LastModified == null ? null :
                        new XElement(xmlns + "lastmod", siteMapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    siteMapNode.Frequency == null ? null :
                       new XElement(xmlns + "changefreq", siteMapNode.Frequency.ToString()),
                    siteMapNode.Priority == null ? null :
                       new XElement(xmlns + "priority", siteMapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));

                root.Add(urlxXElement);
            }

            XDocument document = new XDocument(root)
            {
                Declaration = new XDeclaration("1.0", "UTF-8", "no")
            };

            return document.ToString();
        }

        internal class SiteMapNode
        {
            public SitemapFrequency? Frequency { get; set; }
            public DateTime? LastModified { get; set; }
            public double? Priority { get; set; }
            public string Url { get; set; }
        }

        internal enum SitemapFrequency
        {
            Never,
            Yearly,
            Monthly,
            Weekly,
            Daily,
            Hourly,
            Always
        }
    }
}
