using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Filter;
using Model.DataAccess;
using System;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Controller
{
    public class FoodController
    {
        private readonly IFoodRepository _foodRepository;
        public FoodController(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        private List<string> ShowIngredients(List<Tuple<string, double>> ingredients)
        {
            var formatted = new List<string> { "Składniki: " };
            ingredients.ForEach(x => {
                formatted.Add($"* {x.Item2} {x.Item1} (?)");
            });
            return formatted;
        }

        private string ConvertProductionTime(int? time)
        {
            return $"{(int) time/60}min {(int) time}s";
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
            List<Tuple<string,double>> ingredients = _foodRepository.GetIngredients(foodId);
            if (food.Ingredients == null) Console.WriteLine("Brak składników");
            else
            {
                entries.AddRange(ShowIngredients(ingredients));
            }
            entries.ForEach(x => Console.WriteLine(x));
            Console.ReadLine();
        }

        public void CalculatePriceForOrder()
        {
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz policzyć cene: ");
            double price = _foodRepository.CalculatePriceForFood(orderId);
            ViewHelper.WriteAndWait($"Cena: {price} zł");
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
    }
}
