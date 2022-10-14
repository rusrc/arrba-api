using System.Collections.Generic;
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
    /// Category
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        /// <summary>
        /// Constructor
        /// </summary>
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper, ILogService logService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logService = logService;
        }

        /// <summary>
        /// Get category by alias
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{alias}", Name = "Get category by alias")]
        public async Task<ActionResult<Category>> Get(string alias)
        {
            using (new MultiPointTimer($"category by alias: {alias}", _logService))
            {
                var category = await _unitOfWork.CategoryRepository.GetAsync(alias);
                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
        }

        /// <summary>
        /// Get category by categoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("id/{categoryId}", Name = "Get category by categoryId")]
        public ActionResult<Category> Get(long categoryId)
        {
            using (new MultiPointTimer($"get category by categoryId {categoryId}", _logService))
            {
                var category = _unitOfWork.CategoryRepository.Get(categoryId);
                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all", Name = "Get all categories")]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            using (new MultiPointTimer("get all categories", _logService))
            {
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync(
                    // TODO if not `Услуги`. Change logic later
                    c => c.SuperCategID != 5L);
                var mappedCategories = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);

                return Ok(mappedCategories);
            }
        }
    }
}
