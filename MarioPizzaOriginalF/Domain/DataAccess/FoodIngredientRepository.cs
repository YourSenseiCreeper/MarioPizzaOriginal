using ServiceStack.OrmLite;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class FoodIngredientRepository : BaseRepository<FoodIngredient>, IFoodIngredientRepository
    {
        public void DeleteFoodIngredients(int foodId)
        {
            connection.Open().Delete<FoodIngredient>(x => x.FoodId == foodId);
        }
    }
}
