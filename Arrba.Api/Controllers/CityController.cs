using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Constants;
using Arrba.Domain.Models;
using Arrba.DTO;
using Arrba.Repositories;
using Arrba.Services.Logger;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    /// <summary>
    /// City
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        #pragma warning disable CS1591
        public CityController(IUnitOfWork unitOfWork, IMapper mapper, ILogService logService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logService = logService;
        }

        /// <summary>
        /// Get all cities by countryId
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="isActive">if true return active cities only</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{countryId}/all", Name = "Get all cities by countryId")]
        public async Task<ActionResult<IEnumerable<CityDto>>> Get(long countryId, bool isActive = false)
        {
            using (new MultiPointTimer($"get all cities by countryId {countryId}, isActive {isActive}", _logService))
            {
                IEnumerable<City> cities;

                if (isActive)
                {
                    cities = await _unitOfWork.CityRepository.GetAllAsync(c => c.CountryID == countryId && c.Status == ActiveStatus.active);
                }
                else
                {
                    cities = await _unitOfWork.CityRepository.GetAllAsync(countryId);
                }

                var result = _mapper.Map<IEnumerable<City>, IEnumerable<CityDto>>(cities);

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("{countryId}/all/addLetters", Name = "Get all cities and first letters by countryId")]
        public async Task<ActionResult<(IEnumerable<CityDto> cities, IEnumerable<char> letters)>>
            GetWithLetters(long countryId)
        {
            var cities = await _unitOfWork.CityRepository.GetAllAsync(countryId);
            var letters = cities
                .Select(city => city.Name.FirstOrDefault())
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            var mappedCities = this._mapper.Map<IEnumerable<City>, IEnumerable<CityDto>>(cities);

            return Ok((cities: mappedCities, letters: letters));
        }

        /// <summary>
        /// Get all cities by ordered by weight
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{top}/allByWeight", Name = "Get all cities by ordered by weight")]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetOrderedByWeight(int top = 24, bool isActive = false)
        {
            var countryId = SETTINGS.DefaultCountryId;
            var cities = await _unitOfWork.CityRepository.GetByWeightAsync(top, countryId, isActive);
            var result = this._mapper.Map<IEnumerable<City>, IEnumerable<CityDto>>(cities);
            return Ok(result);
        }

        /// <summary>
        /// Get city by alias
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{alias}", Name = "Get city by alias")]
        public async Task<ActionResult<CityDto>> Get(string alias)
        {
            var city = await _unitOfWork.CityRepository.GetAsync(alias);

            if (city == null)
            {
                return NotFound();
            }
            var result = this._mapper.Map<City, CityDto>(city);

            return Ok(result);
        }

        /// <summary>
        /// Get city by name of city
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("name/{name}", Name = "Get city by name")]
        public async Task<ActionResult<CityDto>> GetCityByName(string name)
        {
            var city = await _unitOfWork.CityRepository.GetAsync(c => c.Name == name);

            if (city == null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<City, CityDto>(city);

            return Ok(result);
        }
    }
}
