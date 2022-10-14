using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Arrba.Constants;
using Arrba.Domain.Models;
using Arrba.DTO;
using Arrba.Repositories;
using Arrba.Services;
using Arrba.Services.Configuration;
using Arrba.Services.Logger;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuperCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly ApplicationConfiguration _configuration;

        public SuperCategoryController(IUnitOfWork unitOfWork, ILogService logService, IMapper mapper, ApplicationConfiguration configuration)
        {
            this._unitOfWork = unitOfWork;
            this._logService = logService;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        /// <summary>
        /// Get all super category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll", Name = "Get all active super category")]
        public async Task<ActionResult<ICollection<SuperCategory>>> GetAllAsync()
        {
            var categories = await _unitOfWork
                .SuperCategoryRepository
                .GetAllAsync(superCategory => superCategory.Status == ActiveStatus.active);

            categories = categories.Select(sc => new SuperCategory
            {
                ID = sc.ID,
                Name = sc.Name,
                Alias = sc.Alias,
                Categories = sc.Categories.Where(c => c.Status == ActiveStatus.active).ToList()
            });

            var result = this._mapper.Map<IEnumerable<SuperCategory>, IEnumerable<SuperCategoryDto>>(categories);

            return Ok(result);
        }

        /// <summary>
        /// Get SuperCategories object with brands and count of brands
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSuperCategories", Name = "Get SuperCategories with brands and count of brands")]
        public async Task<ActionResult<IEnumerable<SuperCategoryWithBrandsDto>>> GetSuperCategories(string cityAlias = null)
        {
            using (var pointer = new MultiPointTimer($"Method: {nameof(GetSuperCategories)}", this._logService))
            {
                // TODO temp solution for caching
                var key = $"SuperCategoryWithBrands:list:{cityAlias ?? "all"}";
                var result = null as IEnumerable<SuperCategoryWithBrandsDto>;

                if (_configuration.UseMemaryCaching)
                {
                    result = CacheService.GetData<IEnumerable<SuperCategoryWithBrandsDto>>(key);
                }

                if (result != null)
                {
                    return Ok(result);
                }

                var predicateAndCity = await GetPredicateCityAsync(cityAlias);
                var predicate = predicateAndCity.predicate;
                var cityName = predicateAndCity.cityName;
                var categoriesWithBrands = await _unitOfWork.BrandRepository.GetCategoriesWithBrandsAsync(24, predicate);

                result =
                    from x in categoriesWithBrands
                    group x by new { x.SuperCategoryAlias, x.SuperCategoryName, x.SuperCategoryId }
                    into g
                    // TODO simplify the class to much unused properties
                    select new SuperCategoryWithBrandsDto
                    {
                        CityName = cityName,
                        SuperCategoryId = g.Key.SuperCategoryId,
                        SuperCategoryName = g.Key.SuperCategoryName,
                        SuperCategoryAlias = g.Key.SuperCategoryAlias,
                        Categories = g.GroupBy(c => new
                        {
                            c.CategoryId,
                            c.CategoryAlias,
                            c.CategoryName,
                            c.CategoryFileName
                        })
                            .Select(x => new CategoryWithBrandsDto
                            {
                                CategoryId = x.Key.CategoryId,
                                CategoryName = x.Key.CategoryName,
                                CategoryAlias = x.Key.CategoryAlias,
                                CategoryFileName = x.Key.CategoryFileName,
                                Brands = x.Select(t => new
                                {
                                    t.BrandName,
                                    t.VehicleCount
                                })
                            })
                    };

                if (_configuration.UseMemaryCaching)
                {
                    await CacheService.SetDataAsync(key, result, TimeSpan.FromHours(2));
                }

                return Ok(result);
            }
        }

        private async Task<(Expression<Func<AdVehicle, bool>> predicate, string cityName)>
        GetPredicateCityAsync(string cityAlias)
        {
            Expression<Func<AdVehicle, bool>> selectByCityIdPredicate =
                vehicle => vehicle.CountryID == _configuration.DefaultCountryId && vehicle.AdStatus == AdStatus.IsOk;

            string cityName = null;
            if (!string.IsNullOrEmpty(cityAlias))
            {
                City city = await _unitOfWork.CityRepository.GetAsync(c => c.Alias == cityAlias);
                if (city != null)
                {
                    cityName = city.NameMultiLang;
                    selectByCityIdPredicate = vehicle =>
                        vehicle.CityID == city.ID && vehicle.AdStatus == AdStatus.IsOk;
                }
            }

            return (predicate: selectByCityIdPredicate, cityName: cityName);
        }
    }
}
