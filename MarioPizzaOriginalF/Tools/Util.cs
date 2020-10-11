using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace MarioPizzaOriginal
{
    public static class Util
    {
        public static string ToSHA256String(string input)
        {
            if (input == null)
                throw new ArgumentException("Argument nie może być nullem!");

            var shaBytes = SHA256.Create().ComputeHash(input.ToUtf8Bytes());
            return ConvertSHAToString(shaBytes);
        }

        private static string ConvertSHAToString(byte[] array)
        {
            var builder = new StringBuilder();
            foreach (var t in array)
            {
                builder.Append($"{t:X2}");
            }

            return builder.ToString().ToLower();
        }
    }
}
