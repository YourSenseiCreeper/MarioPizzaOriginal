using System;
using System.Collections.Generic;
using System.Text;

namespace MarioPizzaOriginal.Model
{
    public class Food
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public double NettPrice { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public int ProductionTime { get; set; }
    }
}
