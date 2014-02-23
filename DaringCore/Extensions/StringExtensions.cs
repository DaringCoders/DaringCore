using System;
using System.Linq;
using System.Text;

namespace DaringCore.Extensions
{
    public static class StringExtensions
    {
        static public string Base64Encode(this string value)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
        }

        static public string Base64Decode(this string value)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(value)); ;
        }

        static public string AlphaNumericOnly(this string value)
        {
            return String.Concat(value.Where(Char.IsLetterOrDigit));
        }
    }
}
