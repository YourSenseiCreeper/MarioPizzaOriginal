using System;
using System.Collections.Generic;
using System.Linq;
using Pastel;

namespace MarioPizzaOriginal
{
    public static class Color
    {
        public const string NavyBlue = "#6891C3";
        public const string Blue = "#AAC5E2";
        public const string PiggyPink = "#F692BC";
        public const string Basic = "#FFFFFF";

        public static string Format(string message, (object data, string color)[] colorTextDictionary)
        {
            var arguments = new object[colorTextDictionary.Length];
            for (var i = 0; i < arguments.Length; i++)
            {
                arguments[i] = colorTextDictionary[i].data.ToString().Pastel(colorTextDictionary[i].color);
            }
            // var coloredPieces = colorTextDictionary.Select(entry => entry.data.ToString().Pastel(entry.color));
            return string.Format(message, arguments);
        }
    }
}
