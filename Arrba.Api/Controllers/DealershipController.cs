using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DealershipController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DealershipController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// get dealership by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}", Name = "get dealership by name")]
        public async Task<ActionResult<Dealership>> Get(string name)
        {
            var dealership = await _unitOfWork.DealershipRepository.GetAsync(d => d.Name == name);
            if (dealership == null)
            {
                return NotFound();
            }

            return Ok(dealership);
        }

        /// <summary>
        /// get dealership by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/id", Name = "get dealership by id")]
        public async Task<ActionResult<Dealership>> Get(long id)
        {
            var dealership = await _unitOfWork.DealershipRepository.GetAsync(d => d.Id == id);
            if (dealership == null)
            {
                return NotFound();
            }

            return Ok(dealership);
        }

        /// <summary>
        /// get active dealerships
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all", Name = "get active dealerships")]
        public async Task<ActionResult<IEnumerable<Dealership>>> Get()
        {
            var dealerships = await _unitOfWork.DealershipRepository.GetAllAsync();
            if (dealerships == null || !dealerships.Any())
            {
                return NotFound();
            }

            return Ok(dealerships);
        }
    }
}
