namespace MarioPizzaOriginal.Domain
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public UnitOfMeasure UnitOfMeasureType { get; set; }
        public double? PriceSmall { get; set; }
        public double? PriceMedium { get; set; }
        public double? PriceLarge { get; set; }

    }
}