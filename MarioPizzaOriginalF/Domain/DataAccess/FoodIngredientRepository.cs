using ServiceStack.OrmLite;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class FoodIngredientRepository : BaseRepository<FoodIngredient>, IFoodIngredientRepository
    {
        private readonly OrmLiteConnectionFactory db;

        public FoodIngredientRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
        }

        public void DeleteFoodIngredients(int foodId)
        {
            db.Open().Delete<FoodIngredient>(x => x.FoodId == foodId);
        }
    }
}
