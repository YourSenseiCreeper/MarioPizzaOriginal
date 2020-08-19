using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Account;
using MarioPizzaOriginal.Domain;
using ServiceStack;
using TinyIoC;

namespace MarioPizzaOriginal
{
    public class MenuCreator
    {
        private Dictionary<string, Action> menuActions;
        private string header;
        private string footer;

        public static MenuCreator Create()
        {
            return new MenuCreator
            {
                menuActions = new Dictionary<string, Action>(),
                header = "Dostępne opcje: ",
                footer = "Wyjście"
            };
        }

        public MenuCreator SetHeader(string header)
        {
            this.header = header;
            return this;
        }

        public MenuCreator AddFooter(string footer)
        {
            this.footer = footer;
            menuActions.Add(footer, null);
            return this;
        }

        public MenuCreator AddOptionRange(Dictionary<string, Action> actions)
        {
            foreach (var pair in actions)
            {
                menuActions.Add(pair.Key, pair.Value);
            }
            return this;
        }

        public void Present()
        {
            bool exit = false;
            int input;
            int index = 1;

            List<string> keys = new List<string>();
            List<Action> values = new List<Action>();
            var user = TinyIoCContainer.Current.Resolve<User>("CurrentUser");
            foreach (var entry in menuActions)
            {
                if (entry.Value == null || user.HasPermission(entry.Value.Method.Name))
                {
                    keys.Add($"{index++}. {entry.Key}");
                    values.Add(entry.Value);
                }
            }
            do
            {
                Console.Clear();
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));
                keys.ForEach(Console.WriteLine);
                input = ViewHelper.AskForInt("", clear: false); //Waiting for answer
                if (input > 0 && input <= values.Count)
                {
                    if (input == values.Count) exit = true;
                    else values[input - 1]();
                }
                else ViewHelper.WriteAndWait($"Nie ma opcji: {input}!");
            } while (!exit);
        }
    }
}
