using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DaringCore.Extensions
{
    public static class StringExtensions
    {
        static public string Base64Encode(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            return Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
        }

        static public string Base64Decode(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            return Encoding.ASCII.GetString(Convert.FromBase64String(value));
        }

        static public string AlphaNumericOnly(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            return String.Concat(value.Where(Char.IsLetterOrDigit));
        }

        public static string SplitCamelCase(this String input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }
    }
}
