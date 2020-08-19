
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using MarioPizzaOriginal.Domain.DataAccess;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class FoodController
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IFoodIngredientRepository _foodIngredientRepository;
        private readonly FoodFilter _foodFilter;
        private readonly MenuCreator _foodMenu;
        public FoodController(TinyIoCContainer container)
        {
            _foodRepository = container.Resolve<IFoodRepository>();
            _foodIngredientRepository = container.Resolve<IFoodIngredientRepository>();
            _foodFilter = new FoodFilter(container);
            _foodMenu = MenuCreator.Create()
                .SetHeader("Dostępne opcje - Produkty:")
                .AddOptionRange(new Dictionary<string, Action>
                {
                    {"Lista wszystkich produktów", GetAllFood},
                    {"Szczegóły produktu", GetFood},
                    {"Dodaj produkt", AddFood},
                    {"Usuń produkt", DeleteFood},
                    {"Szukaj wg filtru", GetFilteredFood}
                })
                .AddFooter("Powrót");
        }

        public void FoodMenu() => _foodMenu.Present();
        private List<string> ShowIngredients(Dictionary<string, double> ingredients)
        {
            var formatted = new List<string> { "Składniki: " };
            formatted.AddRange(ingredients.Select((key, value) => $"* {key} {value}"));
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
                FoodName = ViewHelper.AskForStringNotBlank(string.Format(Resource.FoodController_NewFood_step1, step++, maxStep)),
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
            var header = $"{"Id",5} | {"Nazwa",25}| {"Cena",5}|";
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));
            foodList.ForEach(x => {
                Console.WriteLine($"{x.FoodId,5}| {x.FoodName,25}| {x.Price,5} zł");
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
            if (_foodFilter.FilterMenu()) ShowFood(_foodFilter.Query());
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
            return;
        }
    }
}
