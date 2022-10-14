using System;
using System.Threading.Tasks;
using Arrba.Api.Jwt;
using Arrba.Constants;
using Arrba.Domain.Models;
using Arrba.DTO;
using Arrba.Exceptions;
using Arrba.Repositories;
using Arrba.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arrba.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignUpController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICaptchaService _captchaService;
        private readonly JwtManager _jwtManager;

        public SignUpController(IUnitOfWork unitOfWork, ICaptchaService captchaService, JwtManager jwtManager)
        {
            this._unitOfWork = unitOfWork;
            this._captchaService = captchaService;
            this._jwtManager = jwtManager;
        }

        /// <summary>
        /// sign up
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Jwt token</returns>
        [HttpPost]
        [Route("", Name = "sign up")]
        [AllowAnonymous]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Post(SignUpDto model)
        {
            if (!this._captchaService.Check(model.GrecaptchaToken))
            {
                ModelState.AddModelError("", "неверная капча");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var now = DateTime.Now;
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                UserNickName = model.Email,
                LastLogin = now,
                RegistrationDate = now,
                UserStatus = UserAuthStatus.Authorized,
                Balance = new Balance
                {
                    Amount = SETTINGS.PromotionPrice,
                    LastAddDate = now
                }
            };

            if (await this._unitOfWork.UserRepository.Add(user, model.Password))
            {
                //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, _getSubjectAccount(), _getBodyAccount(callbackUrl));
            }

            var token = _jwtManager.GenerateToken(user.Id, user.UserNickName, user.Email);

            return Ok(token);
        }
    }
}
