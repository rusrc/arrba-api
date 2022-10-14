using System.Threading;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Repositories;
using Arrba.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Arrba.Api.Controllers
{
    public class DiagnosticController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;

        public DiagnosticController(IUnitOfWork unitOfWork, ILogService logService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _logService = logService;
            _configuration = configuration;
        }

        /// <summary>
        /// Ping
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ping", Name = "ping service")]
        public async Task<ActionResult<string>> Ping()
        {
            var result = await Task.Run(() =>
            {
                ThreadPool.GetAvailableThreads(
                    out var workerThreadsCount,
                    out var completionPortThreadsCount);

                _logService.Info($"WorkerThreads: {workerThreadsCount}; CompletionPortThreads: {completionPortThreadsCount}");

                return "Pong";
            });

            return Ok(result);
        }

        /// <summary>
        /// Load call 1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LoadCall1", Name = "Load call 1")]
        public async Task<ActionResult<string>> LoadCall1() => Ok(await Task.Run(() => "Load 1"));

        /// <summary>
        /// Load call 1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LoadCall2", Name = "Load call 2")]
        public async Task<ActionResult<string>> LoadCall2() => Ok(await Task.Run(() => "Load 2"));

        /// <summary>
        /// Load call 1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LoadCall3", Name = "Load call 3")]
        public async Task<ActionResult<string>> LoadCall3() => Ok(await Task.Run(() => "Load 3"));

        /// <summary>
        /// Simulate the situation with exhausted connections
        /// `The connection pool has been exhausted`
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("NotClosedConnectionCall", Name = "Not closed connection call")]
        public async Task<ActionResult<string>> NotClosedConnection()
        {
            const long saintPetersburg = 569L;

            using (var timer = new MultiPointTimer("Not closed connection call", _logService))
            {
                var city = await _unitOfWork.CityRepository.GetAsync(saintPetersburg);

                Task.Run(() =>
                {
                    city.Weight++;
                    _unitOfWork.CompleteAsync();
                });

                return Ok(await Task.Run(() => $"Finished, city: {city.Alias}, weight: {city.Weight}"));
            }
        }
    }
}
