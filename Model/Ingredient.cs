namespace MarioPizzaOriginal.Domain
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public UnitOfMeasure UnitOfMeasureType { get; set; }
        public double AmountOfUOM { get; set; }
    }
}