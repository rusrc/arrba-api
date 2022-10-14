using System;
using System.Text.RegularExpressions;
using Arrba.Extensions.Exceptions;

namespace Arrba.Extensions
{
    public static class StringExtention
    {
        /// <summary>
        ///     Split string like 100|200|500 and return result as double type
        /// </summary>
        /// <param name="Index">Index of element in array, the default is 0</param>
        /// <param name="splitSign">delimeter of sign, the defaul is '|'</param>
        /// <returns></returns>
        public static double ToDouble(this string str, int Index = 0, char splitSign = '|')
        {
            var pattern = string.Concat("\\d*\\", splitSign, "\\d*");
            if (!new Regex(pattern, RegexOptions.IgnoreCase).Match(str).Success)
            {
                throw new FormatException($@"Вы пытаетесь передать строку вида {str}. 
                                              Входящaя строка должна совпадать с форматом {pattern}");
            }

            var from = str.Split(splitSign);
            if (from[Index] == string.Empty)
                return 0;

            double numResult;
            if (double.TryParse(from[Index], out numResult))
                return numResult;

            throw new ExtentionsException("не удается сконвертировать, возможно стоит точка вместо запятой");
        }

        public static bool IsNumeric(this string str)
        {
            var result = new long();
            return long.TryParse(str, out result);
        }


        public static string Left(this string @this, int count)
        {
            return @this.Length <= count ? @this : @this.Substring(0, count);
        }

    }
}