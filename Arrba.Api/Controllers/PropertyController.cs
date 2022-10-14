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
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public PropertyController(IUnitOfWork unitOfWork, IMapper mapper, ILogService logService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logService = logService;
        }

        /// <summary>
        /// Get property by id
        /// </summary>
        /// <param name="id">property id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}", Name = "Get property by id")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<Property>> Get(long id)
        {
            return Ok(await _unitOfWork.PropertyRepository.GetAsync(id));
        }

        /// <summary>
        /// get property by descripion name with like query
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}/like", Name = "get property by descripion name with like query")]
        public async Task<ActionResult<PropertyDto>> Get(string name)
        {
            var properties = await _unitOfWork
                .PropertyRepository
                .GetAllAsync(true, p => p.Description.Contains(name));

            var property = properties.FirstOrDefault();

            if (property == null)
            {
                return NotFound();
            }

            var mapped = this._mapper.Map<Property, PropertyDto>(property);

            return Ok(mapped);
        }

        /// <summary>
        /// Get property by category id
        /// </summary>
        /// <param name="categoryId">categoryId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("category/{categoryId}", Name = "Get property by category id")]
        public async Task<ActionResult<IEnumerable<PropertyFilterDto>>> GetByCategoryId(long categoryId)
        {

            var properities = await _unitOfWork.PropertyCategoryRepository.GetAllAsync(categoryId);

            if (properities == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<IEnumerable<PropertyCateg>, IEnumerable<PropertyFilterDto>>(properities);

            return Ok(result);
        }

        /// <summary>
        /// Get property by category id with checkbloxes
        /// </summary>
        /// <param name="categoryId">categoryId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("category/{categoryId}/add-checkboxes", Name = "Get property by category id with checkbloxes")]
        public async Task<ActionResult<IEnumerable<PropertyFilterDto>>> GetByCategoryIdWithCheckboxes(long categoryId)
        {
            using (new MultiPointTimer($"call: get property by category id {categoryId} with checkbloxes", _logService))
            {
                var properities = await _unitOfWork
                    .PropertyCategoryRepository
                    .GetAllAsync(categoryId, addCheckBoxs: true);

                if (properities == null)
                {
                    return NotFound();
                }

                var result = _mapper.Map<IEnumerable<PropertyCateg>, IEnumerable<PropertyFilterDto>>(properities);

                return Ok(result);
            }
        }
    }
}
