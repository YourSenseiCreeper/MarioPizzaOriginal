using MarioPizzaOriginal.Domain;
using Model.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class IngredientController
    {
        private readonly IIngredientRepository _ingredientRepository;
        public IngredientController(TinyIoCContainer container)
        {
            _ingredientRepository = container.Resolve<IIngredientRepository>();
        }

        public void AddIngredient()
        {
            Ingredient newIngredient = new Ingredient
            {
                IngredientName = ViewHelper.AskForStringNotBlank("Podaj nazwę nowego składnika: "),
                UnitOfMeasureType = ViewHelper.AskForOption<UnitOfMeasure>("Dostępne jednostki miary:", "Podaj numer jednostki miary: ")
            };

            Console.WriteLine("Wpisz -1 jeżeli chcesz pozostawić pole puste");
            string priceSmall = ViewHelper.AskForString($"Podaj cenę (mały rozmiar): ");
            if (!priceSmall.Equals("-1")) { newIngredient.PriceSmall = Convert.ToDouble(priceSmall.Replace(".", ",")); }

            string priceMedium = ViewHelper.AskForString($"Podaj cenę (średni rozmiar): ");
            if (!priceMedium.Equals("-1")) { newIngredient.PriceMedium = Convert.ToDouble(priceMedium.Replace(".", ",")); }

            string priceLarge = ViewHelper.AskForString($"Podaj cenę (duży rozmiar): ");
            if (!priceLarge.Equals("-1")) { newIngredient.PriceLarge = Convert.ToDouble(priceLarge.Replace(".", ",")); }

            _ingredientRepository.Add(newIngredient);
            ViewHelper.WriteAndWait($"Dodano składnik: {newIngredient.IngredientName}");
        }

        public void GetAllIngredients()
        {
            Console.Clear();
            Console.WriteLine("Lista wszystkich składników:");
            ShowIngredients(_ingredientRepository.GetAll());
            Console.ReadLine();
        }

        public void EditIngredient()
        {
            var ingredientId = ViewHelper.AskForInt("Podaj id składnika który chcesz modyfikować: ");
            if(!_ingredientRepository.Exists(ingredientId))
            {
                ViewHelper.WriteAndWait($"Nie znaleziono składnika o id: {ingredientId}");
            }
            Ingredient actual = _ingredientRepository.Get(ingredientId);
            string ingredientName = ViewHelper.AskForString($"Podaj nazwę składnika ({actual.IngredientName}): ");
            if (!ingredientName.Equals(""))
            {
                actual.IngredientName = ingredientName;
            }

            actual.UnitOfMeasureType = ViewHelper.AskForOption<UnitOfMeasure>("Dostępne jednostki miary: ", $"Jednostka miary ({actual.UnitOfMeasureType}): ", actual.UnitOfMeasureType.ToString());

            Console.WriteLine("Wpisz -1 by ustawić wartość NULL");
            string priceSmall = ViewHelper.AskForString($"Podaj cenę (mały rozmiar) ({actual.PriceSmall}): ");
            if (priceSmall.Equals("-1")) { actual.PriceSmall = null; }
            else if (!priceSmall.Equals("")) { actual.PriceSmall = Convert.ToDouble(priceSmall.Replace(",", ".")); }

            string priceMedium = ViewHelper.AskForString($"Podaj cenę (średni rozmiar) ({actual.PriceMedium}): ");
            if (priceMedium.Equals("-1")) { actual.PriceMedium = null; }
            else if (!priceMedium.Equals("")) { actual.PriceMedium = Convert.ToDouble(priceMedium.Replace(",", ".")); }

            string priceLarge = ViewHelper.AskForString($"Podaj cenę (duży rozmiar) ({actual.PriceLarge}): ");
            if (priceLarge.Equals("-1")) { actual.PriceLarge = null; }
            else if (!priceLarge.Equals("")) { actual.PriceLarge = Convert.ToDouble(priceLarge.Replace(",", ".")); }

            _ingredientRepository.Edit(actual);
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
        }

        public void DeleteIngredient()
        {
            var ingredientId = ViewHelper.AskForInt("Podaj id składnika który chcesz usunąć: ");
            _ingredientRepository.Remove(ingredientId);
            ViewHelper.WriteAndWait($"Usunięto składnik o id {ingredientId}");
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

        public void GetIngredient()
        {
            int ingredientId = ViewHelper.AskForInt("Podaj id składnika: ");
            var ingredient = _ingredientRepository.Get(ingredientId);
            
            if (ingredient == null) ViewHelper.WriteAndWait($"Składnik o numerze {ingredientId} nie istnieje!");
            else DescribeIngredient(ingredient);
        }

        //Obsolete
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
                    int input = ViewHelper.AskForInt("Podaj minimlane Id składnika:");
                    if (input > 0) { ingredientIdMin = input; }
                    else { ingredientIdMin = -1; }
                }
                else if (option.Equals("2"))
                {
                    int input = ViewHelper.AskForInt("Podaj maksymalne Id składnika:");
                    if (input > 0) { ingredientIdMax = input; }
                    else { ingredientIdMax = -1; }
                }
                else if (option.Equals("3"))
                {
                    string input = ViewHelper.AskForString("Podaj ciąg który znajduje się w Nazwie składnika:");
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
        public void GetFilteredIngredients()
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
        }
    }
}
