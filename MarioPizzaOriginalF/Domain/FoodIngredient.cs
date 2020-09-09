using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class FoodIngredient
    {
        [AutoIncrement]
        public int FoodIngredientId { get; set; }
        public int FoodId { get; set; }
        [Reference]
        public Food Food { get; set; }
        public int IngredientId { get; set; }
        [Reference]
        public Ingredient Ingredient { get; set; }
        public double IngredientAmount { get; set; }
    }
}
