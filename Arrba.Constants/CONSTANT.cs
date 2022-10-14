namespace Arrba.Constants
{
    public static partial class CONSTANT
    {
        /// <summary>
        /// Арба́ (от перс. ارابه [arābe] = тадж. аррба, через тюрк. арба, каз. арба; узб. arava) — высокая двухколёсная повозка (в Средней Азии)
        /// 
        /// Шуточное Шайтан-Арба:
        /// Первые упоминания об этом необычном транспортном средстве так и не дошли до нас из древнего Китая, 
        /// где данное транспортное средство применялось с XYV̆\ века до нашей эры. Из-за отсутствия промышленного 
        /// производства (потому что ещё не изобрели) пердучего газа его заставляли производить рабов.
        /// Шайтан-арба в роли всепогодного ледокола «Лёнин»
        /// В дальнейшем технологии шайтан-арбы широко применялись в конструкции таких транспортных средств, 
        /// как маршрутка, хачмобиль, пепелац и многих других.В частности, на основе корпуса Ш-А.был выпущен первый 
        /// пассажирский всепогодный ледокол «Лёнин», эксплуатировавшийся на трассах Ямало-Немецкого АО 
        /// в период 1950—1980 г.г.до окончания в этой местности ледникового периода.
        /// </summary>
        public const string PROJECT_NAME = "ARRBA";

        public const string ROOT_HOST_HTTPS = "https://api.arrba.ru";
        public const string UNIX_HOST = "/tmp/api.arrba.kestrel.sock";

        public const string MANAGER = "Manager";
        public const string GUEST = "Guest";

        /// <summary>
        /// Categ folder path
        /// </summary>
        public const string CATEG_FOLDER_PATH = @"content/Imgs/categs/new/";

        //ImgManager namespace
        public const int DEFAULT_CONTAINER_WIDTH = 400;
        public const int DEFAULT_CONTAINER_HEIGHT = 300;
        //ImgManagerExtension
        /// <summary>
        /// Full file size 
        /// </summary>
        public const string BIG_FILE_NAME_PREFIX = "1920x1080";
        public const string FULL_FILE_NAME_PREFIX = "1024x720";
        public const string MIDDLE_FILE_NAME_PREFIX = "400x300";
        public const string MIDDLE_FILE_NAME_PREFIX2 = "540x405";
        public const string SMALL_FILE_NAME_PREFIX = "200x150";
        public const string SMALL_MIDDLE_FILE_NAME_PREFIX = "120x90";
        public const string SUPER_SMALL_FILE_NAME_PREFIX = "60x45";

        public const string REGEXP_PHONE_NUMBER = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";

        public const string SECRET_KEY = "6LcBTBsTAAAAAEHWw3vR5SYrcq-qj0nYt2xgxjNd";
        /// <summary>
        /// Url запроса на проверку
        /// </summary>
        public const string GRECAPTCHA_URL = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";

        /// <summary>
        /// Ссылка на спецификацию https://www.w3.org/TR/html5/forms.html#valid-e-mail-address
        /// </summary>
        public const string EMAIL_REGEXPRESSION = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        public const string FIREBASE_URL = "https://firebasestorage.googleapis.com/v0/b/arrba-142507.appspot.com/o/images%2F{0}?alt=media";
    }
}