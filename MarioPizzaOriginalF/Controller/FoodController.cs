using MarioPizzaOriginal.Domain;
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

        private List<string> ShowIngredients(List<Ingredient> ingredients)
        {
            var formatted = new List<string> { "Składniki: " };
            ingredients.ForEach(x => {
                formatted.Add($"* {x.UnitOfMeasureType.ToString()} {x.IngredientName}");
            });
            return formatted;
        }

        private string ConvertProductionTime(int time)
        {
            return $"{time/60}min {time}s";
        }

        public MarioResult GetFood()
        {
            Console.Clear();
            var foodId = ViewHelper.AskForInt("Podaj id produktu: ");
            var pickedFood = _foodRepository.Get(foodId);
            var message = pickedFood != null ? "" : "Nie znaleziono produktu";
            if(pickedFood == null)
            {
                Console.WriteLine(message); 
                Console.ReadLine();
                return new MarioResult { Message = message, Success = false };
            }
            List<string> entries = new List<string> {
                $"=== {pickedFood.FoodName} ===",
                $"Id produktu: {pickedFood.FoodId}",
                $"Cena: {pickedFood.Price}zł ({pickedFood.NettPrice}zł netto)",
                $"Czas produkcji: {ConvertProductionTime((int) pickedFood.ProductionTime)}",
                $"Waga: {pickedFood.Weight}kg"
            };
            if(pickedFood.Ingredients == null) Console.WriteLine("Brak składników");
            else entries.AddRange(ShowIngredients(pickedFood.Ingredients));
            entries.ForEach(x => Console.WriteLine(x));
            Console.ReadLine();
            return new MarioResult { Message = message, Success = pickedFood != null };
        }

        public void CalculatePriceForOrder()
        {
            Console.Clear();
            int orderId = ViewHelper.AskForInt("Podaj id zamówienia dla którego chcesz policzyć cene: ");
            double price = _foodRepository.CalculatePriceForFood(orderId);
            Console.WriteLine($"Cena: {price} zł");
            Console.ReadLine();
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
            Console.ReadLine();
        }

        public void GetAllFood()
        {
            Console.Clear();
            ShowFood(_foodRepository.GetAll());
        }

        public void GetFilteredFood()
        {
            Console.WriteLine("Wybierz numer filtra by go dostosować. Wpisz -1 by anulować zmiany");
            List<string> filterList = new List<string> { "1. Minimalne Id produktu", "2. Maksymalne Id produktu",
            "3. Nazwa produktu zawiera", "4. Minmalna cena netto", "5. Maksymalna cena netto", "6. Minimalna cena",
            "7. Maksymalna cena", "8. Minimalna waga", "9. Maksymalna waga", "10. Minimalny czas produkcji",
            "11. Maksymalny czas produkcji", "12. Filtruj po składnikach", "13. WYŚWIETL WYNIKI"};
            int foodIdMin = -1, foodIdMax = -1;
            string productName = "";
            double netPriceMin = -1, netPriceMax = -1;
            double priceMin = -1, priceMax = -1;
            double weightMin = -1, weightMax = -1;
            double productionTimeMin = -1, productionTimeMax = -1;

            Dictionary<string, object> ingredientFilter = null;
            int ingredientIdMin = -1, ingredientIdMax = -1;
            string ingredientName = "";
            UnitOfMeasure unitOfMeasure = UnitOfMeasure.NONE;
            double amountOfUOMmin = -1, amountOfUOMmax = -1;
            var option = Console.ReadLine();
            while (!option.Equals("-1"))
            {
                if (option.Equals("1"))
                {
                    Console.WriteLine("Podaj wartość dla Minimalne Id produktu:");
                    // Not a number exception!!!
                    var input = Convert.ToInt32(Console.ReadLine());
                    if (input > 0) { foodIdMin = input; }
                    else { foodIdMin = -1; }
                }
                else if (option.Equals("2"))
                {
                    Console.WriteLine("Podaj wartość dla Maksymalne Id produktu:");
                    // Not a number exception!!!
                    var input = Convert.ToInt32(Console.ReadLine());
                    if (input > 0) { foodIdMax = input; }
                    else { foodIdMax = -1; }
                }
                else if (option.Equals("3"))
                {
                    Console.WriteLine("Podaj ciąg który znajduje się w Nazwie produktu:");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { productName = input; }
                    else { productName = ""; }
                }
                else if (option.Equals("4"))
                {
                    Console.WriteLine("Podaj minimalną cenę netto produktu: ");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { netPriceMin = Convert.ToDouble(input); }
                    else { netPriceMin = -1; }
                }
                else if (option.Equals("5"))
                {
                    Console.WriteLine("Podaj maksymalną cenę netto produktu: ");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { netPriceMax = Convert.ToDouble(input); }
                    else { netPriceMax = -1; }
                }
                else if (option.Equals("6"))
                {
                    Console.WriteLine("Podaj minimalną cenę (brutto) produktu:");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { priceMin = Convert.ToDouble(input); }
                    else { priceMin = -1; }
                }
                else if (option.Equals("7"))
                {
                    Console.WriteLine("Podaj maksymalną cenę (brutto) produktu:");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { priceMax = Convert.ToDouble(input); }
                    else { priceMax = -1; }
                }
                else if (option.Equals("8"))
                {
                    Console.WriteLine("Podaj minimalną wagę produktu w kg:");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { weightMin = Convert.ToDouble(input); }
                    else { weightMin = -1; }
                }
                else if (option.Equals("9"))
                {
                    Console.WriteLine("Podaj maksymalną wagę produktu w kg:");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { weightMax = Convert.ToDouble(input); }
                    else { weightMax = -1; }
                }
                else if (option.Equals("10"))
                {
                    Console.WriteLine("Podaj minimalny czas produkcji w sekundach bez przecinka:");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { productionTimeMin = Convert.ToInt32(input); }
                    else { productionTimeMin = -1; }
                }
                else if (option.Equals("11"))
                {
                    Console.WriteLine("Podaj maksymalny czas produkcji w sekundach bez przecinka:");
                    var input = Console.ReadLine();
                    if ((Convert.ToInt32(input) != -1)) { productionTimeMax = Convert.ToInt32(input); }
                    else { productionTimeMax = -1; }
                }
                else if (option.Equals("12"))
                {
                    //LEGACY CODE
                    IngredientController ingredient = new IngredientController(null);
                    ingredientFilter = ingredient.GetFilteredIngredientsDict();
                    ingredientIdMin = (int)ingredientFilter["IngredientIdMin"];
                    ingredientIdMax = (int)ingredientFilter["IngredientIdMax"];
                    ingredientName = (string)ingredientFilter["IngredientName"];
                    unitOfMeasure = (UnitOfMeasure)ingredientFilter["UnitOfMeasure"];
                    amountOfUOMmin = (double)ingredientFilter["AmountOfUOMmin"];
                    amountOfUOMmax = (double)ingredientFilter["AmountOfUOMmax"];
                }
                else if (option.Equals("13"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Nie ma takiej opcji!");
                }
                option = Console.ReadLine();
            }
            //TODO If option value == -1 -> this shouldn't be executed
            var filter = _foodRepository.GetAll().FindAll(x =>
                (foodIdMin == -1 || x.FoodId >= foodIdMin) &&
                (foodIdMax == -1 || x.FoodId <= foodIdMax) &&
                (productName == "" || x.FoodName.Contains(productName)) &&
                (netPriceMin == -1 || x.NettPrice >= netPriceMin) &&
                (netPriceMax == -1 || x.NettPrice <= netPriceMax) &&
                (priceMin == -1 || x.Price >= priceMin) &&
                (priceMax == -1 || x.Price <= priceMax) &&
                (weightMin == -1 || x.FoodId >= weightMin) &&
                (weightMax == -1 || x.FoodId <= weightMax) &&
                (productionTimeMin == -1 || x.ProductionTime >= productionTimeMin) &&
                (productionTimeMax == -1 || x.ProductionTime <= productionTimeMax) &&
                (ingredientFilter == null || x.Ingredients.Exists(y =>
                    (ingredientIdMin == -1 || y.IngredientId >= ingredientIdMin) &&
                    (ingredientIdMax == -1 || y.IngredientId <= ingredientIdMax) &&
                    (ingredientName == "" || y.IngredientName.Contains(ingredientName)) &&
                    (unitOfMeasure == UnitOfMeasure.NONE || y.UnitOfMeasureType == unitOfMeasure)
                    ))
            );
            Console.WriteLine($"Znaleziono {filter.Count} pasujących produktów:");
            ShowFood(filter);
        }


        public MarioResult GetIngredients()
        {
            var foodId = ViewHelper.AskForInt("Podaj id produktu dla którego chcesz sprawdzić składniki:");
            if (_foodRepository.Exists(foodId))
            {
                return new MarioResult { Message = $"Nie znaleziono produktu o id {foodId}", Success = false };
            }
            ShowIngredients(new List<Ingredient>());
            Console.Read();
            return new MarioResult { Success = true };
        }        
    }
}
