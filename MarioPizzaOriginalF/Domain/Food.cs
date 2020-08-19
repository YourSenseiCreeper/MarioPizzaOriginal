using ServiceStack.DataAnnotations;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Domain
{
    public class Food
    {
        [AutoIncrement]
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        [Reference]
        public List<FoodIngredient> Ingredients { get; set; }
        public double NettPrice { get; set; }
        public double Price { get; set; }
        public double? Weight { get; set; }
        public int? ProductionTime { get; set; }
        /*
        public double? PriceSmall { get; set; }
        public double? PriceMedium { get; set; }
        public double? PriceLarge { get; set; }
        */
    }
}
