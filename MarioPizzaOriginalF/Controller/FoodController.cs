using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Filter;
using Model;
using Model.DataAccess;
using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Controller
{
    public class FoodController
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IFoodIngredientRepository _foodIngredientRepository;
        public FoodController(IFoodRepository foodRepository, IFoodIngredientRepository foodIngredientRepository)
        {
            _foodRepository = foodRepository;
            _foodIngredientRepository = foodIngredientRepository;
        }

        private List<string> ShowIngredients(Dictionary<string, double> ingredients)
        {
            var formatted = new List<string> { "Składniki: " };
            foreach(var element in ingredients)
            {
                formatted.Add($"* {element.Key} {element.Value}");
            }
            return formatted;
        }

        private string ConvertProductionTime(int? time)
        {
            if (time != null)
            {
                int quoient = Math.DivRem((int)time, 60, out int remainder);
                return $"{quoient}min {remainder}s";
            }
            else return "0min 0s";
        }
        public void AddFood()
        {
            int step = 1;
            int maxStep = 6;
            var food = new Food
            {
                FoodName = ViewHelper.AskForStringNotBlank($"(krok {step++} z {maxStep}) Podaj nazwę nowego produktu: "),
                Ingredients = new List<FoodIngredient>()
            };
            
            bool addAnother = true;
            string input;
            if(ViewHelper.AskForYesNo($"(krok {step++} z {maxStep}) Chcesz dodać składniki produktu"))
            {
                while (addAnother)
                {
                    Console.Clear();
                    Console.WriteLine("Możliwe opcje:");
                    Console.WriteLine("1. Dodaj składnik");
                    Console.WriteLine("2. Zakończ dodawanie");
                    input = Console.ReadLine();
                    if (input.Equals("1"))
                    {
                        food.Ingredients.Add(new FoodIngredient
                        {
                            IngredientId = ViewHelper.AskForInt("Podaj id składnika który chcesz dodać: "),
                            IngredientAmount = ViewHelper.AskForDouble("Podaj ilość: ")
                        });
                    }
                    else if (input.Equals("2"))
                    {
                        addAnother = false;
                        ViewHelper.WriteAndWait("Dodawanie składników zakończone!");
                    }
                    else Console.WriteLine($"Nie ma opcji: {input}!");
                }
            }

            
            food.NettPrice = ViewHelper.AskForDouble($"(krok {step++} z { maxStep}) Podaj cenę netto: ");
            food.Price = ViewHelper.AskForDouble($"(krok {step++} z { maxStep}) Podaj cenę: ");

            ViewHelper.WriteAndWait("Wpisz -1 by ustawić wartość NULL");
            string weight = ViewHelper.AskForString($"(krok {step++} z { maxStep}) Podaj wagę produktu: ");
            if (weight.Equals("-1")) { food.Weight = null; }
            else if (!weight.Equals("")) { food.Weight = Convert.ToDouble(weight.Replace(".", ",")); }

            string productionTime = ViewHelper.AskForString($"(krok {step++} z { maxStep}) Podaj czas produkcji w sekundach: ");
            if (productionTime.Equals("-1")) { food.ProductionTime = null; }
            else if (!productionTime.Equals("")) { food.ProductionTime = Convert.ToInt32(productionTime); }
            /*
            string priceSmall = ViewHelper.AskForString($"(krok {step++}z { maxStep}) Podaj cenę (mały rozmiar): ");
            if (priceSmall.Equals("-1")) { food.PriceSmall = null; }
            else if (!priceSmall.Equals("")) { food.PriceSmall = Convert.ToDouble(priceSmall.Replace(",", ".")); }

            string priceMedium = ViewHelper.AskForString($"(krok {step++}z { maxStep}) Podaj cenę (średni rozmiar): ");
            if (priceMedium.Equals("-1")) { food.PriceMedium = null; }
            else if (!priceMedium.Equals("")) { food.PriceMedium = Convert.ToDouble(priceMedium.Replace(",", ".")); }

            string priceLarge = ViewHelper.AskForString($"(krok {step++}z { maxStep}) Podaj cenę (duży rozmiar): ");
            if (priceLarge.Equals("-1")) { food.PriceLarge = null; }
            else if (!priceLarge.Equals("")) { food.PriceLarge = Convert.ToDouble(priceLarge.Replace(",", ".")); }
            */
            _foodRepository.Add(food);
            ViewHelper.WriteAndWait($"Dodano produkt o nazwie {food.FoodName} i cenie {food.Price} zł");
        }
        
        public void GetFood()
        {
            var foodId = ViewHelper.AskForInt("Podaj id produktu: ");
            var food = _foodRepository.Get(foodId);
            var message = food != null ? "" : "Nie znaleziono produktu";
            if(food == null)
            {
                ViewHelper.WriteAndWait(message);
                return;
            }
            List<string> entries = new List<string> {
                $"=== {food.FoodName} ===",
                $"Id produktu: {food.FoodId}",
                $"Cena: {food.Price}zł ({food.NettPrice} zł netto)",
                $"Czas produkcji: {ConvertProductionTime(food.ProductionTime)}",
                $"Waga: {food.Weight}kg"
            };
            var ingredients = _foodRepository.GetIngredients(foodId);

            if (ingredients.Count == 0) entries.Add("Brak składników!");
            else entries.AddRange(ShowIngredients(ingredients));

            entries.ForEach(x => Console.WriteLine(x));
            Console.ReadLine();
        }

        public void CalculatePriceForOrder()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz policzyć cene: ");
            double price = _foodRepository.CalculatePriceForFood(orderId);
            ViewHelper.WriteAndWait($"Cena: {price} zł");
        }

        public void DeleteFood()
        {
            int foodId = ViewHelper.AskForInt("Podaj id produktu, który chcesz usunąć: ");
            var food = _foodRepository.Get(foodId);
            if (food == null)
            {
                ViewHelper.WriteAndWait($"Nie znaleziono produktu o id {foodId}");
                return;
            }
            string foodName = _foodRepository.GetName(foodId);
            _foodIngredientRepository.DeleteFoodIngredients(foodId);
            _foodRepository.Remove(foodId);
            ViewHelper.WriteAndWait($"Usunięto produkt {foodName} o id {foodId}");
        }

        private void ShowFood(List<Food> foodList)
        {
            var header = $"{"Id".PadRight(5)}|" +
                $"{"Nazwa".PadRight(25)}|" +
                $"{"Cena".PadLeft(5)}|";
            Console.WriteLine(header);
            for (int i = 0; i < header.Length; i++)
            {
                Console.Write("=");
            }
            Console.Write("\n");
            foodList.ForEach(x => {
                Console.WriteLine($"{x.FoodId.ToString().PadRight(5)}|" +
                    $"{x.FoodName.PadRight(25)}|" +
                    $"{x.Price.ToString().PadRight(5)} zł");
            });
            ViewHelper.WriteAndWait($"Znaleziono {foodList.Count} pasujących produktów:");
        }

        public void GetAllFood()
        {
            Console.Clear();
            ShowFood(_foodRepository.GetAll());
        }
        

        public void GetFilteredFood()
        {
            string option = "";
            List<string> filterList;
            var filter = new FoodFilter();
            filter.FoodIdMin = 10;
            //List<PropertyInfo> properties = new List<PropertyInfo>(filter.GetType().GetProperties());

            while (!option.Equals("-1"))
            {
                Console.Clear();
                filterList = new List<string> {
                "Wybierz numer filtra by go dostosować. Wpisz -1 by anulować zmiany",
                "1. Minimalne Id produktu " + (filter.FoodIdMin != null ? $"({filter.FoodIdMin})" : ""),
                "2. Maksymalne Id produktu " + (filter.FoodIdMax != null ? $"({filter.FoodIdMax})" : ""),
                "3. Nazwa produktu zawiera " + (filter.FoodName != null ? $"({filter.FoodName})" : ""),
                "4. Minmalna cena netto " + (filter.NettPriceMin != null ? $"({filter.NettPriceMin})" : ""),
                "5. Maksymalna cena netto " + (filter.NettPriceMax != null ? $"({filter.NettPriceMax})" : ""),
                "6. Minimalna cena " + (filter.PriceMin != null ? $"({filter.PriceMin})" : ""),
                "7. Maksymalna cena " + (filter.PriceMax != null ? $"({filter.PriceMax})" : ""),
                "8. Minimalna waga " + (filter.WeightMin != null ? $"({filter.WeightMin})" : ""),
                "9. Maksymalna waga " + (filter.WeightMax != null ? $"({filter.WeightMax})" : ""),
                "10. Minimalny czas produkcji " + (filter.ProductionTimeMin != null ? $"({filter.ProductionTimeMin})" : ""),
                "11. Maksymalny czas produkcji " + (filter.ProductionTimeMax != null ? $"({filter.ProductionTimeMax})" : ""),
                "12. Filtruj po składnikach",
                "13. WYŚWIETL WYNIKI"};
                filterList.ForEach(x => Console.WriteLine(x));
                //properties.ForEach(y => Console.WriteLine($"{y.Name} ({y.PropertyType.FullName}) = {y.GetValue(filter) ?? "NULL"}"));

                option = Console.ReadLine();
                switch (option)
                {
                    case "1": filter.FoodIdMin = ViewHelper.FilterInt("Podaj wartość dla Minimalne Id produktu: ", true); break;
                    case "2": filter.FoodIdMax = ViewHelper.FilterInt("Podaj wartość dla Maksymalne Id produktu: ", true); break;
                    case "3": filter.FoodName = ViewHelper.FilterString("Podaj ciąg który znajduje się w Nazwie produktu: "); break;
                    case "4": filter.NettPriceMin = ViewHelper.FilterDouble("Podaj minimalną cenę netto produktu: "); break;
                    case "5": filter.NettPriceMax = ViewHelper.FilterDouble("Podaj maksymalną cenę netto produktu: "); break;
                    case "6": filter.PriceMin = ViewHelper.FilterDouble("Podaj minimalną cenę (brutto) produktu: "); break;
                    case "7": filter.PriceMax = ViewHelper.FilterDouble("Podaj maksymalną cenę (brutto) produktu: "); break;
                    case "8": filter.WeightMin = ViewHelper.FilterDouble("Podaj minimalną wagę produktu w kg: "); break;
                    case "9": filter.WeightMax = ViewHelper.FilterDouble("Podaj maksymalną wagę produktu w kg: "); break;
                    case "10": filter.ProductionTimeMin = ViewHelper.FilterInt("Podaj minimalny czas produkcji w sekundach bez przecinka: "); break;
                    case "11": filter.ProductionTimeMax = ViewHelper.FilterInt("Podaj maksymalny czas produkcji w sekundach bez przecinka: "); break;
                    case "12": ViewHelper.WriteAndWait("do dodania"); break;
                    case "13": option = "-1"; break;
                    default: ViewHelper.WriteAndWait("Nie ma takiej opcji!"); break;
                }
            }
            ShowFood(_foodRepository.Filter(filter));
        }


        public void GetIngredients()
        {
            var foodId = ViewHelper.AskForInt("Podaj id produktu dla którego chcesz sprawdzić składniki:");
            if (_foodRepository.Exists(foodId))
            {
                ViewHelper.WriteAndWait($"Nie znaleziono produktu o id {foodId}");
                return;
            }
            ShowIngredients(_foodRepository.GetIngredients(foodId));
            Console.Read();
            return;
        }
        /*
        private static void Filter(string header, Dictionary<string, FilterObject> filterObjects, string footer)
        {
            bool exit = false;
            int input;
            int index = 1;
            
            filterObjects.Add(footer, null); // Last option - return
            //List<string> keys = new List<string>();
            //List<FilterObject> values = new List<FilterObject>();
            foreach (var entry in filterObjects)
            {
                keys.Add($"{index++}. {entry.Key}");
                values.Add(entry.Value);
            }
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine(header);
                keys.ForEach(x => Console.WriteLine(x));
                input = ViewHelper.AskForInt("", clear: false); //Waiting for answer
                if (input > 0 && input <= values.Count)
                {
                    if (input == values.Count) exit = true;
                    else values[input - 1] = FilterValue(values[input - 1]); //Possible loop? Nope
                }

                else ViewHelper.WriteAndWait($"Nie ma opcji: {input}!");
            }
        }

        private static FilterObject FilterValue(FilterObject filterElement)
        {
            Console.Write(filterElement.Message);
            string input = Console.ReadLine();
            bool answerOk = false;
            while (!answerOk)
            {
                try
                {
                    if (filterElement.IntendtType == typeof(int)) filterElement.IntFilter = Convert.ToInt32(input);
                    else if (filterElement.IntendtType == typeof(double)) filterElement.DoubleFilter = Convert.ToDouble(input);
                    else filterElement.StringFilter = input;
                    answerOk = true;
                }
                catch (FormatException)
                {
                    string variableType = filterElement.IntendtType == typeof(int) ? "liczbą całkowitą!" : "liczbą!";
                    Console.WriteLine($"{input} nie jest {variableType}");
                    Console.ReadLine();
                }
            }
            return filterElement;
        }
        */
    }

    public class FilterObject
    {
        public string Message { get; set; }
        public int? IntFilter { get; set; }
        public double? DoubleFilter { get; set; }
        public string StringFilter { get; set; }
        public Type IntendtType { get; set; }
        public Type ValueType
        {
            get {
                if (IntFilter != null) return typeof(int);
                else if (DoubleFilter != null) return typeof(double);
                else return typeof(string);
            }
        }
    }
}
