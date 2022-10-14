using System;
using System.IO;
using static System.Convert;


namespace Arrba.Constants
{
    public static partial class SETTINGS
    {
        public static DbArrbaContextType DataBaseType => (DbArrbaContextType)Enum.Parse(typeof(DbArrbaContextType), "DataBaseType" ?? nameof(DbArrbaContextType.DbArrbaAzure));
        public static int HotAdLimit => 27;

        public static int CountRows => 20;
        /// <summary>
        /// INFO_MAIL = "info@arrba.ru"
        /// </summary>
        public static string MailInfo => "info@arrba.ru";
        /// <summary>
        /// Display the name of email when comming to client, by default 'arrba'
        /// </summary>
        public static string MailDisplayName => "arrba";
        /// <summary>
        /// FROM = "info@arrba.ru";
        /// </summary>
        public static string MailFrom => "info@arrba.ru";
        /// <summary>
        /// PW = "gCpBCLK00N3mCXxdJ9qC";
        /// </summary>
        public static string MailPassword => "gCpBCLK00N3mCXxdJ9qC";
        /// <summary>
        /// Default privary supercategory that dispalyed on index home page
        /// </summary>
        public static long DefaultSuperCategory => 7;
        /// <summary>
        /// Modiration status for vehicles active or not/>
        /// </summary>
        public static bool AdVehicleModirationStatus => false;
        /// <summary>
        /// Default currency id that matched with currency id in database 
        /// </summary>
        public static long DefaultCurrencyID => 4;
        /// <summary>
        /// Default country Id
        /// 1 - Russia, 2 - Kazahstan
        /// </summary>
        public static long DefaultCountryId => 1;
        /// <summary>
        /// The days count advertisement life
        /// </summary>
        public static long AdvertLifeDays => 60;

        public static bool UseMemaryCaching => false;

        public static double PromotionPrice => 0;
        
        /// <summary>
        /// ~/App_Data/filename.png
        /// </summary>
        public static string NoImageName => "no_img1.jpg";
        public static string LogoWaterMarkFile => "logo_watermark_dark_big3.png";
        public static string BackgroundImage => "background1.png";

        public enum DbArrbaContextType
        {
            DbArrba,
            DbArrbaAzure
        }
    } 
}

//const string INFO_MAIL = "info@arrba.ru";
//const string DISPLAY_NAME = "arrba.ru";
//const string FROM = "info@arrba.ru";
//const string TO = "ruslan878707@gmail.com";
//const string USER_NAME = "info@arrba.ru";
//const string PW = "gCpBCLK00N3mCXxdJ9qC";