using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioPizzaOriginal
{
    public class FilterHelper
    {
        public static object FilterDouble(string message, object[] args)
        {
            double? input = ViewHelper.AskForDouble(message);
            return input == -1 ? null : input;
        }

        public static object FilterInt(string message, object current, object[] args)
        {
            int? input = args?[0] as int? == -1 ?
                ViewHelper.AskForInt(message, min: -1, current: (int?)current) :
                ViewHelper.AskForInt(message, current: (int?)current);
            return input == -1 ? null : input;
        }

        public static object FilterString(string message, object[] args)
        {
            string input = ViewHelper.AskForString(message);
            return input != "-1" ? input : null;
        }

        public static object FilterDateTime(string message, object[] args)
        {
            string answer;
            bool answerOk = false;
            DateTime result = new DateTime();
            do
            {
                Console.Clear();
                Console.WriteLine(message);
                answer = Console.ReadLine();

                if (string.IsNullOrEmpty(answer)) ViewHelper.WriteAndWait("Data nie może być pusta! Jeżeli chcesz wyjść wpisz -1");
                if (answer == "-1") return null;
                try
                {
                    result = DateTime.ParseExact(answer, "g", new CultureInfo("fr-FR"));
                    answerOk = true;
                }
                catch (FormatException)
                {
                    ViewHelper.WriteAndWait($"'{answer}' zły format daty! Przykład: 01/01/2000 06:15");
                }
            } while (!answerOk);
            return result;
        }
        public static T FilterOption<T>(string message, object[] args) where T : Enum => ViewHelper.UnsafeAskForOption<T>(message, "");
    }
}
