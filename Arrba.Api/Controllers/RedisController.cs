using Arrba.Services;
using Arrba.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedisController : ControllerBase
    {
        private readonly ILogService _logService;

        public RedisController(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Check redis
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Ping", Name = "Ping")]
        public ActionResult<string> Ping()
        {
            this._logService.Info("Ping");
            return Ok(CacheService.Ping());
        }

        /// <summary>
        /// FlushDb
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FlushDb", Name = "FlushDb")]
        [AllowAnonymous]
        public ActionResult<string> Post([FromQuery] string password)
        {
            if (password == "123456Ru!")
                return Ok(CacheService.FlushDb());
            else
                return StatusCode(403);
        }
    }
}
