using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.DTO;
using Arrba.Repositories;
using Arrba.Services.Logger;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public ItemTypeController(IUnitOfWork unitOfWork, IMapper mapper, ILogService logService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logService = logService;
        }

        /// <summary>
        /// Get type by id
        /// </summary>
        /// <param name="id">type id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}", Name = "GetTypeById")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ItemType>> Get(long id)
        {
            var type = await _unitOfWork.ItemTypeRepository.GetAsync(id);

            if (type == null)
            {
                return NotFound();
            }

            return Ok(type);
        }

        // TODO optimize query, remove SingleOrDefault(t => t.Name.Contains(alias));
        /// <summary>
        /// get type by categoryId and type's name
        /// </summary>
        /// <param name="categoryId">categoryId</param>
        /// <param name="name">English from or translatiration</param>
        /// <returns></returns>
        [HttpGet]
        [Route("categoryId/{categoryId}/typeName/{name}", Name = "get type by categoryId and type's name")]
        public async Task<ActionResult<ItemType>> GetByCategId(long categoryId, string name)
        {
            var types = await _unitOfWork.ItemTypeRepository.GetItemTypes(categoryId);
            var type = types.SingleOrDefault(t => t.Name.Contains(name));

            if (type == null)
            {
                return NotFound();
            }

            return Ok(type);
        }

        // TODO optimize query, remove types.SingleOrDefault(t => t.ID == id);
        /// <summary>
        /// get type by categoryId and type's name
        /// </summary>
        /// <param name="categoryId">categoryId</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("categoryId/{categoryId}/typeId/{id}", Name = "get type by categoryId and type's id")]
        public async Task<ActionResult<ItemType>> GetByTypeIdAndCategId(long categoryId, long id)
        {
            var types = await _unitOfWork.ItemTypeRepository.GetItemTypes(categoryId);
            var type = types.SingleOrDefault(t => t.ID == id);

            if (type == null)
            {
                return NotFound();
            }

            return Ok(type);
        }

        /// <summary>
        /// Get types by category id
        /// </summary>
        /// <param name="categoryId">categoryId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ByCategoryId/{categoryId}", Name = "get types by categoryId")]
        public async Task<ActionResult<IEnumerable<ItemType>>> GetByCategId(long categoryId)
        {
            using (new MultiPointTimer($"call: get types by categoryId {categoryId}", _logService))
            {
                var type = await _unitOfWork.ItemTypeRepository.GetItemTypes(categoryId);

                if (type == null)
                {
                    return NotFound();
                }

                return Ok(type);
            }
        }

        [HttpGet]
        [Authorize/*(Roles = "admin")*/]
        [Route("all", Name = "get all types")]
        public async Task<ActionResult<IEnumerable<ItemType>>> GetAll()
        {
            var types = await _unitOfWork.ItemTypeRepository.GetAllAsync();

            if (types.Any() is false)
            {
                return NotFound();
            }

            return Ok(types);
        }

        [HttpGet]
        [Authorize/*(Roles = "admin")*/]
        [Route("all/{name}/name", Name = "get types by type's name")]
        public async Task<ActionResult<IEnumerable<ItemType>>> GetAllByAlias(string name)
        {
            var types = await _unitOfWork.ItemTypeRepository.GetAllAsync(t => t.Name.Contains(name));

            if (types.Any() is false)
            {
                return NotFound();
            }

            return Ok(types);
        }

        [HttpPost]
        [Authorize/*(Roles = "admin")*/]
        [Route("save/{name}/name", Name = "add new type")]
        public async Task<ActionResult<IEnumerable<ItemType>>> Save([FromBody]ItemType model)
        {
            var type = await _unitOfWork.ItemTypeRepository.GetAsync(t => t.Name.Contains(model.Name));

            if (type == null)
            {
                _unitOfWork.ItemTypeRepository.Add(model);
                await _unitOfWork.CompleteAsync();
            }

            ModelState.AddModelError("", $"{model.Name} already exists");
            return BadRequest(ModelState);
        }

        /// <summary>
        /// get all types with category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("allWithCategory", Name = "get all types with category")]
        public async Task<ActionResult<IEnumerable<TypeCategoryDto>>> GetAllWithCategory()
        {
            var categoryTypes = await _unitOfWork.CategoryTypeRepository.GetAllAsync();

            if (categoryTypes.Any() is false)
            {
                return NotFound();
            }

            var mappedItems = this._mapper.Map<IEnumerable<CategType>, IEnumerable<TypeCategoryDto>>(categoryTypes);

            return Ok(mappedItems);
        }
    }
}
