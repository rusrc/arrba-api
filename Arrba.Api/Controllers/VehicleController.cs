using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Arrba.Api.Jwt;
using Arrba.Constants;
using Arrba.Domain.Models;
using Arrba.DTO;
using Arrba.Exceptions;
using Arrba.Extensions;
using Arrba.ImageLibrary;
using Arrba.Repositories;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;
using Arrba.Services.Configuration;
using Arrba.Services.Logger;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Arrba.Constants.CONSTANT;


namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        private readonly ApplicationConfiguration _configuration;
        private readonly IImgManager _imgManager;
        private readonly IDeclineService _declineService;

        public VehicleController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHostingEnvironment appEnvironment,
            ILogService logService,
            ApplicationConfiguration configuration,
            IImgManager imgManager,
            IDeclineService declineService)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._logService = logService;
            this._configuration = configuration;
            this._imgManager = imgManager;
            this._declineService = declineService;
        }

        /// <summary>
        /// Return errors form ModelState.Values.Errors
        /// </summary>
        string Errors => !ModelState.IsValid ? ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(error => error.ErrorMessage + " " + error.Exception?.Message)
            .Aggregate((prev, next) => prev + "/n" + next) : null;

        /// <summary>
        /// Get single vehicle
        /// </summary>
        /// <param name="adVehicleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{adVehicleId}", Name = "Get vehicle by Id")]
        public async Task<ActionResult<VehicleDto>> Get(long adVehicleId)
        {
            var vehicle = await _unitOfWork.AdVehicleRepository.GetAsync(adVehicleId);
            var result = _mapper.Map<AdVehicle, VehicleWithPropertyDto>(vehicle);

            // TODO disable right now
            //if (vehicle != null)
            //{
            //    await AddView(vehicle);
            //}

            return Ok(result);
        }

        [HttpGet]
        [Route("dealershipId/{dealershipId}", Name = "Get vehicle list by dealershipId")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetByDealershipId(long dealershipId, long? currencyId, PriceSorter? sorter, int? pageNumber)
        {
            pageNumber = pageNumber ?? 1;
            Currency currency = null;

            if (currencyId.HasValue)
            {
                currency = await this._unitOfWork.CurrencyRepository.GetAsync(c => c.ID == currencyId);
            }

            var vehicles = await _unitOfWork.AdVehicleRepository.GetByDealershipIdAsync(dealershipId, (int)pageNumber, SETTINGS.CountRows, currency, sorter);
            var count = await _unitOfWork.AdVehicleRepository.GetCountByDealershipIdAsync(dealershipId);
            var mappedItems = _mapper.Map<IEnumerable<AdVehicle>, IEnumerable<VehicleDto>>(vehicles);
            var result = mappedItems.ToPagedList(SETTINGS.CountRows, count, (int)pageNumber);

            return Ok(result);
        }

        /// <summary>
        /// Get items list
        /// User for simple GET query
        /// Example: api/vahicle/{regionId}/{cityId}?superCategId=7&categId=37&pageNumber=3'
        /// </summary>
        /// <param name="regionId">cityId</param>
        /// <param name="cityId">cityId</param>
        /// <param name="superCategId">superCategoryId</param>
        /// <param name="categId">categoryId</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <returns>list</returns>
        [HttpGet]
        [Route("{regionId}/{cityId}", Name = "Get vehicle list by categoryId")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Get(long regionId, long cityId,
            [FromQuery]long superCategId,
            [FromQuery]long categId,
            [FromQuery]int? pageNumber)
        {
            var queryString = Request.GetDisplayUrl(); // Request.RequestUri.Query
            var page = pageNumber ?? 1;
            var items = await _unitOfWork.AdVehicleRepository.GetAsync(queryString, regionId, cityId, page);
            var count = await _unitOfWork.AdVehicleRepository.GetCountAsync(queryString, regionId, cityId, page);
            var mappedItems = _mapper.Map<IEnumerable<AdVehicle>, IEnumerable<VehicleDto>>(items);
            var result = mappedItems.ToPagedList(SETTINGS.CountRows, count, page);

            return Ok(result);
        }

        /// <summary>
        /// TODO change later
        /// </summary>
        [HttpPost]
        [Route("all", Name = "Get vehicle list without brandName, modelName, regionName, cityName")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Post(
            [FromQuery] int? pageNumber,
            [FromBody] KeyValuePair<string, string>[] @params)
        {
            return await Post(/*null,*/ null, null, null, pageNumber, @params);
        }


        /// <summary>
        /// TODO change later
        /// Get vehicle list BrandName
        /// </summary>
        [HttpPost]
        [Route("brand/{brandName}/all", Name = "Get vehicle list BrandName NoCity")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> PostNoCity(
            string brandName,
            [FromQuery] int? pageNumber,
            [FromBody] KeyValuePair<string, string>[] @params)
        {
            return await Post(/*regionId,*/ null, brandName, null, pageNumber, @params);
        }

        /// <summary>
        /// TODO change later
        /// Get vehicle list BrandName
        /// </summary>
        [HttpPost]
        [Route("brand/{brandName}/model/{modelName}/all", Name = "Get vehicle list BrandName, model NoCity")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> PostNoCity(
            string brandName,
            string modelName,
            [FromQuery] int? pageNumber,
            [FromBody] KeyValuePair<string, string>[] @params)
        {
            return await Post(/*regionId,*/ null, brandName, modelName, pageNumber, @params);
        }

        /// <summary>
        /// TODO change later
        /// Get vehicle list without brandName and modelName
        /// </summary>
        [HttpPost]
        [Route("city/{cityAlias}/all", Name = "Get vehicle list without brandName and modelName")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Post(
            // string regionAlias, // TODO region set later
            string cityAlias,
            [FromQuery] int? pageNumber,
            [FromBody] KeyValuePair<string, string>[] @params)
        {
            // TODO remove when region will be set
            // regionAlias = regionAlias == "null" || regionAlias == "undefined" ? null : regionAlias;

            return await Post(/*regionAlias,*/ cityAlias, null, null, pageNumber, @params);
        }

        /// <summary>
        /// TODO change later
        /// Get vehicle list BrandName
        /// </summary>
        [HttpPost]
        [Route("city/{cityAlias}/brand/{brandName}/all", Name = "Get vehicle list BrandName")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Post(
            // long regionId,  // TODO region set later
            string cityAlias,
            string brandName,
            [FromQuery] int? pageNumber,
            [FromBody] KeyValuePair<string, string>[] @params)
        {
            return await Post(/*regionId,*/ cityAlias, brandName, null, pageNumber, @params);
        }

        /// <summary>
        /// TODO change later
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("city/{cityAlias}/brand/{brandName}/model/{modelName}/all", Name = "Get vehicle list BrandName, ModelName, CityAlias")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Post(
            // string regionAlias,  // TODO region set later
            string cityAlias,
            string brandName,
            string modelName,
            [FromQuery] int? pageNumber,
            [FromBody] KeyValuePair<string, string>[] @params)
        {
            //var regionId = 0L; // TODO region set later
            var cityId = 0L;

            if (!string.IsNullOrEmpty(cityAlias))
            {
                var city = await _unitOfWork.CityRepository.GetAsync(c => c.Alias.Equals(cityAlias, StringComparison.CurrentCultureIgnoreCase));
                cityId = city.ID;
            }

            return await Post(/*regionId,*/ cityId, brandName, modelName, pageNumber, @params);
        }

        [Authorize]
        [HttpPost]
        [Route("add", Name = "AddItem")]
        public async Task<ActionResult<string>> Add([FromBody] AddNewVehicleDto model)
        {
            if (model.IsDealer is false && PhonesIsValid(model.PhoneNumbers, out string errorMessage) is false)
            {
                ModelState.AddModelError("", errorMessage);
            }

            if (!ModelState.IsValid)
            {
                throw new BusinessLogicException(this.Errors);
            }

            // 1. Get user if exsits in db. Otherwise create new
            User currenUser;

            if (User.Identity.IsAuthenticated && JwtManager.RoleExsits(User.Identity, GUEST))
            {
                currenUser = await _unitOfWork.UserRepository.CreateUnauthorizedUserOrGetExistedAsync(model.Email);
            }
            else
            {
                currenUser = await _unitOfWork.UserRepository.GetAsync(User.GetUserEmail(), loadPhones: true);
            }

            var now = DateTime.Now;
            //var imgJson = (await _imgManager.SaveImagesAsync(
            //    model.CategoryId.ToString(), 
            //    model.TemporaryImageFolder)).ToString();

            var imgJson = (await _imgManager.SaveImagesAsync(model.TemporaryImageFolder, model.TemporaryImageFolder)).ToString();

            var adVehicle = new AdVehicle
            {
                BrandID = model.BrandId,
                ModelID = model.ModelId,
                CountryID = model.CountryId,
                CityID = model.CityId,
                TypeID = model.TypeId,
                Price = model.Price,
                MinimalPrice = model.MinimalPrice,
                Year = model.Year,
                CurrencyID = model.CurrencyId,
                Comment = model.AdditionalComment,
                FolderImgName = model.TemporaryImageFolder,
                SuperCategID = model.SuperCategoryId,
                CategID = model.CategoryId,
                DealershipId = model.DealershipId,
                Condition = model.Condition,

                User = currenUser,
                ImgJson = imgJson,
                AddDate = now,
                LastModified = now,
                DateExpired = now.AddDays(_configuration.AdvertLifeDays),
            };

            if (model.Properties != null)
            {
                adVehicle.DynamicPropertyAds = new List<DynamicPropertyAdVehicle>();

                var properties = model.Properties.ToObject<Dictionary<string, object>>();
                var @params = await ReplaceParamsAsync(properties);
                foreach (var param in @params)
                {
                    adVehicle.DynamicPropertyAds.Add(new DynamicPropertyAdVehicle
                    {
                        AdVehicleID = adVehicle.ID,
                        PropertyID = long.Parse(param.Key),
                        PropertyValue = param.Value.ToString()
                    });
                }

                adVehicle.Description = await this.GetPropertyDesriptionAsync(adVehicle.DynamicPropertyAds);
            }

            if (model.IsDealer is false)
            {
                this.AddPhones(currenUser.UserPhones, model.PhoneNumbers);
            }

            try
            {
                this._unitOfWork.AdVehicleRepository.Add(adVehicle);
                await this._unitOfWork.CompleteAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _logService.Error(ex.Message, ex);
                _logService.Error(@"adVehicle json: " + JsonConvert.SerializeObject(adVehicle, Formatting.Indented));
                throw;
            }

            return Ok(adVehicle.ID);
        }

        [HttpPost]
        [Authorize]
        [Route("deactualize", Name = "item sold or not found")]
        public async Task<ActionResult<string>> Deactualize(long id)
        {
            var item = await this._unitOfWork.AdVehicleRepository.GetAsync(v => v.ID == id);

            if (item == null)
            {
                return NotFound();
            }

            item.DateExpired = item.AddDate;
            await this._unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpPost]
        [Route("seoTitle", Name = "seo title")]
        [Produces("application/json")]
        public async Task<Seo> GetSeoTitleForItme(VehicleTitleSeo model)
        {
            // купить прицепы [в России]
            // купить прицепы платформа [в России]
            // купить прицепы платформа в Москве

            // купить прицепы BMW [в России]
            // купить прицепы платформа BWM [в России]
            // купить прицепы платформа BMW в Москве

            // купить прицепы BMW 100 [в России]
            // купить прицепы платформа BWM 100 [в России]
            // купить прицепы платформа BMW 100 в Москве

            using (var timer = new MultiPointTimer($"{nameof(VehicleController)}.{nameof(GetSeoTitleForItme)}", _logService))
            {
                City city = null;
                Category category = null;
                ItemType type = null;
                Brand brand = null;
                ItemModel itemModel = null;

                if (!string.IsNullOrEmpty(model.CityAlias))
                    city = await _unitOfWork.CityRepository.GetAsync(model.CityAlias);

                if (!string.IsNullOrEmpty(model.CategoryName))
                    category = await _unitOfWork.CategoryRepository.GetAsync(model.CategoryName);

                if (model.TypeId.HasValue)
                    type = await _unitOfWork.ItemTypeRepository.GetAsync((long)model.TypeId);

                if (!string.IsNullOrEmpty(model.BrandName))
                    brand = await _unitOfWork.BrandRepository.GetAsync(model.BrandName);

                if (!string.IsNullOrEmpty(model.ModelName) && brand != null)
                    itemModel = await _unitOfWork.ModelRepository.GetAsync(model.BrandName, brand.ID);


                var cityNamePrepositional = city != null ? _declineService.GetPrepositional(city.Name) : null;

                var result = new Seo("Купить", category?.Name, type?.Name, brand?.Name, itemModel?.Name, "arrba.ru")
                {
                    Title = GenerateTitle(),
                    H1 = GenerateTitle(),
                    MetaDescription = GenerateDescription(),
                    Breadcrumbs = BreadcrumbRoutes()
                };

                timer.AddPoint("Soe.Dto generated");

                return result;

                string GenerateTitle()
                {
                    var str = $"Купить {category?.Name} {type?.Name} {brand?.Name} {itemModel?.Name}";
                    if (city != null)
                    {
                        str += $"в {cityNamePrepositional}";
                    }
                    return str;
                }

                string GenerateDescription()
                {
                    var str = $"На сайте arrba.ru можно купить {category?.Name} {brand?.Name} недорого.";
                    if (city != null)
                    {
                        str += $" {category?.Name} {brand?.Name} по низкой стоимости в {cityNamePrepositional}";
                    }
                    return str;
                }

                IEnumerable<KeyValuePair<string, string[]>> BreadcrumbRoutes()
                {
                    var crumbs = new List<KeyValuePair<string, string[]>>
                {
                    new KeyValuePair<string, string[]>("главная", new[] {"/"})
                };

                    if (city != null)
                    {
                        crumbs.Add(new KeyValuePair<string, string[]>($"{category.Name} в {cityNamePrepositional}",
                            new[] { "/", "cities", city.Alias, "categories", category.Alias }));

                        if (brand != null)
                        {
                            crumbs.Add(new KeyValuePair<string, string[]>($"{brand.Name} в {cityNamePrepositional}",
                                new[] { "/", "cities", city.Alias, "categories", category.Alias, brand.Name }));
                        }
                    }
                    else
                    {
                        crumbs.Add(new KeyValuePair<string, string[]>(category.Name,
                            new[] { "/", "categories", category.Alias }));

                        if (brand != null)
                        {
                            crumbs.Add(new KeyValuePair<string, string[]>(brand.Name,
                                new[] { "/", "categories", category.Alias, brand.Name }));
                        }
                    }

                    return crumbs;
                }
            }

        }

        #region private methods

        private async Task AddView(AdVehicle vehicle)
        {
            vehicle.ViewCount++;
            _unitOfWork.AdVehicleRepository.Update(vehicle);
            await _unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// Get the list of AdVehicle
        /// Use for complexe query
        /// </summary>
        private async Task<ActionResult<IEnumerable<VehicleDto>>> Post(
            // long regionId,  // TODO region set later
            long cityId,
            string brandName,
            string modelName,
            [FromQuery] int? pageNumber,
            [FromBody] KeyValuePair<string, string>[] @params)
        {
            using (var timer = new MultiPointTimer($"{nameof(VehicleController)}.{nameof(Post)}", _logService))
            {
                if (@params.Length > 0)
                {
                    @params = await ReplaceParamsAsync(@params);
                }

                var brand = null as Brand;
                if (string.IsNullOrEmpty(brandName) is false && (brand = await _unitOfWork.BrandRepository.GetAsync(brandName)) == null)
                {
                    return NotFound();
                }

                var model = null as ItemModel;
                if (string.IsNullOrEmpty(modelName) is false && (model = await _unitOfWork.ModelRepository.GetAsync(modelName, brand.ID)) == null)
                {
                    return NotFound();
                }

                if (brand?.ID > 0) AddParam(ref @params, "BrandID", brand.ID);
                if (model?.ID > 0) AddParam(ref @params, "ModelID", model.ID);

                var regionId = 0; // TODO add region when will be
                var page = pageNumber ?? 1;
               
                var items = await _unitOfWork.AdVehicleRepository.GetAsync(regionId, cityId, page, @params);
                timer.AddPoint("End get items query");

                var count = await _unitOfWork.AdVehicleRepository.GetCountAsync(regionId, cityId, page, @params);
                timer.AddPoint("End get count items query");

                var mappedItems = _mapper.Map<IEnumerable<AdVehicle>, IEnumerable<VehicleDto>>(items);

                if (mappedItems.Any() is false)
                {
                    return NotFound();
                }

                PagedListDto<VehicleDto> result = mappedItems.ToPagedList(SETTINGS.CountRows, count, page);
                timer.AddPoint("End mapping items");

                return Ok(result);
            }
        }

        private void AddPhones(ICollection<UserPhone> userPhones, string[] newNumbers)
        {
            foreach (var newNumber in GetNumbers(newNumbers))
            {
                if (userPhones.Any(userPhone => userPhone.Number == newNumber) == false)
                {
                    userPhones.Add(new UserPhone { Number = newNumber });
                }
            }
        }

        private async Task<string> GetPropertyDesriptionAsync(IEnumerable<DynamicPropertyAdVehicle> dynamicPropertys)
        {
            if (!dynamicPropertys.Any()) return null;

            var ids = dynamicPropertys.Select(dp => dp.PropertyID).ToArray();
            var properties = await _unitOfWork
                .PropertyRepository
                .GetAllAsync(true, p => ids.Contains(p.ID));

            var result = dynamicPropertys.Select(dp =>
            {
                var property = properties.SingleOrDefault(p => p.ID == dp.PropertyID);
                if (property.ControlType == ControlTypeEnum.Select)
                {
                    var option = property.SelectOptions.SingleOrDefault(o => o.ID.ToString() == dp.PropertyValue);
                    return $"{property.Description}: {option.NameMultiLang}";
                }

                if (property.ControlType == ControlTypeEnum.CheckBox)
                {
                    return $"{property.Description}";
                }

                return $"{property.Description}: {dp.PropertyValue} {property.UnitMeasure}";
            })
                .Aggregate((prev, next) => $"{prev}, {next}");

            return result;

        }

        private string[] GetNumbers(string[] numbers)
        {
            return numbers
                    .Select(number => Regex.Matches(number, "\\d+")
                    .Select(m => m.Value)
                    .Aggregate((prev, next) => prev + next)).ToArray();
        }

        private void AddParam(ref KeyValuePair<string, string>[] @params, string key, object value)
        {
            @params = @params.Concat(new[] { new KeyValuePair<string, string>(key, value.ToString()) }).ToArray();
        }

        private bool PhonesIsValid(IEnumerable<string> numbers, out string errorMessage)
        {
            errorMessage = null;

            //If phone empty or invalid format
            var phoneRegexp = new Regex(REGEXP_PHONE_NUMBER, RegexOptions.IgnoreCase);
            var isValid = numbers.Any(number => phoneRegexp.Match(number ?? "").Success);

            if (!isValid)
            {
                errorMessage = "Телефон указан в неверном формате. Пример +7 999 9999999";
            }

            return isValid;
        }

        /// <summary>
        /// Solution to replace properties with names to propeties id. 
        /// For proper query into sql database
        /// </summary>
        /// <param name="params"></param>
        private async Task<KeyValuePair<string, string>[]> ReplaceParamsAsync(IEnumerable<KeyValuePair<string, string>> @params)
        {
            var propertyNames = @params.Select(p => p.Key).ToArray();
            var properties = await _unitOfWork.PropertyRepository.GetAllAsync(p => propertyNames.Contains(p.Name));
            var paramsList = @params.ToList();

            foreach (var property in properties)
            {
                var param = paramsList.SingleOrDefault(p => p.Key == property.Name);
                if (string.IsNullOrEmpty(param.Key) == false)
                {
                    paramsList.Add(new KeyValuePair<string, string>(property.ID.ToString(), param.Value));
                    paramsList.Remove(param);
                }
            }

            return paramsList.ToArray();
        }

        /// <summary>
        /// Solution to replace properties with names to propeties id. 
        /// For proper query into sql database
        /// </summary>
        /// <param name="params"></param>
        private async Task<Dictionary<string, object>> ReplaceParamsAsync(Dictionary<string, object> @params)
        {
            var propertyNames = @params.Select(p => p.Key).ToArray();
            var properties = await _unitOfWork.PropertyRepository.GetAllAsync(p => propertyNames.Contains(p.Name));
            var paramsList = @params;

            foreach (var property in properties)
            {
                var param = paramsList.SingleOrDefault(p => p.Key == property.Name);
                if (string.IsNullOrEmpty(param.Key) is false && string.IsNullOrEmpty(param.Value.ToString()) is false)
                {
                    var value = param.Value.ToString();
                    if (value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        value = "1";
                    }
                    if (value.Equals("false", StringComparison.InvariantCultureIgnoreCase))
                    {
                        value = "0";
                    }

                    paramsList.Add(property.ID.ToString(), value);
                }
                paramsList.Remove(param.Key);
            }

            return paramsList;
        }
        #endregion
    }
}
