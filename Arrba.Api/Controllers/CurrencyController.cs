using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.Repositories;
using Arrba.Services.Logger;
using log4net.Core;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    /// <summary>
    /// Currency
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _logService;
        
        #pragma warning disable CS1591
        public CurrencyController(IUnitOfWork unitOfWork, ILogService logService)
        {
            _unitOfWork = unitOfWork;
            _logService = logService;
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all", Name = "Get all currencies")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetAllCurrencies()
        {
            using (new MultiPointTimer("call: get all currencies", _logService))
            {
                var currencies = await _unitOfWork.CurrencyRepository.GetAllAsync();

                if (currencies.Any() is false)
                {
                    return NotFound();
                }

                return Ok(currencies);
            }
        }

        /// <summary>
        /// Get exchange rates
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ExchangeRate", Name = "get exchange rates")]
        public async Task<ActionResult<IEnumerable<ExchangeRate>>> GetExchangeRate()
        {
            var exchageRate = await _unitOfWork.CurrencyRepository.GetExchangeRatesAsync();

            if (exchageRate == null)
            {
                return NotFound();
            }

            return Ok(exchageRate);
        }
    }
}
