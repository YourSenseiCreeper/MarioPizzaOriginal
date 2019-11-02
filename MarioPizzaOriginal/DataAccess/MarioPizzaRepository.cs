﻿
using MarioPizzaOriginal.Model;
using MarioPizzaOriginal.Model.Enums;
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

        public MarioPizzaRepository(bool staticOrDynamic)
        {
            _StaticData = staticOrDynamic;
            if (_StaticData)
            {
                _marioPizzaData = new MarioPizzaData
                {
                    PizzaList = new List<Pizza>(),
                    KebabList = new List<Kebab>(),
                    TortillaList = new List<Tortilla>(),
                    DrinkList = new List<Drink>(),
                    OrderList = new List<MarioPizzaOrder>()
                };
            }
            else
            {
                string dataText = Convert.ToBase64String(Resources.sampledata);
                _marioPizzaData = JsonConvert.DeserializeObject<MarioPizzaData>(dataText);
            }
        }
        public void DeleteKebab(int foodId)
        {
            _marioPizzaData.KebabList.RemoveAll(x => x.FoodId != foodId);
        }

        public void DeletePizza(int foodId)
        {
            _marioPizzaData.PizzaList.RemoveAll(x => x.FoodId != foodId);
        }

        public void DeleteTortilla(int foodId)
        {
            _marioPizzaData.TortillaList.RemoveAll(x => x.FoodId != foodId);
        }

        public void EditKebab(Kebab editedKebab)
        {
            var actualKebab = _marioPizzaData.KebabList.First(x => x.FoodId == editedKebab.FoodId);
            if (actualKebab != null)
            {
                var editIndex = _marioPizzaData.KebabList.IndexOf(actualKebab);
                _marioPizzaData.KebabList[editIndex] = editedKebab;
            }
        }

        public void EditPizza(Pizza editedPizza)
        {
            var actualpizza = _marioPizzaData.PizzaList.First(x => x.FoodId == editedPizza.FoodId);
            if (actualpizza != null)
            {
                var editIndex = _marioPizzaData.PizzaList.IndexOf(actualpizza);
                _marioPizzaData.PizzaList[editIndex] = editedPizza;
            }
        }

        public void EditTortilla(Tortilla editedTortilla)
        {
            var actualtortilla = _marioPizzaData.TortillaList.First(x => x.FoodId == editedTortilla.FoodId);
            if (actualtortilla != null)
            {
                var editIndex = _marioPizzaData.TortillaList.IndexOf(actualtortilla);
                _marioPizzaData.TortillaList[editIndex] = editedTortilla;
            }
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

        public void AddIngredient(string ingredientName, UnitOfMeasure unitOfMeasure, double amoutOfUOM)
        {
            if(_marioPizzaData.IngredientList.First(x => x.IngredientName.Equals(ingredientName)) == null){
                var ingredient = new Ingredient
                {
                    IngredientId = _marioPizzaData.IngredientList.Count,
                    IngredientName = ingredientName,
                    UnitOfMeasureType = unitOfMeasure,
                    AmoutOfUOM = amoutOfUOM
                };
                _marioPizzaData.IngredientList.Add(ingredient);
            }
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
            if (actualIngredient != null)
            {
                var editIndex = _marioPizzaData.IngredientList.IndexOf(actualIngredient);
                _marioPizzaData.IngredientList[editIndex] = editedIngredient;
            }
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

        public void AddOrder(Dictionary<FoodSizeSauce, double> orderList, OrderPriority orderPriority)
        {
            var order = new MarioPizzaOrder
            {
                OrderId = _marioPizzaData.OrderList.Count,
                OrderList = orderList,
                Priority = orderPriority
            };
            _marioPizzaData.OrderList.Add(order);
        }

        public void AddElementToOrder(int orderId, FoodSizeSauce element, double quantity)
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

        public void DeleteElementFromOrder(int orderId, string elementName)
        {
            var orders = _marioPizzaData.OrderList.First(x => x.OrderId == orderId).OrderList;
            var elementToRemove = orders.Keys.First(x => x.FoodName.Equals(elementName));
            orders.Remove(elementToRemove);
        }

        public void DeleteElementFromOrder(int orderId, int foodId)
        {
            var orders = _marioPizzaData.OrderList.First(x => x.OrderId == orderId).OrderList;
            var elementToRemove = orders.Keys.First(x => x.FoodId == foodId);
            orders.Remove(elementToRemove);
        }
    }
}
