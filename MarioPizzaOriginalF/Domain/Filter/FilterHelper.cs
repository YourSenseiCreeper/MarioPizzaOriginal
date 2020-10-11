using System;
using System.Linq;
using MarioPizzaOriginal.Tools;
using TinyIoC;

namespace MarioPizzaOriginal.Domain.Filter
{
    public class FilterHelper
    {
        private readonly ViewHelper _viewHelper;
        private readonly IConsole _console;

        public FilterHelper(IConsole console)
        {
            _console = console;
            _viewHelper = new ViewHelper(console);
        }

        public object FilterDouble(string message, object[] args)
        {
            bool answerOk;
            double result = 0;
            do
            {
                answerOk = false;
                _console.Clear();
                var currentValue = args?[0] != null ? args[0].ToString() : string.Empty;
                var answer = _viewHelper.EditableString(message, currentValue);
                if (string.IsNullOrEmpty(answer)) return null;
                if (double.TryParse(answer, out var innerResult))
                {
                    answerOk = true;
                    result = innerResult;
                }
                else _viewHelper.WriteAndWait($"'{answer}' nie jest liczbą!");
            } while (!answerOk);
            return result;
        }

        public object FilterInt(string message, object[] args)
        {
            bool answerOk;
            var result = 0;
            do
            {
                answerOk = false;
                var currentValue = args?[0] != null ? args[0].ToString() : string.Empty;
                var answer = _viewHelper.EditableString(message, currentValue);
                if (string.IsNullOrEmpty(answer)) return null;
                if (int.TryParse(answer, out var innerResult))
                {
                    result = innerResult;
                    answerOk = true;
                }
                else _viewHelper.WriteAndWait($"'{answer}' nie jest liczbą!");
            } while (!answerOk);

            return result;
        }

        public object FilterString(string message, object[] args)
        {
            _console.Clear();
            var currentValue = args?[0] != null ? args[0].ToString() : string.Empty;
            var answer = _viewHelper.EditableString(message, currentValue);
            return string.IsNullOrEmpty(answer) ? null : answer;
        }

        public object FilterDateTime(string message, object[] args)
        {
            var answerOk = false;
            var result = new DateTime();
            do
            {
                _console.Clear();
                var currentValue = string.Empty;
                if (args?[0] != null)
                {
                    var argument = (DateTime) args[0];
                    currentValue = argument.ToShortDateString();
                }
                var answer = _viewHelper.EditableString(message, currentValue);
                if (string.IsNullOrEmpty(answer)) return null;
                try
                {
                    //result = DateTime.ParseExact(answer, "g", new CultureInfo("fr-FR"));
                    result = DateTime.Parse(answer);
                    answerOk = true;
                }
                catch (FormatException)
                {
                    _viewHelper.WriteAndWait($"'{answer}' zły format daty! Przykład: 01/01/2000 06:15");
                }
            } while (!answerOk);
            return result;
        }

        public object FilterOption<T>(string message, object[] args) where T : Enum
        {
            T result = default;
            var answerOk = false;
            do
            {
                _console.Clear();
                var index = 0;

                _console.WriteLine("Dostępne opcje: ");
                Enum.GetNames(typeof(T)).ToList().ForEach(element => _console.WriteLine($"{index++}. {element}"));
                var currentValue = string.Empty;
                if (args?[0] != null)
                {
                    var argument = (int) args[0];
                    currentValue = argument.ToString();
                }
                var answer = _viewHelper.EditableString(message, currentValue);
                if (string.IsNullOrEmpty(answer)) return null;
                try
                {
                    //If currentValue is not Null or Empty parse currentValue otherwise parse answer
                    //if (string.IsNullOrEmpty(answer)) answer = currentValue;
                    result = (T)Enum.Parse(typeof(T), answer.ToUpper());
                    answerOk = true;
                }
                catch (ArgumentException) { _viewHelper.WriteAndWait($"'{answer}' nie jest jedną z możliwych wartości!"); }
            } while (!answerOk);
            return result;
        }
    }
}
