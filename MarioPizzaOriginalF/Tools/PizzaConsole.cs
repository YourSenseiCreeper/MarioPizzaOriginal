using System;

namespace MarioPizzaOriginal.Tools
{
    public class PizzaConsole : IConsole
    {
        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }
        public void WriteLine(string text) => Console.WriteLine(text);
        public void Write(string text) => Console.Write(text);
        public void Write(char c) => Console.Write(c);

        public string ReadLine() => Console.ReadLine();
        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

        public void Clear() => Console.Clear();
    }
}
