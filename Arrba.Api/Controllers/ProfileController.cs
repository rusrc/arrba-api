using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.DTO;
using Arrba.Extensions;
using Arrba.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProfileController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// get current profile of user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("user", Name = "get current profile of user")]
        public async Task<ActionResult<ProfileDto>> Get()
        {
            var user = await _unitOfWork.UserRepository
                .GetAsync(User.GetUserEmail(), loadPhones: true, loadBalance: true);

            if (user == null)
            {
                return NotFound();
            }

            var profile = this._mapper.Map<ProfileDto>(user);
            return Ok(profile);
        }

        /// <summary>
        /// get phone number by AdId
        /// </summary>
        /// <param name="adVehicleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("phone/{adVehicleId}", Name = "get phone number by AdId")]
        public async Task<ActionResult<PhoneDto>> Get(long adVehicleId)
        {
            var phone = await _unitOfWork.UserPhoneRepository.GetByVehicleIdAsync(adVehicleId);

            if (phone == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PhoneDto>(phone));
        }

        /// <summary>
        /// delete phone number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("phone/{number}/delete", Name = "delete phone number")]
        public async Task<ActionResult<PhoneDto>> Delete(string number)
        {

            var user = await _unitOfWork.UserRepository.GetAsync(User.GetUserEmail(), loadPhones: true);
            var phone = user?.UserPhones
                .SingleOrDefault(ph => ph.Number.Equals(number, StringComparison.CurrentCultureIgnoreCase));

            if (phone == null)
            {
                return NotFound();
            }

            _unitOfWork.UserPhoneRepository.Remove(phone);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        /// <summary>
        /// get phone numbers by AdId
        /// </summary>
        /// <param name="adVehicleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("phone/{adVehicleId}/all", Name = "get phone numbers by AdId")]
        public async Task<ActionResult<PhoneDto>> GetAll(long adVehicleId)
        {
            var phones = await _unitOfWork.UserPhoneRepository.GetAllAsync(adVehicleId);

            if (!phones.Any())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<UserPhone>, IEnumerable<PhoneDto>>(phones));
        }
    }
}
