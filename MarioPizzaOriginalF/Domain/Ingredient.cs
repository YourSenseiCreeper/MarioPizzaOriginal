using MarioPizzaOriginal.Domain.Enums;
using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class Ingredient
    {
        [AutoIncrement]
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        [Reference]
        public UnitOfMeasure UnitOfMeasureType { get; set; }
        public double? PriceSmall { get; set; }
        public double? PriceMedium { get; set; }
        public double? PriceLarge { get; set; }

    }
}