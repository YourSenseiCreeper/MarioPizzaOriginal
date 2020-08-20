using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class FoodIngredient
    {
        [AutoIncrement]
        public int FoodIngredientId { get; set; }
        public int FoodId { get; set; }
        [References(typeof(Ingredient))]
        public int IngredientId { get; set; }
        [Reference]
        public Ingredient Ingredient { get; set; }
        public double IngredientAmount { get; set; }
    }
}
