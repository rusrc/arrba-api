using System;
using Arrba.GoogleCaptchaV2;
using Arrba.Services.Logger;

namespace Arrba.Services
{
    public class GrecaptchaService : ICaptchaService
    {
        private readonly ILogService _logService;
        public GrecaptchaService(ILogService logService)
        {
            this._logService = logService;
        }

        public bool Check(string greCaptchaResponse)
        {
            var grecaptcha = new Grecaptcha().GetResult(greCaptchaResponse);

            if (grecaptcha.IsError)
            {
                this._logService.Error(grecaptcha.ErrorMsg);
                throw new ApplicationException(grecaptcha.ErrorMsg);
            }

            return grecaptcha.Success;
        }
    }
}
