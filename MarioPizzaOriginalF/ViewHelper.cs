using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioPizzaOriginal
{
    public static class ViewHelper
    {
        public static int AskForInt(string message, bool inline = true, bool loop = false)
        {
            bool answerOk = false;
            int result = 0;
            string answer;
            while (!answerOk)
            {
                Console.Clear();
                if (inline) Console.Write(message);
                else Console.WriteLine(message);
                answer = Console.ReadLine();
                try { result = Convert.ToInt32(answer); answerOk = true; }
                catch (FormatException)
                { 
                    Console.WriteLine($"{answer} nie jest liczbą całkowitą!");
                    Console.ReadLine();
                }
            }
            return result; 
        }

        public static string AskForString(string message, bool inline = true)
        {
            if (inline) Console.Write(message);
            else Console.WriteLine(message);
            return Console.ReadLine();
        }

        public static string AskForStringNotBlank(string message, bool inline = true)
        {
            string answer = "";
            //XOR xd (!AB + A!B)
            while (string.IsNullOrEmpty(answer))
            {
                if (inline) Console.Write(message);
                else Console.WriteLine(message);
                answer = Console.ReadLine();
                if (string.IsNullOrEmpty(answer)) { Console.WriteLine("Nazwa nie może być pusta!"); }
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
        /// <param name="currentWhenBlank">Accepts blank answer and uses currentValue instead</param>
        /// <returns></returns>
        public static T AskForOption<T>(string messageAllElements, string messageNewValue, string currentValue = "", 
            bool inline = true) where T : Enum
        {
            bool answerOk = false;
            string answer;
            T result = (T) Enum.GetValues(typeof(T)).GetValue(0);
            while (!answerOk)
            {
                var allElements = Enum.GetNames(typeof(T));
                int index = 0;
                Console.WriteLine($"{messageAllElements} ");
                foreach(string element in allElements)
                {
                    Console.WriteLine($"{index++}. {element}");
                }
                if (inline) Console.Write(messageNewValue);
                else Console.WriteLine(messageNewValue);
                answer = Console.ReadLine();
                try
                {
                    result = (T) Enum.Parse(typeof(T), string.IsNullOrEmpty(currentValue) ? currentValue.ToUpper() : answer.ToUpper());
                    answerOk = true;
                    return result;
                }
                catch (ArgumentException){ Console.WriteLine($"{answer} nie jest jedną z możliwych wartości!"); }
            }
            return result;
        }

        public static bool Exists<T, U>(T repository, int id, string whatNotExist) where T : Model.DataAccess.IRepository<U>
        {
            U element = repository.Get(id);
            bool exist = false;
            if (element == null)
            {
                Console.WriteLine($"{whatNotExist} o id = {id} nie istnieje!");
                exist = true;
            }
            return exist;
        }

        public static bool AskForYesNo(string question)
        {
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
    }
}
