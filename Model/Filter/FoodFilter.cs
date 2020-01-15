using System;

namespace MarioPizzaOriginal.Filter
{
    public class FoodFilter
    {
        public int? FoodIdMin { get; set; }
        public int? FoodIdMax { get; set; }
        public string FoodName { get; set; }
        public double? NettPriceMin { get; set; }
        public double? NettPriceMax { get; set; }
        public double? PriceMin { get; set; }
        public double? PriceMax { get; set; }
        public double? WeightMin { get; set; }
        public double? WeightMax { get; set; }
        public int? ProductionTimeMin { get; set; }
        public int? ProductionTimeMax { get; set; }
    }
}
