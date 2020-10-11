using System;
using System.Collections.Generic;
using System.Linq;

namespace MarioPizzaOriginal.Tools
{
    public class FakeConsole : IConsole
    {
        public ConsoleColor ForegroundColor
        {
            get => ConsoleColor.White;
            set { }
        }

        public Stack<string> Input { get; }
        public List<string> Output { get; }

        public FakeConsole(IEnumerable<string> input)
        {
            Output = new List<string>();
            var readyInput = input ?? new string[] { };
            Input = new Stack<string>(readyInput.Reverse());
            _isLastWriteLine = true;
            _keyList = new Queue<ConsoleKeyInfo>();
        }

        public void LoadKeylist()
        {
            if (Input.Count == 0)
                throw new FakeConsoleNoInputException("There's nothing to Read from current line of Input!");
            var word = Input.Peek();
            for (var i = 0; i < word.Length; i++)
            {
                var letter = word[i];
                Enum.TryParse(letter.ToString(), true, out ConsoleKey key);
                if (i == word.Length - 1)
                    key = ConsoleKey.Enter;
                if (letter == '\b')
                    key = ConsoleKey.Backspace;
                _keyList.Enqueue(new ConsoleKeyInfo(letter, key, false, false, false));
            }
        }

        public void WriteLine(string text)
        {
            Output.Add(text);
            _isLastWriteLine = true;
        }

        public void Write(string text)
        {
            if (_isLastWriteLine)
                Output.Add(text);
            else Output[Output.Count - 1] += text;
            _isLastWriteLine = false;
        }

        public void Write(char c) => Write(c.ToString());

        public string ReadLine()
        {
            if (Input.Count == 0)
                throw new FakeConsoleNoInputException("There's nothing to Read from current line of Input!");
            return Input.Pop();
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            if (_keyList.Count == 0)
                LoadKeylist();

            if (_keyList.Count != 0)
            {
                // możliwy błąd kiedy skończą się litery, a pobierzemy następną literę kolejnego wyrazu
                // wartoby to jakoś zarejestrować
                var word = Input.Pop();
                if (word.Length > 1)
                    Input.Push(word.Substring(1, word.Length - 1));
                return _keyList.Dequeue();
            }
            throw new FakeConsoleNoInputException("There's nothing to Read from current line of Input!");

            // var sliced = Input.Pop();
            // if (sliced.Length < 1)
            // var outputChar = sliced[0];
            // var ignoreConsoleKey = sliced != "!" ? ConsoleKey.A : ConsoleKey.Enter;
            // if (sliced.Length != 1)
            //     Input.Push(sliced.Substring(1, sliced.Length - 1));
            // return new ConsoleKeyInfo(outputChar, ignoreConsoleKey, false, false, false);
        }

        public void Clear()
        {
            Output.Clear();
            _isLastWriteLine = true;
        }


        private bool _isLastWriteLine;
        private readonly Queue<ConsoleKeyInfo> _keyList;
    }

    public class FakeConsoleNoInputException : ArgumentException
    {
        public FakeConsoleNoInputException(string message) : base(message)
        {
        }
    }
}
