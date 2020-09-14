using System;
using System.Collections.Generic;
using System.Linq;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Domain.Enums;
using Pastel;
using DColor = System.Drawing.Color;

namespace MarioPizzaOriginal
{
    public static class ViewHelper
    {
        public static int AskForInt(string message, int? min = null, int? max = null, int? current = null, bool exit = true, bool clear=true)
        {
            string answer;
            bool answerOk;
            int result = 0;
            do
            {
                answerOk = false;
                var formatedMessage = current != null ? $"{message.Replace(":", "")} ({current}): " : message;
                if (clear) Console.Clear();
                Console.Write(formatedMessage);
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
            var answerOk = false;
            double result = 0;
            do
            {
                if (clear) Console.Clear();
                if (inline) Console.Write(message);
                else Console.WriteLine(message);
                answer = Console.ReadLine().Replace(".", ",");
                if (double.TryParse(answer, out var innerResult))
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
        public static string AskForPassword(string message)
        {
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.Cyan;
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
            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.White;
            return pass;
        }

        public static string EditableValue(string value)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(value);
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && value.Length > 0)
                {
                    Console.Write("\b \b");
                    value = value.Substring(0, value.Length - 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write(keyInfo.KeyChar);
                    value += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            Console.Write("\n");
            Console.ForegroundColor = ConsoleColor.White;
            return value;
        }


        public static T AskForOption<T>(string messageAllElements, string messageNewValue, string currentValue = "", 
            bool inline = true) where T : Enum
        {
            return UnsafeAskForOption<T>(messageAllElements, messageNewValue, inline);
        }

        public static T UnsafeAskForOption<T>(string messageAllElements, string messageNewValue, bool inline = true)
        {
            T result = default;
            bool answerOk = false;
            do
            {
                Console.Clear();
                int index = 0;

                Console.WriteLine(messageAllElements);
                Enum.GetNames(typeof(T)).ToList().ForEach(element => Console.WriteLine($"{index++}. {element}"));
                Console.Write(messageNewValue);
                var answer = Console.ReadLine();

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
            return options.Contains(answer?.ToLower().Trim());
        }
        
        public static void WriteAndWait(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        public static string StatusColor(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.WAITING: return status.ToString().Pastel(DColor.Gold);
                case OrderStatus.IN_PROGRESS: return status.ToString().Pastel(DColor.Green);
                case OrderStatus.DELIVERY: return status.ToString().Pastel(DColor.Blue);
                case OrderStatus.DONE: return status.ToString().Pastel(DColor.Gray);
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, "Nie ma takiego koloru!");
            }
        }
        public static bool CheckIfElementNotExists<T>(IRepository<T> repository, string message, string missing, out int elementId)
        {
            elementId = AskForInt(message);
            if (repository.Exists(elementId))
                return false;
            WriteAndWait(string.Format(missing, elementId));
            return true;
        }
    }
}
