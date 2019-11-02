using MarioPizzaOriginal.DataAccess;
using MarioPizzaOriginal.Model;
using MarioPizzaOriginal.Model.Enums;
using MarioPizzaOriginal.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarioPizzaOriginal.Controller
{
    public class FoodSizeSauceController : IFoodDisposable, IPreparable
    {
        private readonly IMarioPizzaRepository _marioPizzaRepository;
        public FoodSizeSauceController(IMarioPizzaRepository marioPizzaRepository)
        {
            _marioPizzaRepository = marioPizzaRepository;
        }

        public List<string> GetIngredients(int foodId)
        {
            Pizza singlePizza = _marioPizzaRepository.GetPizza(foodId);
            List<string> formatted = new List<string>();
            var index = 1;

            singlePizza.Ingredients.ForEach(ing => formatted.Add($"{index++}. {ing.AmoutOfUOM} {ing.UnitOfMeasureType.ToString()}"));
            return formatted;
        }

        public FoodSize FoodSizeType { get; set; }
        public List<Ingredient> SauceList { get; set; }
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public double NettPrice { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public double ProductionTime { get; set; }

        public void ChangeStatus(OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<string> GetPreparationInstructions()
        {
            throw new NotImplementedException();
        }

        public OrderStatus GetStatus()
        {
            throw new NotImplementedException();
        }

        public void Prepare()
        {
            throw new NotImplementedException();
        }

        public void TakeOrder()
        {
            throw new NotImplementedException();
        }

        public List<Ingredient> GetIngredients()
        {
            throw new NotImplementedException();
        }
    }
}
