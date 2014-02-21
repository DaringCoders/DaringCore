using System;
using System.Text;

namespace DaringCore.Extensions
{
    public static class StringExtensions
    {
        static public string EncodeTo64(this string value)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
        }

        static public string DecodeFrom64(this string value)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(value)); ;
        }
    }
}
