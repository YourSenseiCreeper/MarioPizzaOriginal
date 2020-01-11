using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal
{
    public static class ViewHelper
    {
        public static int AskForInt(string message, bool inline = true, bool clear = true)
        {
            bool answerOk = false;
            int result = 0;
            string answer; 
            while (!answerOk)
            {
                if (clear) Console.Clear();
                if (inline) Console.Write(message);
                else Console.WriteLine(message);
                answer = Console.ReadLine();
                try { result = Convert.ToInt32(answer); answerOk = true; }
                catch (FormatException)
                { 
                    WriteAndWait($"{answer} nie jest liczbą całkowitą!");
                }
            }
            return result; 
        }

        public static double AskForDouble(string message, bool inline = true, bool clear = true)
        {
            bool answerOk = false;
            int result = 0;
            string answer;
            while (!answerOk)
            {
                if (clear) Console.Clear();
                if (inline) Console.Write(message);
                else Console.WriteLine(message);
                answer = Console.ReadLine();
                try { result = Convert.ToInt32(answer); answerOk = true; }
                catch (FormatException)
                {
                    WriteAndWait($"{answer} nie jest liczbą!");
                }
            }
            return result;
        }

        public static string AskForString(string message, bool inline = true, bool clear = true)
        {
            if (clear) Console.Clear();
            if (inline) Console.Write(message);
            else Console.WriteLine(message);
            return Console.ReadLine();
        }

        public static string AskForStringNotBlank(string message, bool inline = true)
        {
            string answer = "";
            while (string.IsNullOrEmpty(answer))
            {
                Console.Clear();
                if (inline) Console.Write(message);
                else Console.WriteLine(message);
                answer = Console.ReadLine();
                if (string.IsNullOrEmpty(answer)) { WriteAndWait("Nazwa nie może być pusta!"); }
            }
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
            bool answerOk = false;
            string answer;
            T result = default;
            while (!answerOk)
            {
                Console.Clear();
                var allElements = new List<string>(Enum.GetNames(typeof(T)));
                int index = 0;

                Console.WriteLine($"{messageAllElements} ");
                allElements.ForEach(element => Console.WriteLine($"{index++}. {element}"));

                if (inline) Console.Write(messageNewValue);
                else Console.WriteLine(messageNewValue);
                answer = Console.ReadLine();
                try
                {
                    //If currentValue is not Null or Empty parse currentValue otherwise parse answer
                    if (string.IsNullOrEmpty(answer)) answer = currentValue;
                    result = (T) Enum.Parse(typeof(T), answer.ToUpper());
                    answerOk = true;
                }
                catch (ArgumentException){ WriteAndWait($"{answer} nie jest jedną z możliwych wartości!"); }
            }
            return result;
        }

        public static bool AskForYesNo(string question)
        {
            Console.Clear();
            Console.Write($"{question} (y/n): ");
            string answer = Console.ReadLine();
            return answer.ToLower().Equals("yes") ||
                answer.ToLower().Equals("y") ||
                answer.ToLower().Equals("tak") ||
                answer.ToLower().Equals("t");
        }
        
        public static void WriteAndWait(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        public static double? FilterDouble(string message)
        {
            var input = AskForDouble(message);
            double? result = null;
            if (Convert.ToInt32(input) != -1) result = Convert.ToDouble(input);
            return result;
        }

        public static int? FilterInt(string message, bool m1 = false)
        {
            bool answerOk = false;
            int? result = null;
            double input;
            while (!answerOk)
            {
                input = AskForDouble(message);
                result = Convert.ToInt32(input);
                if (result > 0) answerOk = true;
                else if (m1 && result == -1)
                {
                    answerOk = true;
                    return null;
                }
                else WriteAndWait("Wartość nie może być mniejsza niż -1!");
            }
            return result;
        }

        public static string FilterString(string message)
        {
            string input = AskForString(message);
            return input != "-1" ? input : null;
        }
    }
}
