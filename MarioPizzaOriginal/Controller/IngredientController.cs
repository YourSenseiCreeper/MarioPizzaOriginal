using MarioPizzaOriginal.DataAccess;
using MarioPizzaOriginal.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarioPizzaOriginal.Controller
{
    public class IngredientController
    {
        private readonly IMarioPizzaRepository _marioPizzaRepository;
        public IngredientController(IMarioPizzaRepository marioPizzaRepository)
        {
            _marioPizzaRepository = marioPizzaRepository;
        }

        public MarioResult AddIngredient()
        {
            Console.WriteLine("Podaj nazwę nowego składnika:");
            var ingredientName = Console.ReadLine();
            UnitOfMeasure unitofmeasure = UnitOfMeasure.NONE;
            while(unitofmeasure == UnitOfMeasure.NONE)
            {
                Console.WriteLine("Wybierz jednostkę miary spośród podanych:");
                List<UnitOfMeasure> uoms = Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>().ToList();
                uoms.ForEach(uom => Console.WriteLine($"{(int)uom}. {uom}"));
                var userUOM = Console.ReadLine();
                try
                {
                    unitofmeasure = (UnitOfMeasure) Enum.Parse(typeof(UnitOfMeasure), userUOM);
                }
                catch(ArgumentException) { Console.WriteLine($"{userUOM} nie istnieje!");  }
            }
            Console.WriteLine("Podaj ilość nowego składnika: ");
            var amoutofUOM = Convert.ToDouble(Console.ReadLine());
            _marioPizzaRepository.AddIngredient(new Ingredient
            {
                IngredientId = _marioPizzaRepository.GetIngredientList().Count(),
                IngredientName = ingredientName,
                UnitOfMeasureType = unitofmeasure,
                AmountOfUOM = amoutofUOM
            });
            var message = $"Dodano nowy składnik: {ingredientName}";
            Console.WriteLine(message);
            return new MarioResult { Success = true, Message = message };
        }

        private void ShowIngredients(List<Ingredient> ingredients)
        {
            Console.Write("Id".PadLeft(5));
            Console.Write("Nazwa składnika".PadLeft(30));
            Console.Write("Jednostka miary".PadLeft(30));
            Console.Write("Ilość składnika\n".PadLeft(30));
            ingredients.ForEach(x => {
                Console.Write(x.IngredientId.ToString().PadLeft(5));
                Console.Write(x.IngredientName.PadLeft(20));
                Console.Write(x.UnitOfMeasureType.ToString().PadLeft(30));
                Console.Write(x.AmountOfUOM.ToString().PadLeft(25));
                Console.Write("\n");
            });
        }
        public void AllIngredients()
        {
            Console.WriteLine("Lista dostępnych składników:");
            ShowIngredients(_marioPizzaRepository.GetIngredientList());
        }

        public MarioResult DeleteIngredient()
        {
            Console.WriteLine("Podaj nazwę lub numer składnika który chcesz usunąć");
            var ingredient = Console.ReadLine();
            bool success;
            if (Int32.TryParse(ingredient, out int ingredientId))
            {
                success = _marioPizzaRepository.DeleteIngredient(ingredientId);
            }
            else
            {
                success = _marioPizzaRepository.DeleteIngredient(ingredient);
            }
            var message = success ? $"Usunięto składnik {ingredient}" : "Podany składnik nie istnieje!";
            Console.WriteLine(message);
            return new MarioResult { Success = success, Message = message };
        }

        private void DescribeIngredient(Ingredient ingredient)
        {
            List<string> text = new List<string> { $"Nazwa składnika: {ingredient.IngredientName}",
            $"Numer porządkowy: {ingredient.IngredientId}", $"Jednostka miary: {ingredient.UnitOfMeasureType.ToString()}",
            $"Ilość jednostki miary: {ingredient.AmountOfUOM}"};
            text.ForEach(x => Console.WriteLine(x));
        }

        public MarioResult GetIngredient(string ingredientName)
        {
            var ingredient = _marioPizzaRepository.GetIngredient(ingredientName);
            var message = $"Składnik {ingredientName} nie istnieje!";
            if(ingredient != null)
            {
                message = "";
                DescribeIngredient(ingredient);
            }
            return new MarioResult { Success = (ingredient != null), Message = message };
        }

        public MarioResult GetIngredient(int ingredientId)
        {
            var ingredient = _marioPizzaRepository.GetIngredient(ingredientId);
            var message = $"Składnik o numerze {ingredientId} nie istnieje!";
            if (ingredient != null)
            {
                message = "";
                DescribeIngredient(ingredient);
            }
            return new MarioResult { Success = (ingredient != null), Message = message };
        }

        public Dictionary<string, object> GetFilteredIngredientsDict()
        {
            Console.WriteLine("Wybierz numer filtra by go dostosować. Wpisz -1 by anulować zmiany");
            List<string> filterList = new List<string> { "1. Minimalne Id składnika", "2. Maksymalne Id składnika",
            "3. Nazwa składnika zawiera", "4. Jednostka miary", "5. Minimalna ilość składnika", "6. Maksymalna ilość składnika",
            "7. WYŚWIETL WYNIKI"};
            int ingredientIdMin = -1, ingredientIdMax = -1;
            string ingredientName = "";
            UnitOfMeasure unitOfMeasure = UnitOfMeasure.NONE;
            double amountOfUOMmin = -1, amountOfUOMmax = -1;
            var option = Console.ReadLine();
            while (!option.Equals("-1"))
            {
                if (option.Equals("1"))
                {
                    Console.WriteLine("Podaj minimlane Id składnika:");
                    // Not a number exception!!!
                    var input = Convert.ToInt32(Console.ReadLine());
                    if (input > 0) { ingredientIdMin = input; }
                    else { ingredientIdMin = -1; }
                }
                else if (option.Equals("2"))
                {
                    Console.WriteLine("Podaj maksymalne Id składnika:");
                    // Not a number exception!!!
                    var input = Convert.ToInt32(Console.ReadLine());
                    if (input > 0) { ingredientIdMax = input; }
                    else { ingredientIdMax = -1; }
                }
                else if (option.Equals("3"))
                {
                    Console.WriteLine("Podaj ciąg który znajduje się w Nazwie składnika:");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { ingredientName = input; }
                    else { ingredientName = ""; }
                }
                else if (option.Equals("4"))
                {
                    Console.WriteLine("Podaj jednostkę miary: ");
                    string values = Enum.GetValues(typeof(UnitOfMeasure)).ToString();
                    Console.WriteLine($"Dostępne wartości: {values}");
                    var input = Console.ReadLine();
                    if (input.Equals("-1")) { unitOfMeasure = UnitOfMeasure.NONE; }
                    else { unitOfMeasure = (UnitOfMeasure) Enum.Parse(typeof(UnitOfMeasure), input, true); }
                }
                else if (option.Equals("5"))
                {
                    Console.WriteLine("Podaj minimalną ilość składnika: ");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { amountOfUOMmin = Convert.ToDouble(input); }
                    else { amountOfUOMmin = -1; }
                }
                else if (option.Equals("6"))
                {
                    Console.WriteLine("Podaj maksymalną ilość składnika: ");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { amountOfUOMmax = Convert.ToDouble(input); }
                    else { amountOfUOMmax = -1; }
                }
                else if (option.Equals("7"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Nie ma takiej opcji!");
                }
                option = Console.ReadLine();
            }
            var output = new Dictionary<string, object>
            {
                ["IngredientIdMin"] = ingredientIdMin,
                ["IngredientIdMax"] = ingredientIdMax,
                ["IngredientName"] = ingredientName,
                ["UnitOfMeasure"] = unitOfMeasure,
                ["AmountOfUOMmin"] = amountOfUOMmin,
                ["AmountOfUOMmax"] = amountOfUOMmax
            };
            return output;
        }

        public MarioResult GetFilteredIngredients()
        {
            Dictionary<string, object> filter = GetFilteredIngredientsDict();
            int ingredientIdMin = (int) filter["IngredientIdMin"];
            int ingredientIdMax = (int) filter["IngredientIdMax"];
            string ingredientName = (string) filter["IngredientName"];
            UnitOfMeasure unitOfMeasure = (UnitOfMeasure) filter["UnitOfMeasure"];
            double amountOfUOMmin = (double)filter["AmountOfUOMmin"];
            double amountOfUOMmax = (double)filter["AmountOfUOMmax"];

            var filteredValues = _marioPizzaRepository.GetIngredientList().FindAll(x =>
                (ingredientIdMin != -1 || x.IngredientId >= ingredientIdMin) &&
                (ingredientIdMax != -1 || x.IngredientId <= ingredientIdMax) &&
                (ingredientName != "" || x.IngredientName.Contains(ingredientName)) &&
                (unitOfMeasure != UnitOfMeasure.NONE || x.UnitOfMeasureType == unitOfMeasure) &&
                (amountOfUOMmin != -1 || x.AmountOfUOM >= amountOfUOMmin) &&
                (amountOfUOMmax != -1 || x.AmountOfUOM <= amountOfUOMmax)
            );
            Console.WriteLine($"Znaleziono {filteredValues.Count()} pasujących do filtra:");
            ShowIngredients(filteredValues);
            return new MarioResult { Success = true };
        }
    }
}
