using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using MarioPizzaOriginal.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarioPizzaOriginal.DataAccess
{
    public class MarioPizzaRepository : IMarioPizzaRepository
    {
        private readonly bool _StaticData;
        private readonly MarioPizzaData _marioPizzaData;
        private List<Food> allFood;

        public MarioPizzaRepository(bool staticOrDynamic)
        {
            _StaticData = staticOrDynamic;
            if (_StaticData)
            {
                _marioPizzaData = new MarioPizzaData
                {
                    OrderList = new List<MarioPizzaOrder>(),
                    IngredientList = new List<Ingredient>()
                };
            }
            else
            {
                string dataText = Convert.ToBase64String(Resources.sampledata);
                _marioPizzaData = JsonConvert.DeserializeObject<MarioPizzaData>(dataText);
            }
        }
        /*
        public void DeleteKebab(int foodId)
        {
            var tortilla = _marioPizzaData.TortillaList.Find(x => x.FoodId == foodId);
            _marioPizzaData.TortillaList.Remove(tortilla);
            DeleteFood(tortilla);
        }

        public void DeletePizza(int foodId)
        {
            var pizza = _marioPizzaData.PizzaList.Find(x => x.FoodId == foodId);
            _marioPizzaData.PizzaList.Remove(pizza);
            DeleteFood(pizza);
        }

        public void DeleteTortilla(int foodId)
        {
            var tortilla = _marioPizzaData.TortillaList.Find(x => x.FoodId == foodId);
            _marioPizzaData.TortillaList.Remove(tortilla);
            DeleteFood(tortilla);
        }

        public void EditKebab(Kebab editedKebab)
        {
            var actualKebab = _marioPizzaData.KebabList.First(x => x.FoodId == editedKebab.FoodId);
            var editIndex = _marioPizzaData.KebabList.IndexOf(actualKebab);
            _marioPizzaData.KebabList[editIndex] = editedKebab;
            EditFood(editedKebab, editIndex);
        }

        public void EditPizza(Pizza editedPizza)
        {
            var actualpizza = _marioPizzaData.PizzaList.First(x => x.FoodId == editedPizza.FoodId);
            var editIndex = _marioPizzaData.PizzaList.IndexOf(actualpizza);
            _marioPizzaData.PizzaList[editIndex] = editedPizza;
            EditFood(editedPizza, editIndex);
        }

        public void EditTortilla(Tortilla editedTortilla)
        {
            var actualtortilla = _marioPizzaData.TortillaList.First(x => x.FoodId == editedTortilla.FoodId);
            var editIndex = _marioPizzaData.TortillaList.IndexOf(actualtortilla);
            _marioPizzaData.TortillaList[editIndex] = editedTortilla;
            EditFood(editedTortilla, editIndex);
        }

        public Kebab GetKebab(int foodId)
        {
            return _marioPizzaData.KebabList.FirstOrDefault(x => x.FoodId == foodId);
        }

        public Pizza GetPizza(int foodId)
        {
            return _marioPizzaData.PizzaList.FirstOrDefault(x => x.FoodId == foodId);
        }

        public Tortilla GetTortilla(int foodId)
        {
            return _marioPizzaData.TortillaList.FirstOrDefault(x => x.FoodId == foodId);
        }
        */
        public Ingredient GetIngredient(string ingredientName)
        {
            return _marioPizzaData.IngredientList.First(x => x.IngredientName.Equals(ingredientName));
        }

        public Ingredient GetIngredient(int ingredientId)
        {
            return _marioPizzaData.IngredientList.First(x => x.IngredientId == ingredientId);
        }

        public List<Ingredient> GetIngredientList()
        {
            return _marioPizzaData.IngredientList;
        }

        public void AddIngredient(Ingredient ingredient)
        {
            _marioPizzaData.IngredientList.Add(ingredient);
        }

        public bool DeleteIngredient(string ingredientName)
        {
            return _marioPizzaData.IngredientList.RemoveAll(x => x.IngredientName.Equals(ingredientName)) > 0;
        }
        public bool DeleteIngredient(int ingredientId)
        {
            return _marioPizzaData.IngredientList.RemoveAll(x => x.IngredientId == ingredientId) > 0;
        }

        public void EditIngredient(Ingredient editedIngredient)
        {
            var actualIngredient = _marioPizzaData.IngredientList.First(x => x.IngredientId == editedIngredient.IngredientId);
            var editIndex = _marioPizzaData.IngredientList.IndexOf(actualIngredient);
            _marioPizzaData.IngredientList[editIndex] = editedIngredient;
        }

        public void SaveData()
        {
            using (StreamWriter outputFile = new StreamWriter("sampledata.json"))
            {
                outputFile.WriteLine(JsonConvert.SerializeObject(_marioPizzaData));
            }
        }

        public MarioPizzaOrder GetOrder(int orderId)
        {
            return _marioPizzaData.OrderList.First(x => x.OrderId == orderId);
        }

        public List<MarioPizzaOrder> GetAllOrders()
        {
            return _marioPizzaData.OrderList;
        }

        public bool OrderExists(int orderId)
        {
            return _marioPizzaData.OrderList.Exists(x => x.OrderId == orderId);
        }

        public void AddOrder(MarioPizzaOrder order)
        {
            _marioPizzaData.OrderList.Add(order);
        }

        public void AddElementToOrder(int orderId, Food element, double quantity)
        {
            _marioPizzaData.OrderList.First(x => x.OrderId == orderId)?.OrderList.Add(element, quantity);
        }

        public void ChangeOrderPriority(int orderId, OrderPriority newOrderPriority)
        {
            _marioPizzaData.OrderList.First(x => x.OrderId == orderId).Priority = newOrderPriority;
        }

        public void DeleteOrder(int orderId)
        {
            _marioPizzaData.OrderList.RemoveAll(x => x.OrderId == orderId);
        }

        /*public void DeleteElementFromOrder(int orderId, string elementName)
        {
            var orders = _marioPizzaData.OrderList.First(x => x.OrderId == orderId).OrderList;
            var elementToRemove = orders.Keys.First(x => x.FoodName.Equals(elementName));
            orders.Remove(elementToRemove);
        }*/

        public void DeleteElementFromOrder(int orderId, int foodId)
        {
            var orders = _marioPizzaData.OrderList.First(x => x.OrderId == orderId).OrderList;
            var elementToRemove = orders.Keys.First(x => x.FoodId == foodId);
            orders.Remove(elementToRemove);
        }
        /*
        public void AddPizza(Pizza pizza)
        {
            _marioPizzaData.PizzaList.Add(pizza);
            AddNewFood(pizza);
        }

        public Drink GetDrink(int foodId)
        {
            return _marioPizzaData.DrinkList.First(x => x.FoodId == foodId);
        }

        public void EditDrink(Drink editedDrink)
        {
            var actualDrink = _marioPizzaData.DrinkList.First(x => x.FoodId == editedDrink.FoodId);
            var editIndex = _marioPizzaData.DrinkList.IndexOf(actualDrink);
            _marioPizzaData.DrinkList[editIndex] = editedDrink;
        }

        public void DeleteDrink(int foodId)
        {
            var drink = _marioPizzaData.DrinkList.Find(x => x.FoodId == foodId);
            _marioPizzaData.DrinkList.Remove(drink);
            DeleteFood(drink);
        }
        */
        public List<Food> GetAllFood()
        {
            //Can cause big laggs once the database grow bigger
            //Now it's about to be OK
            if (allFood == null)
            {
                allFood = new List<Food>();
            }
            return allFood;
        }

        private void AddNewFood(Food foodSizeSauce)
        {
            allFood.Add(foodSizeSauce);
        }
        private void EditFood(Food foodSizeSauce, int foodIndex)
        {
            allFood[foodIndex] = foodSizeSauce;
        }
        private void DeleteFood(FoodSizeSauce foodSizeSauce)
        {
            allFood.Remove(foodSizeSauce);
        }
        /*
        public void AddKebab(Kebab kebab)
        {
            _marioPizzaData.KebabList.Add(kebab);
            AddNewFood(kebab);
        }

        public void AddTortilla(Tortilla tortilla)
        {
            _marioPizzaData.TortillaList.Add(tortilla);
            AddNewFood(tortilla);
        }

        public void AddDrink(Drink drink)
        {
            _marioPizzaData.DrinkList.Add(drink);
            AddNewFood(drink);
        }
        */
        public void ChangeOrderStatus(int orderId, OrderStatus newOrderStatus)
        {
            var actualOrder = _marioPizzaData.OrderList.First(x => x.OrderId == orderId);
            var index = _marioPizzaData.OrderList.IndexOf(actualOrder);
            _marioPizzaData.OrderList[index].Status = newOrderStatus;
        }

        public OrderStatus GetOrderStatus(int orderId)
        {
            return _marioPizzaData.OrderList.First(x => x.OrderId == orderId).Status;
        }

        public List<Food> GetFilteredFood()
        {
            throw new NotImplementedException();
        }

        public Food GetFood(int foodId)
        {
            return allFood.FirstOrDefault(x => x.FoodId == foodId);
        }

        public List<Ingredient> GetIngredients()
        {
            throw new NotImplementedException();
        }

        public List<Ingredient> GetIngredientsForFood(int foodId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<Food, double> GetOrderElements(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
