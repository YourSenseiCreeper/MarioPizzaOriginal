using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class Ingredient
    {
        [Index]
        [AutoIncrement]
        public int IngredientId { get; set; }
        [Required]
        public string IngredientName { get; set; }
        [Required]
        public UnitOfMeasure UnitOfMeasureType { get; set; }
        public double? PriceSmall { get; set; }
        public double? PriceMedium { get; set; }
        public double? PriceLarge { get; set; }

    }
}