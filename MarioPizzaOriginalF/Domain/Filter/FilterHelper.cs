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
            var answerOk = false;
            double result = 0;
            do
            {
                answerOk = false;
                Console.Clear();
                Console.Write(message);
                var currentValue = args?[0] != null ? args[0].ToString() : string.Empty;
                var answer = ViewHelper.EditableValue(currentValue);
                if (string.IsNullOrEmpty(answer)) return null;
                if (double.TryParse(answer, out var innerResult))
                {
                    answerOk = true;
                    result = innerResult;
                }
                else ViewHelper.WriteAndWait($"'{answer}' nie jest liczbą!");
            } while (!answerOk);
            return result;
        }

        public static object FilterInt(string message, object[] args)
        {
            bool answerOk;
            var result = 0;
            do
            {
                answerOk = false;
                Console.Write(message);
                var currentValue = args?[0] != null ? args[0].ToString() : string.Empty;
                var answer = ViewHelper.EditableValue(currentValue);
                if (string.IsNullOrEmpty(answer)) return null;
                if (int.TryParse(answer, out var innerResult))
                {
                    result = innerResult;
                    answerOk = true;
                }
                else ViewHelper.WriteAndWait($"'{answer}' nie jest liczbą!");
            } while (!answerOk);

            return result;
        }

        public static object FilterString(string message, object[] args)
        {
            Console.Clear();
            Console.Write(message);
            var currentValue = args?[0] != null ? args[0].ToString() : string.Empty;
            var answer = ViewHelper.EditableValue(currentValue);
            return string.IsNullOrEmpty(answer) ? null : answer;
        }

        public static object FilterDateTime(string message, object[] args)
        {
            bool answerOk = false;
            DateTime result = new DateTime();
            do
            {
                Console.Clear();
                Console.Write(message);
                var currentValue = string.Empty;
                if (args?[0] != null)
                {
                    var argument = (DateTime) args[0];
                    currentValue = argument.ToShortDateString();
                }
                var answer = ViewHelper.EditableValue(currentValue);
                if (string.IsNullOrEmpty(answer)) return null;
                try
                {
                    //result = DateTime.ParseExact(answer, "g", new CultureInfo("fr-FR"));
                    result = DateTime.Parse(answer);
                    answerOk = true;
                }
                catch (FormatException)
                {
                    ViewHelper.WriteAndWait($"'{answer}' zły format daty! Przykład: 01/01/2000 06:15");
                }
            } while (!answerOk);
            return result;
        }

        public static object FilterOption<T>(string message, object[] args) where T : Enum
        {
            T result = default;
            bool answerOk = false;
            do
            {
                Console.Clear();
                var index = 0;

                Console.WriteLine("Dostępne opcje: ");
                Enum.GetNames(typeof(T)).ToList().ForEach(element => Console.WriteLine($"{index++}. {element}"));
                Console.Write(message);
                var currentValue = string.Empty;
                if (args?[0] != null)
                {
                    var argument = (int) args[0];
                    currentValue = argument.ToString();
                }
                var answer = ViewHelper.EditableValue(currentValue);
                if (string.IsNullOrEmpty(answer)) return null;
                try
                {
                    //If currentValue is not Null or Empty parse currentValue otherwise parse answer
                    //if (string.IsNullOrEmpty(answer)) answer = currentValue;
                    result = (T)Enum.Parse(typeof(T), answer.ToUpper());
                    answerOk = true;
                }
                catch (ArgumentException) { ViewHelper.WriteAndWait($"'{answer}' nie jest jedną z możliwych wartości!"); }
            } while (!answerOk);
            return result;
        }
    }
}
