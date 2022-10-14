using Newtonsoft.Json;
using System.Linq;
using System.Net;
using CONSTS = Arrba.Constants.CONSTANT;

namespace Arrba.GoogleCaptchaV2
{
    public class Grecaptcha
    {
        #region Properties
        private string _secret;
        public string Secret
        {
            get
            {
                if (string.IsNullOrEmpty(_secret))
                {
                    return CONSTS.SECRET_KEY;
                }
                return _secret;
            }
            set { _secret = value; }
        }

        private string _url;

        public string URL
        {
            get
            {
                if (string.IsNullOrEmpty(_url))
                {
                    return CONSTS.GRECAPTCHA_URL;
                }
                return _url;
            }
            set { _url = value; }
        }

        public bool Success { get; set; }
        public bool IsError { get; private set; }

        public string ErrorMsg { get; private set; }
        /// <summary>
        /// Объект с ответом
        /// </summary>
        private GrecaptchaResponse GResponse { get; set; }
        #endregion

        public Grecaptcha() { }

        /// <summary>
        /// Задайте ключ через конструктор
        /// </summary>
        /// <param name="SecretKey">Секретный ключ полученный от гугла</param>
        public Grecaptcha(string SecretKey)
        {
            this.Secret = SecretKey;
        }

        /// <summary>
        /// Задайте ключ и url через конструктор
        /// </summary>
        /// <param name="SecretKey">Секретный ключ полученный от гугла</param>
        /// <param name="Url">Url запроса на проверку</param>
        public Grecaptcha(string SecretKey, string Url)
        {
            this.Secret = SecretKey;
            this.URL = Url;
        }

        /// <summary>
        /// Совершает запрос на проверку по ссылке https://www.google.com/recaptcha/api/siteverify
        /// </summary>
        /// <param name="grecaptchaResponse">Хеш-код гуглкапчи полученный от клиента</param>
        /// <returns>Grecaptcha</returns>
        public Grecaptcha GetResult(string grecaptchaResponse)
        {
            GrecaptchaResponse jsonResult = null;
            using (var client = new WebClient())
            {
                jsonResult = JsonConvert.DeserializeObject<GrecaptchaResponse>
                    (
                        value: client.DownloadString(string.Format(this.URL, this.Secret, grecaptchaResponse))
                    );
            }

            if (!jsonResult.Success && jsonResult.ErrorCodes.Count > 0)
            {
                this.Success = false;
                this.ErrorMsg = jsonResult.ErrorCodes.Aggregate((a, b) => a + b);
                this.IsError = true;
            }
            else
            {
                this.GResponse = jsonResult;
                this.Success = jsonResult.Success;
                this.IsError = false;
            }

            return this;
        }

        #region Helper
        private string GetError(string ErrorName)
        {
            var msg = string.Empty;

            switch (ErrorName)
            {
                case ("missing-input-secret"):
                    msg = "";//ResxCabinet.gRecaptchaMissingInputSecret;
                    break;
                case ("invalid-input-secret"):
                    msg = ""; //ResxCabinet.gRecaptchaInvalidInputSecret;
                    break;
                case ("missing-input-response"):
                    msg = ""; //ResxCabinet.gRecaptchaMissingInputResponse;
                    break;
                case ("invalid-input-response"):
                    msg = ""; //ResxCabinet.gRecaptchaInvalidInputResponse;
                    break;
                default:
                    msg = ""; //ResxCabinet.gRecaptchaDefault;
                    break;
            }

            return msg;

        }
        #endregion
    }
}