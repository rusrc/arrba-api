using System;
using System.Threading.Tasks;
using Arrba.Api.Jwt;
using Arrba.DTO;
using Arrba.Repositories;
using Arrba.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Arrba.Constants.CONSTANT;

namespace Arrba.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly JwtManager _jwtManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICaptchaService _captchaService;

        public TokenController(JwtManager jwtManager, ICaptchaService captchaService, IUnitOfWork unitOfWork)
        {
            this._jwtManager = jwtManager;
            this._captchaService = captchaService;
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// get token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("", Name = "get token")]
        [AllowAnonymous]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Post(UserDto user)
        {
            string token;
            if (await CheckUser(user.Email, user.Password) 
                && !string.IsNullOrEmpty(token = _jwtManager.GenerateToken(0, user.Email, user.Email)))
            {
                return Ok(token);
            }

            return NotFound();
        }

        /// <summary>
        /// get token with grecaptcha
        /// </summary>
        /// <param name="grecaptcha"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("grecaptcha", Name = "get token with grecaptcha")]
        [AllowAnonymous]
        [Produces("application/json")]
        public ActionResult<string> PostWithReCaptcha(GrecaptchaDto grecaptcha)
        {
            var role = GUEST;
            var userName = Guid.NewGuid().ToString("N");

            if (this._captchaService.Check(grecaptcha.grecaptchaToken))
            {
                return Ok(_jwtManager.GenerateToken(0, userName, userName, role));
            }

            return BadRequest();
        }

        private async Task<bool> CheckUser(string email, string password)
        {
            return await _unitOfWork.UserRepository.CheckUser(email, password);
        }
    }
}
