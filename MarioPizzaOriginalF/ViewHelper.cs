using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal
{
    public static class ViewHelper
    {
        public static int AskForInt(string message, int? min = null, int? max = null, bool inline = true, bool clear = true)
        {
            string answer;
            bool answerOk;
            int result = 0;
            do
            {
                answerOk = true;
                if (clear) Console.Clear();
                if (inline) Console.Write(message);
                else Console.WriteLine(message);
                answer = Console.ReadLine();
                // Check if answer is Int, then 
                // if min is not null check wheater Int is above and 
                // if max is not null check if Int is below max
                if (int.TryParse(answer, out int innerResult))
                {
                    if (min != null && innerResult <= min)
                    {
                        WriteAndWait($"{answer} nie może być mniejsza niż {min}!");
                        answerOk = false;
                    }
                    if (max != null && innerResult >= max)
                    {
                        WriteAndWait($"{answer} nie może być większa od {max}!");
                        answerOk = false;
                    }
                    result = innerResult;
                }
                else WriteAndWait($"{answer} nie jest liczbą!");
            } while (!answerOk);
            return result;
        }

        public static double AskForDouble(string message, bool inline = true, bool clear = true)
        {
            string answer;
            bool answerOk = false;
            double result = 0;
            do
            {
                if (clear) Console.Clear();
                if (inline) Console.Write(message);
                else Console.WriteLine(message);
                answer = Console.ReadLine();
                if (double.TryParse(answer, out double innerResult))
                {
                    answerOk = true; result = innerResult;
                }
                else WriteAndWait($"{answer} nie jest liczbą!");
            } while (!answerOk);
            return result;
        }

        public static string AskForString(string message, bool inline = true, bool clear = true)
        {
            if (clear) Console.Clear();
            if (inline) Console.Write(message);
            else Console.WriteLine(message);
            return Console.ReadLine();
        }

        public static string AskForStringNotBlank(string message, bool inline = true, bool clear = true)
        {
            string answer;
            bool answerOk = false;
            do
            {
                if (clear) Console.Clear();
                if (inline) Console.Write(message);
                else Console.WriteLine(message);
                answer = Console.ReadLine();
                if (!string.IsNullOrEmpty(answer)) answerOk = true;
                else { WriteAndWait("Nazwa nie może być pusta!"); }
            } while (!answerOk);
            return answer;
        }

        /// <summary>
        /// Allows to pick a value from Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageAllElements"></param>
        /// <param name="messageNewValue"></param>
        /// <param name="currentValue"></param>
        /// <param name="inline">Answer in the same line as question?</param>
        /// <returns></returns>
        public static T AskForOption<T>(string messageAllElements, string messageNewValue, string currentValue = "", 
            bool inline = true) where T : Enum
        {
            string answer;
            T result = default;
            bool answerOk = false;
            do
            {
                Console.Clear();
                var allElements = new List<string>(Enum.GetNames(typeof(T)));
                int index = 0;

                Console.WriteLine($"{messageAllElements} ");
                allElements.ForEach(element => Console.WriteLine($"{index++}. {element}"));

                string message = string.IsNullOrEmpty(currentValue) ? messageNewValue : $"{messageNewValue} ({currentValue})";
                if (inline) Console.Write(message);
                else Console.WriteLine(message);
                answer = Console.ReadLine();

                try
                {
                    //If currentValue is not Null or Empty parse currentValue otherwise parse answer
                    if (string.IsNullOrEmpty(answer)) answer = currentValue;
                    result = (T)Enum.Parse(typeof(T), answer.ToUpper());
                    answerOk = true;
                }
                catch (ArgumentException) { WriteAndWait($"{answer} nie jest jedną z możliwych wartości!"); }
            } while (!answerOk);
            return result;
        }

        public static bool AskForYesNo(string question)
        {
            Console.Clear();
            Console.Write($"{question} (y/n): ");
            var options = new List<string> { "yes", "y", "tak", "t" };
            string answer = Console.ReadLine();
            return options.Contains(answer.ToLower());
        }
        
        public static void WriteAndWait(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        public static double? FilterDouble(string message)
        {
            double? input = AskForDouble(message);
            return input == -1 ? null : input;
        }

        public static int? FilterInt(string message, bool m1 = false)
        {
            int? input = m1 ? AskForInt(message, min: -1) : AskForInt(message);
            return input == -1 ? null : input;
        }

        public static string FilterString(string message)
        {
            string input = AskForString(message);
            return input != "-1" ? input : null;
        }

        public static DateTime FilterDateTime(string message, bool isMin)
        {
            string input = AskForString(message);
            return input != "-1" ? Convert.ToDateTime(input) : (isMin ? DateTime.MinValue : DateTime.MaxValue);
        }

        public static T FilterOption<T>(string message) where T : Enum
        {
            return AskForOption<T>(message, "");
        }
    }
}
