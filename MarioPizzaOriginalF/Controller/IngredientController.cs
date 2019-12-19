using MarioPizzaOriginal.DataAccess;
using MarioPizzaOriginal.Domain;
using Model.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarioPizzaOriginal.Controller
{
    public class IngredientController
    {
        private readonly IIngredientRepository _ingredientRepository;
        public IngredientController(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public MarioResult AddIngredient()
        {
            Console.Clear();
            Ingredient newIngredient = new Ingredient();
            bool nameOk = false;
            while (!nameOk)
            {
                Console.Write("Podaj nazwę nowego składnika: ");
                string ingredientName = Console.ReadLine();

                if (!ingredientName.Equals(""))
                {
                    nameOk = true; 
                    newIngredient.IngredientName = ingredientName;
                }
                else Console.WriteLine("Nazwa nie może być pusta!");
            }
            UnitOfMeasure unitofmeasure = UnitOfMeasure.NONE;
            List<UnitOfMeasure> uoms = Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>().ToList();
            
            while(unitofmeasure == UnitOfMeasure.NONE)
            {
                uoms.ForEach(uom => Console.WriteLine($"{(int)uom}. {uom}"));
                Console.Write("Wybierz jednostkę miary spośród podanych: ");
                var userUOM = Console.ReadLine();
                try
                {
                    //Not working :(
                    unitofmeasure = (UnitOfMeasure) Enum.Parse(typeof(UnitOfMeasure), userUOM);
                }
                catch(ArgumentException) { Console.WriteLine($"{userUOM} nie istnieje!");  }
            }
            newIngredient.UnitOfMeasureType = unitofmeasure;

            Console.WriteLine("Wpisz -1 jeżeli chcesz pozostawić pole puste");
            Console.Write($"Podaj cenę (mały rozmiar): ");
            string priceSmall = Console.ReadLine();
            if (!priceSmall.Equals("-1")) { newIngredient.PriceSmall = Convert.ToDouble(priceSmall.Replace(".", ",")); }

            Console.Write($"Podaj cenę (średni rozmiar): ");
            string priceMedium = Console.ReadLine();
            if (!priceMedium.Equals("-1")) { newIngredient.PriceMedium = Convert.ToDouble(priceMedium.Replace(".", ",")); }

            Console.Write($"Podaj cenę (duży rozmiar): ");
            string priceLarge = Console.ReadLine();
            if (!priceLarge.Equals("-1")) { newIngredient.PriceLarge = Convert.ToDouble(priceLarge.Replace(".", ",")); }

            _ingredientRepository.Add(newIngredient);
            var message = $"Dodano składnik: {newIngredient.IngredientName}";
            Console.WriteLine(message);
            return new MarioResult { Success = true, Message = message };
        }

        public MarioResult GetAllIngredients()
        {
            Console.Clear();
            Console.WriteLine("Lista wszystkich składników:");
            ShowIngredients(_ingredientRepository.GetAll());
            return new MarioResult { Success = true };
        }

        public MarioResult EditIngredient()
        {
            Console.Write("Podaj id składnika który chcesz modyfikować: ");
            var ingredientId = Convert.ToInt32(Console.ReadLine());
            if(!_ingredientRepository.Exists(ingredientId))
            {
                var message = $"Nie znaleziono składnika o id: {ingredientId}";
                Console.WriteLine(message);
                return new MarioResult { Success = false, Message = message};
            }
            Ingredient actual = _ingredientRepository.Get(ingredientId);
            Ingredient edited = new Ingredient { IngredientId = actual.IngredientId };
            Console.Write($"Podaj nazwę składnika ({actual.IngredientName}): ");
            var ingredientName = Console.ReadLine();
            if (!ingredientName.Equals(""))
            {
                edited.IngredientName = ingredientName;
            }

            List<UnitOfMeasure> uoms = Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>().ToList();
            uoms.ForEach(uom => Console.WriteLine($"{(int)uom}. {uom}"));
            Console.Write($"Jednostka miary ({actual.UnitOfMeasureType}): ");
            var userUOM = Console.ReadLine();
            if (!userUOM.Equals(""))
            {
                try
                {
                    edited.UnitOfMeasureType = (UnitOfMeasure)Enum.Parse(typeof(UnitOfMeasure), userUOM);
                }
                catch (ArgumentException) { Console.WriteLine($"{userUOM} nie istnieje!"); }
            }
            Console.Write($"Podaj cenę (mały rozmiar) ({actual.PriceSmall}): ");
            string priceSmall = Console.ReadLine();
            if (!priceSmall.Equals("")) { edited.PriceSmall = Convert.ToDouble(priceSmall.Replace(",", ".")); }

            Console.Write($"Podaj cenę (średni rozmiar) ({actual.PriceMedium}): ");
            string priceMedium = Console.ReadLine();
            if (!priceMedium.Equals("")) { edited.PriceMedium = Convert.ToDouble(priceMedium.Replace(",", ".")); }

            Console.Write($"Podaj cenę (duży rozmiar) ({actual.PriceLarge}): ");
            string priceLarge = Console.ReadLine();
            if (!priceLarge.Equals("")) { edited.PriceLarge = Convert.ToDouble(priceLarge.Replace(",", ".")); }

            _ingredientRepository.Edit(edited);
            return new MarioResult { Success = true };
        }

        private void ShowIngredients(List<Ingredient> ingredients)
        {
            string header = $"{"Id".PadRight(5)}|" +
                $"{"Nazwa składnika".PadRight(30)}|" +
                $"{"Jednostka miary".PadRight(15)}|" +
                $"{"Cena (Mała)".PadRight(10)}|"+
                $"{"Cena (Średnia)".PadRight(10)}|"+
                $"{"Cena (Duża)".PadRight(10)}";
            Console.WriteLine(header);
            for (int i = 0; i < header.Length; i++)
            {
                Console.Write("=");
            }
            Console.Write("\n");
            ingredients.ForEach(x => {
                Console.WriteLine($"{x.IngredientId.ToString().PadRight(5)}|" +
                    $"{x.IngredientName.PadRight(30)}|" +
                    $"{x.UnitOfMeasureType.ToString().PadRight(15)}|" +
                    $"{x.PriceSmall.ToString().PadRight(10)}|" +
                    $"{x.PriceMedium.ToString().PadRight(10)}|" +
                    $"{x.PriceLarge.ToString().PadRight(10)}");
            });
            Console.ReadLine();
        }
        public void AllIngredients()
        {
            Console.WriteLine("Lista dostępnych składników:");
            ShowIngredients(_ingredientRepository.GetAll());
        }

        public MarioResult DeleteIngredient()
        {
            Console.WriteLine("Podaj nazwę lub numer składnika który chcesz usunąć");
            var ingredient = Console.ReadLine();
            bool success = false;
            if (Int32.TryParse(ingredient, out int ingredientId))
            {
                _ingredientRepository.Remove(ingredientId);
                success = true;
            }
            var message = success ? $"Usunięto składnik {ingredient}" : "Podany składnik nie istnieje!";
            Console.WriteLine(message);
            return new MarioResult { Success = success, Message = message };
        }

        private void DescribeIngredient(Ingredient ingredient)
        {
            List<string> text = new List<string> { 
                $"Nazwa składnika: {ingredient.IngredientName}",
                $"Numer porządkowy: {ingredient.IngredientId}",
                $"Jednostka miary: {ingredient.UnitOfMeasureType.ToString()}",
                $"Cena (Mała): {ingredient.PriceSmall}",
                $"Cena (Średnia): {ingredient.PriceMedium}",
                $"Cena (Duża): {ingredient.PriceLarge}"};
            text.ForEach(line => Console.WriteLine(line));
        }

        /*
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
        */

        public MarioResult GetIngredient()
        {
            Console.Write("Podaj id składnika: ");
            int ingredientId = Convert.ToInt32(Console.ReadLine());
            var ingredient = _ingredientRepository.Get(ingredientId);
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

        //Unused
        public MarioResult GetFilteredIngredients()
        {
            Dictionary<string, object> filter = GetFilteredIngredientsDict();
            int ingredientIdMin = (int) filter["IngredientIdMin"];
            int ingredientIdMax = (int) filter["IngredientIdMax"];
            string ingredientName = (string) filter["IngredientName"];
            UnitOfMeasure unitOfMeasure = (UnitOfMeasure) filter["UnitOfMeasure"];
            double amountOfUOMmin = (double)filter["AmountOfUOMmin"];
            double amountOfUOMmax = (double)filter["AmountOfUOMmax"];

            var filteredValues = _ingredientRepository.GetAll().FindAll(x =>
                (ingredientIdMin != -1 || x.IngredientId >= ingredientIdMin) &&
                (ingredientIdMax != -1 || x.IngredientId <= ingredientIdMax) &&
                (ingredientName != "" || x.IngredientName.Contains(ingredientName)) &&
                (unitOfMeasure != UnitOfMeasure.NONE || x.UnitOfMeasureType == unitOfMeasure) &&
                (amountOfUOMmin != -1 || x.PriceSmall >= amountOfUOMmin) &&
                (amountOfUOMmax != -1 || x.PriceSmall <= amountOfUOMmax)
            );
            Console.WriteLine($"Znaleziono {filteredValues.Count()} pasujących do filtra:");
            ShowIngredients(filteredValues);
            return new MarioResult { Success = true };
        }
    }
}
