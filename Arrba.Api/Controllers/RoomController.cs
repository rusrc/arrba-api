using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.Domain.ModelsView;
using Arrba.DTO;
using Arrba.Extensions;
using Arrba.Repositories;
using Arrba.Repositories.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        /// <summary>
        /// all items by sorter
        /// </summary>
        /// <param name="sorter"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("all/items", Name = "all items by sorter")]
        public async Task<ActionResult<IEnumerable<PagedListDto<VehicleDto>>>> Get(long? categoryId, int? page, RoomSorter sorter = RoomSorter.All)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(User.GetUserEmail());
            if (user == null)
            {
                return NotFound();
            }

            page = page ?? 1;
            categoryId = categoryId ?? 0;

            var items = _unitOfWork.AdVehicleRepository.GetAsync(user.Id, (long)categoryId, sorter);
            var pagedList = items.ToPagedList(12, (int)page);

            // TODO temp workarround
            var mappedItems = _mapper.Map<IEnumerable<AdVehicle>, IEnumerable<VehicleDto>>(pagedList.Items);
            var result = new PagedListDto<VehicleDto>
            {
                Items = mappedItems,
                PageSize = pagedList.PageSize,
                FirstItemOnPage = pagedList.FirstItemOnPage,
                HasNextPage = pagedList.HasNextPage,
                HasPreviousPage = pagedList.HasPreviousPage,
                IsFirstPage = pagedList.IsFirstPage,
                IsLastPage = pagedList.IsLastPage,
                LastItemOnPage = pagedList.LastItemOnPage,
                PageCount = pagedList.PageCount,
                PageNumber = pagedList.PageNumber,
                TotalItemCount = pagedList.TotalItemCount
            };

            return Ok(result);
        }

        /// <summary>
        /// get all user's items for filter
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("all/countOfItems", Name = "get all user's items for filter")]
        public async Task<ActionResult<CountOfAds>> Get()
        {
            var user = await _unitOfWork.UserRepository.GetAsync(User.GetUserEmail());
            if (user == null)
            {
                return NotFound();
            }

            var result = _unitOfWork.AdVehicleRepository.GetCountOfAdsObject(user.Id);

            return Ok(result);
        }

        /// <summary>
        /// get count of categories by filter
        /// </summary>
        /// <param name="sorter"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("all/countOfCategories", Name = "get count of categories by filter")]
        public async Task<ActionResult<object>> GetCountOfCategories(RoomSorter sorter = RoomSorter.All)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(User.GetUserEmail());
            if (user == null)
            {
                return NotFound();
            }

            var result = _unitOfWork.AdVehicleRepository.GetCountOfCategories(user.Id, sorter);

            return Ok(result);
        }

        /// <summary>
        /// Edit the vehicle item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("item/edit")]
        public async Task<ActionResult<AdVehicle>> Post(VehicleDto model)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(User.GetUserEmail());
            if (user == null)
            {
                return NotFound();
            }

            AdVehicle adVehicle = await _unitOfWork.AdVehicleRepository.GetAsync(model.ID);
            if (adVehicle == null)
            {
                return NotFound();
            }

            if (user.Id == adVehicle.UserID)
            {
                adVehicle.Description = model.Description;
                _unitOfWork.AdVehicleRepository.Update(adVehicle);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                return BadRequest("User.id not equal to adVehicle.UserId");
            }

            return Ok(adVehicle);
        }
    }
}
