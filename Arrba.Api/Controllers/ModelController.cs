using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.DTO;
using Arrba.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ModelController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Get Models By CategoryId And BrandId
        /// </summary>
        /// <param name="categoryId">categoryId</param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("category/{categoryId}/brand/{brandId}", Name = "Get Models By CategoryId And BrandId")]
        public async Task<ActionResult<IEnumerable<ItemFilterDto>>> GetByCategId(long categoryId, long brandId)
        {
            var models = await _unitOfWork.ModelRepository.GetModels(categoryId, brandId);
            var result = this._mapper.Map<IEnumerable<ItemModel>, IEnumerable<ItemFilterDto>>(models);

            return Ok(result);
        }

        /// <summary>
        /// get model by brandId and model name
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("brand/{brandId}/model/{modelName}", Name = "get model by brandId and model name")]
        public async Task<ActionResult<IEnumerable<ItemFilterDto>>> GetByBrandId(long brandId, string modelName)
        {
            var model = await _unitOfWork
                .ModelRepository
                .GetAsync(m => m.BrandID == brandId && m.Name == modelName);

            if (model == null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<ItemModel, ItemFilterDto>(model);

            return Ok(result);
        }

        [HttpGet]
        [Route("all", Name = "get models")]
        public async Task<ActionResult<IEnumerable<ItemFilterDto>>> Get()
        {
            var models = await _unitOfWork.ModelRepository.GetAllAsync();

            if (models == null)
            {
                return NotFound();
            }

            // var result = this._mapper.Map<ItemModel, ItemFilterDto>(model);

            return Ok(models);
        }
    }
}
