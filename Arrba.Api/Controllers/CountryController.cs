using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    /// <summary>
    /// Country
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        #pragma warning disable CS1591
        public CountryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all", Name = "Get all countries")]
        public async Task<ActionResult<IEnumerable<Country>>> Get()
        {
            return Ok(await _unitOfWork.CountryRepository.GetAllAsync());
        }

        /// <summary>
        /// Get country by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}", Name = "Get country by Id")]
        public async Task<ActionResult<Country>> Get(long id)
        {
            return Ok(await _unitOfWork.CountryRepository.GetAsync(id));
        }
    }
}
