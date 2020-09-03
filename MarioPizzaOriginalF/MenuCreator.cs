using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain;
using Pastel;
using TinyIoC;
using DColor = System.Drawing.Color;

namespace MarioPizzaOriginal
{
    public class MenuCreator
    {
        private Dictionary<string, Action> menuActions;
        private List<Action> gobackActions;
        private string header;
        private string footer;

        public MenuCreator()
        {
            menuActions = new Dictionary<string, Action>();
            gobackActions = new List<Action>();
            header = "Dostępne opcje: ";
            footer = "Wyjście";
        }

        public MenuCreator(string header, string footer, Dictionary<string, Action> actions)
        {
            menuActions = actions;
            menuActions.Add(footer, null);
            gobackActions = new List<Action>();
            this.header = header;
        }
        public static MenuCreator Create()
        {
            return new MenuCreator
            {
                menuActions = new Dictionary<string, Action>(),
                gobackActions = new List<Action>(),
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

        public MenuCreator AddOption(string description, Action action)
        {
            menuActions.Add(description, action);
            return this;
        }

        public MenuCreator AddGoBackAction(Action cause)
        {
            gobackActions.Add(cause);
            return this;
        }

        public void Present() => UniversalPresent(true);

        public void PresentRightless() => UniversalPresent(false);

        private void UniversalPresent(bool respectRights)
        {
            var exit = false;
            var index = 1;

            var keys = new List<string>();
            var values = new List<Action>();
            var user = TinyIoCContainer.Current.Resolve<User>("CurrentUser");
            foreach (var entry in menuActions)
            {
                if (entry.Value == null || user.HasPermission(entry.Value.Method.Name) || !respectRights)
                {
                    keys.Add($"{index++.ToString().Pastel(DColor.Coral)}. {entry.Key}");
                    values.Add(entry.Value);
                }
            }
            do
            {
                Console.Clear();
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));
                keys.ForEach(Console.WriteLine);
                var input = ViewHelper.AskForInt("", clear: false); //Waiting for answer
                if (input > 0 && input <= values.Count)
                {
                    if (input == values.Count) exit = true;
                    else
                    {
                        values[input - 1]();
                        if (gobackActions.Contains(values[input - 1]))
                            exit = true;
                    }
                }
                else ViewHelper.WriteAndWait($"Nie ma opcji: {input.ToString().Pastel(DColor.Red)}!");
            } while (!exit);
        }
    }
}
