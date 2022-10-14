using Microsoft.Extensions.Configuration;

namespace Arrba.Services.Configuration
{
    public class ApplicationConfiguration
    {
        private readonly IConfiguration _configuration;

        public ApplicationConfiguration(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string DbArrbaConnectionString => _configuration.GetConnectionString("DbArrba");
        // public string DbArrbaClusterConnectionString => _configuration.GetConnectionString("DbArrbaCluster");

        public string UiHost => _configuration.GetValue<string>(nameof(UiHost));

        public bool UseMemaryCaching => _configuration.GetValue<bool>(nameof(UseMemaryCaching));

        public int HotAdLimit => _configuration.GetValue<int>(nameof(HotAdLimit));

        public int CountRows => _configuration.GetValue<int>(nameof(CountRows));

        /// <summary>
        /// Default privary supercategory that dispalyed on index home page
        /// </summary>
        public long DefaultSuperCategory => _configuration.GetValue<long>(nameof(DefaultSuperCategory));
        /// <summary>
        /// Modiration status for vehicles active or not/>
        /// </summary>
        public bool AdVehicleModirationStatus => _configuration.GetValue<bool>(nameof(AdVehicleModirationStatus));
        /// <summary>
        /// Default currency id that matched with currency id in database 
        /// </summary>
        public long DefaultCurrencyId => _configuration.GetValue<long>(nameof(DefaultCurrencyId));
        /// <summary>
        /// Default country Id
        /// 1 - Russia, 2 - Kazahstan
        /// </summary>
        public long DefaultCountryId => _configuration.GetValue<long>(nameof(DefaultCountryId));
        /// <summary>
        /// The days count advertisement life
        /// </summary>
        public int AdvertLifeDays => _configuration.GetValue<int>(nameof(AdvertLifeDays));

        public double PromotionPrice => _configuration.GetValue<double>(nameof(PromotionPrice));

        /// <summary>
        /// ~/App_Data/filename.png
        /// </summary>
        public string NoImageName => _configuration.GetValue<string>(nameof(NoImageName));
        public string LogoWaterMarkFile => _configuration.GetValue<string>(nameof(LogoWaterMarkFile));
        public string BackgroundImage => _configuration.GetValue<string>(nameof(BackgroundImage));

        /// <summary>
        /// "arrba-142507.appspot.com"
        /// </summary>
        public string FirebaseStorageEndpoint => _configuration.GetValue<string>(nameof(FirebaseStorageEndpoint));

        #region TODO mail notification unused yet
        public string MailInfo => _configuration.GetValue<string>(nameof(MailInfo));  // "info@arrba.ru";
        /// <summary>
        /// Display the name of email when comming to client, by default 'arrba'
        /// </summary>
        public string MailDisplayName => _configuration.GetValue<string>(nameof(MailDisplayName));  //"arrba";
        /// <summary>
        /// FROM = "info@arrba.ru";
        /// </summary>
        public string MailFrom => _configuration.GetValue<string>(nameof(MailFrom));  //"info@arrba.ru";
        /// <summary>
        /// PW = "gCpBCLK00N3mCXxdJ9qC";
        /// </summary>
        public string MailPassword => _configuration.GetValue<string>(nameof(MailPassword));  //"gCpBCLK00N3mCXxdJ9qC"; 
        #endregion
    }
}
