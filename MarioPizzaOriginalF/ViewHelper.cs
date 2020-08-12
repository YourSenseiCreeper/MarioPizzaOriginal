using MarioPizzaOriginal.Controller;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MarioPizzaOriginal
{
    public static class ViewHelper
    {
        public static int AskForInt(string message, int? min = null, int? max = null, int? current = null, bool inline = true, bool clear=true)
        {
            string answer;
            bool answerOk;
            int result = 0;
            do
            {
                answerOk = false;
                var formatedMessage = current != null ? $"{message.Replace(":", "")} ({current}): " : message;
                if (clear) Console.Clear();
                if (inline) Console.Write(formatedMessage);
                else Console.WriteLine(formatedMessage);
                answer = Console.ReadLine();
                // Check if answer is Int, then 
                // if min is not null check wheater Int is above and 
                // if max is not null check if Int is below max
                if (int.TryParse(answer, out int innerResult))
                {
                    if (min != null && innerResult <= min)
                    {
                        WriteAndWait($"'{answer}' nie może być mniejsza niż {min}!");
                        continue;
                    }
                    if (max != null && innerResult >= max)
                    {
                        WriteAndWait($"'{answer}' nie może być większa od {max}!");
                        continue;
                    }
                    result = innerResult;
                    answerOk = true;
                }
                else WriteAndWait($"'{answer}' nie jest liczbą!");
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
                answer = Console.ReadLine().Replace(".", ",");
                if (double.TryParse(answer, out double innerResult))
                {
                    answerOk = true; 
                    result = innerResult;
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

        public static string AskForString(string mes, object[] args) 
        {
            int argIndex = 0;
            string message = Resource.ResourceManager.GetString(mes); 
            //Resource.FoodController_NewFood_step1
            
            foreach (object arg in args)
            {
                message = message.Replace($"{argIndex}", arg.ToString());
            }
            return AskForString(message);
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
        /// Method asking for password replacing writed letters with asterisks. By CraigTP
        /// </summary>
        public static string AskForPassword(string message, object[] args)
        {
            Console.Write(message);
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass.Substring(0, pass.Length - 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            return pass;
        }

        public static string ConvertSHAToString(byte[] array)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                builder.Append($"{array[i]:X2}");
                if (i % 4 == 3) builder.Append(" ");
            }

            return builder.ToString();
        }

        public static string AskForStringNotBlank(string message, object[] args)
        {
            int argIndex = 0;
            //Resource.FoodController_NewFood_step1
            foreach (object arg in args)
            {
                message = message.Replace($"{argIndex}", arg.ToString());
            }
            return AskForStringNotBlank(message);
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
            return UnsafeAskForOption<T>(messageAllElements, messageNewValue, inline);
        }

        public static T UnsafeAskForOption<T>(string messageAllElements, string messageNewValue, bool inline = true)
        {
            string answer;
            T result = default;
            bool answerOk = false;
            do
            {
                Console.Clear();
                int index = 0;

                Console.WriteLine(messageAllElements);
                Enum.GetNames(typeof(T)).ToList().ForEach(element => Console.WriteLine($"{index++}. {element}"));
                Console.Write(messageNewValue);
                answer = Console.ReadLine();

                try
                {
                    //If currentValue is not Null or Empty parse currentValue otherwise parse answer
                    //if (string.IsNullOrEmpty(answer)) answer = currentValue;
                    result = (T) Enum.Parse(typeof(T), answer.ToUpper());
                    answerOk = true;
                }
                catch (ArgumentException) { WriteAndWait($"'{answer}' nie jest jedną z możliwych wartości!"); }
            } while (!answerOk);
            return result;
        }

        public static bool AskForYesNo(string question)
        {
            Console.Clear();
            Console.Write($"{question} (y/n): ");
            var options = new List<string> { "yes", "y", "tak", "t" };
            string answer = Console.ReadLine();
            return options.Contains(answer.ToLower().Trim());
        }
        
        public static void WriteAndWait(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        public static object FilterDouble(string message, object[] args)
        {
            double? input = AskForDouble(message);
            return input == -1 ? null : input;
        }

        public static object FilterInt(string message, object current, object[] args)
        {
            int? input = args?[0] as int? == -1 ? 
                AskForInt(message, min: -1, current: (int?) current) : 
                AskForInt(message, current: (int?)current);
            return input == -1 ? null : input;
        }

        public static object FilterString(string message, object[] args)
        {
            string input = AskForString(message);
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
                
                if (string.IsNullOrEmpty(answer)) WriteAndWait("Data nie może być pusta! Jeżeli chcesz wyjść wpisz -1");
                if (answer == "-1") return null;
                try
                {
                    result = DateTime.ParseExact(answer, "g", new CultureInfo("fr-FR"));
                    answerOk = true;
                } catch (FormatException)
                {
                    WriteAndWait($"'{answer}' zły format daty! Przykład: 01/01/2000 06:15");
                }
            } while (!answerOk);
            return result;
        }
        public static T FilterOption<T>(string message, object[] args) where T : Enum => UnsafeAskForOption<T>(message, "");
    }
}
