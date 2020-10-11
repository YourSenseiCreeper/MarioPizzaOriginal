using System;
using System.Collections.Generic;
using System.Linq;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Domain.Enums;
using Pastel;
using DColor = System.Drawing.Color;

namespace MarioPizzaOriginal.Tools
{
    public class ViewHelper
    {
        private readonly IConsole _console;
        public ViewHelper(IConsole console)
        {
            _console = console;
        }

        public int AskForInt(string message, int? min = null, int? max = null, int? current = null, bool exit = true, bool clear=true)
        {
            string answer;
            bool answerOk;
            int result = 0;
            do
            {
                answerOk = false;
                var formatedMessage = current != null ? $"{message.Replace(":", "")} ({current}): " : message;
                if (clear) _console.Clear();
                _console.Write(formatedMessage);
                answer = _console.ReadLine();
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

        public double AskForDouble(string message, bool inline = true, bool clear = true)
        {
            var answerOk = false;
            double result = 0;
            do
            {
                if (clear) _console.Clear();
                if (inline) _console.Write(message);
                else _console.WriteLine(message);
                var answer = _console.ReadLine().Replace(".", ",");
                if (double.TryParse(answer, out var innerResult))
                {
                    answerOk = true; 
                    result = innerResult;
                }
                else WriteAndWait($"{answer} nie jest liczbą!");
            } while (!answerOk);
            return result;
        }

        public string AskForString(string message, bool inline = true, bool clear = true)
        {
            if (clear) _console.Clear();
            if (inline) _console.Write(message);
            else _console.WriteLine(message);
            return _console.ReadLine();
        }

        public string AskForStringNotBlank(string message, bool inline = true, bool clear = true)
        {
            string answer;
            bool answerOk = false;
            do
            {
                if (clear) _console.Clear();
                if (inline) _console.Write(message);
                else _console.WriteLine(message);
                answer = _console.ReadLine();
                if (!string.IsNullOrEmpty(answer)) answerOk = true;
                else { WriteAndWait("Nazwa nie może być pusta!"); }
            } while (!answerOk);
            return answer;
        }
        /// <summary>
        /// Method asking for password replacing writed letters with asterisks. By CraigTP
        /// </summary>
        public string AskForPassword(string message)
        {
            return EditableString(message, "", password: true);
        }

        public int EditableInt(string message, int value)
        {
            _console.ForegroundColor = ConsoleColor.Cyan;
            _console.Write(message + value);
            var stringValue = value.ToString();
            do
            {
                var keyInfo = _console.ReadKey(intercept: true);
                var key = keyInfo.Key;

                if (key == ConsoleKey.Enter)
                {
                    if (int.TryParse(stringValue, out var result))
                    {
                        value = result;
                        break;
                    }
                    _console.WriteLine($"'{stringValue}' nie jest liczbą całkowitą!");
                }
                if (key == ConsoleKey.Backspace && stringValue.Length > 0)
                {
                    _console.Write("\b \b");
                    stringValue = stringValue.Substring(0, stringValue.Length - 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    _console.Write(keyInfo.KeyChar);
                    stringValue += keyInfo.KeyChar;
                }
            } while (true);
            _console.Write("\n");
            _console.ForegroundColor = ConsoleColor.White;
            return value;
        }

        public string EditableString(string message, string value, bool password=false)
        {
            _console.ForegroundColor = ConsoleColor.Cyan;
            _console.Write(message + value);
            do
            {
                var keyInfo = _console.ReadKey(intercept: true);
                var key = keyInfo.Key;

                if (key == ConsoleKey.Enter) break;
                if (key == ConsoleKey.Backspace && value.Length > 0)
                {
                    _console.Write("\b \b");
                    value = value.Substring(0, value.Length - 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    _console.Write(password ? '*' : keyInfo.KeyChar);
                    value += keyInfo.KeyChar;
                }
            } while (true);
            _console.Write("\n");
            _console.ForegroundColor = ConsoleColor.White;
            return value;
        }

        public T EditableValue<T>(string messageAllElements, string messageNewValue, T enumValue) where T : Enum
        {
            var answerOk = false;
            do
            {
                _console.Clear();
                var index = 0;
                _console.WriteLine(messageAllElements);
                Enum.GetNames(typeof(T)).ToList().ForEach(element => _console.WriteLine($"{index++}. {element}"));
                var answer = EditableString(messageNewValue, enumValue.ToString());
                try
                {
                    //If currentValue is not Null or Empty parse currentValue otherwise parse answer
                    //if (string.IsNullOrEmpty(answer)) answer = currentValue;
                    enumValue = (T)Enum.Parse(typeof(T), answer.ToUpper());
                    answerOk = true;
                }
                catch (ArgumentException) { WriteAndWait($"'{answer}' nie jest jedną z możliwych wartości!"); }
            } while (!answerOk);
            return enumValue;
        }


        public T AskForOption<T>(string messageAllElements, string messageNewValue, string currentValue = "", 
            bool inline = true) where T : Enum
        {
            return UnsafeAskForOption<T>(messageAllElements, messageNewValue, inline);
        }

        public T UnsafeAskForOption<T>(string messageAllElements, string messageNewValue, bool inline = true)
        {
            T result = default;
            var answerOk = false;
            do
            {
                _console.Clear();
                int index = 0;

                _console.WriteLine(messageAllElements);
                Enum.GetNames(typeof(T)).ToList().ForEach(element => _console.WriteLine($"{index++}. {element}"));
                _console.Write(messageNewValue);
                var answer = _console.ReadLine();

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

        public List<T> AskForListFromPrepared<T>(string messageAllElements, string messageNewValue, Func<string, T> converter,
            List<T> preparedOptions, string addElement, string removeElement, List<T> currentValues = null)
        {
            var result = currentValues ?? new List<T>();
            var exit = false;
            do
            {
                _console.Clear();
                _console.WriteLine($"1. {addElement}");
                _console.WriteLine($"2. {removeElement}");
                _console.WriteLine("3. Zakończ");
                _console.WriteLine("Wybrane elementy: " + string.Join(", ", result));
                var answer = _console.ReadLine();
                if (answer.Equals("1"))
                {
                    _console.Clear();
                    _console.WriteLine(messageAllElements);
                    var index = 0;
                    preparedOptions.ForEach(element => _console.WriteLine($"{index++}. {element}"));
                    _console.Write(messageNewValue);
                    _console.WriteLine("Jeżeli rezygnujesz, wciśnij enter");
                    AddToListListByNameOrId(preparedOptions, result, converter);
                }
                else if (answer.Equals("2"))
                {
                    _console.Clear();
                    _console.WriteLine("Wpisz nazwę elementu aby usunąć go z listy.");
                    _console.WriteLine("Jeżeli rezygnujesz, wciśnij enter");
                    _console.WriteLine(string.Join(", ", result));
                    RemoveFromListByNameOrId(result, converter);
                }
                else if (answer.Equals("3")) exit = true;
                else WriteAndWait($"Nie ma opcji '{answer}'");

            } while (!exit);
            return result;
        }

        private void AddToListListByNameOrId<T>(List<T> preparedOptions, List<T> currentValues, 
            Func<string, T> converter)
        {
            currentValues = currentValues ?? new List<T>();
            var picked = _console.ReadLine();
            if (string.IsNullOrEmpty(picked)) return;
            if (int.TryParse(picked, out var pickedNumber))
            {
                if (pickedNumber < 0 || pickedNumber >= preparedOptions.Count)
                    WriteAndWait($"Numer musi być z zakresu od 0 do {preparedOptions.Count - 1}!");
                else
                {
                    var pickedElement = preparedOptions[pickedNumber];
                    if (currentValues.Contains(pickedElement))
                        WriteAndWait($"'{pickedElement}' jest już na liście!");
                    else
                        currentValues.Add(pickedElement);
                }
            }
            else if (preparedOptions.Contains(converter(picked)))
            {
                var pickedElement = preparedOptions[pickedNumber];
                if (currentValues.Contains(pickedElement))
                    WriteAndWait($"'{pickedElement}' jest już na liście!");
                else
                    currentValues.Add(pickedElement);
            }
            else WriteAndWait($"'{picked}' nie jest jedną z możliwych wartości!");
        }

        private void RemoveFromListByNameOrId<T>(List<T> currentValues, Func<string, T> converter)
        {
            if (currentValues == null || currentValues.Count == 0)
            {
                WriteAndWait("Lista jest pusta!");
                return;
            }

            var picked = _console.ReadLine();
            if (string.IsNullOrEmpty(picked)) return;
            if (int.TryParse(picked, out var pickedNumber))
            {
                var userPickedNumber = pickedNumber - 1;
                if (userPickedNumber < 0 || userPickedNumber >= currentValues.Count)
                    WriteAndWait($"Numer musi być z zakresu od 1 do {currentValues.Count}!");
                else
                    currentValues.RemoveAt(userPickedNumber);
                return;
            }

            var pickedElement = converter(picked);
            if (currentValues.Contains(pickedElement))
            {
                currentValues.Remove(pickedElement);
                return;
            }

            WriteAndWait($"'{picked}' nie jest jedną z możliwych wartości!");
        }

        public bool AskForYesNo(string question)
        {
            _console.Clear();
            _console.Write($"{question} (y/n): ");
            var options = new List<string> { "yes", "y", "tak", "t" };
            var answer = _console.ReadLine();
            return options.Contains(answer?.ToLower().Trim());
        }
        
        public void WriteAndWait(string message)
        {
            _console.WriteLine(message);
            _console.ReadLine();
        }

        public string StatusColor(OrderStatus status)
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
        public bool CheckIfElementNotExists<T>(IRepository<T> repository, string message, string missing, out int elementId)
        {
            elementId = AskForInt(message);
            if (repository.Exists(elementId))
                return false;
            WriteAndWait(string.Format(missing, elementId));
            return true;
        }
    }
}
