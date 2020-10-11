using System;

namespace MarioPizzaOriginal.Tools
{
    public interface IConsole
    {
        ConsoleColor ForegroundColor { set; }
        void WriteLine(string text);
        void Write(string text);
        void Write(char c);
        string ReadLine();
        ConsoleKeyInfo ReadKey(bool intercept);
        void Clear();
    }
}
