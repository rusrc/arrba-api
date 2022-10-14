using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.DTO;
using Arrba.Repositories;
using Arrba.Services.Logger;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    /// <summary>
    /// Brands
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        /// <summary>
        /// Constructor
        /// </summary>
        public BrandController(IUnitOfWork unitOfWork, IMapper mapper, ILogService logService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logService = logService;
        }

        /// <summary>
        /// Get brand by brandName
        /// </summary>
        /// <param name="brandName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("name/{brandName}", Name = "Get brand by brandName")]
        public async Task<ActionResult<Brand>> GetBrandByBrandName(string brandName)
        {
            var brand = await _unitOfWork.BrandRepository.GetAsync(brandName);

            if (brand == null)
            {
                return NotFound();
            }

            return Ok(brand);
        }

        /// <summary>
        /// Get Brands By CategoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("category/{categoryId}", Name = "get brands by categoryId")]
        public async Task<ActionResult<IEnumerable<ItemFilterDto>>> GetByCategoryId(long categoryId)
        {
            using (new MultiPointTimer($"call: get brands by categoryId {categoryId}", _logService))
            {
                var brands = await _unitOfWork.CategoryBrandRepository.GetAllAsync(categoryId);

                if (brands == null || !brands.Any())
                {
                    return NotFound();
                }

                var result = _mapper.Map<IEnumerable<CategBrand>, IEnumerable<ItemFilterDto>>(brands);

                return Ok(result);
            }
        }

        /// <summary>
        /// Get all brands
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all", Name = "Get all Brands")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<IEnumerable<Brand>>> GetAll()
        {
            var brands = await this._unitOfWork.BrandRepository.GetAllAsync();

            if (brands == null || brands.Any() is false)
            {
                return NotFound();
            }

            return Ok(brands);
        }
    }
}
