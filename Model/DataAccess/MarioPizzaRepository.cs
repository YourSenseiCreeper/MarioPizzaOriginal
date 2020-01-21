using ServiceStack.OrmLite;
using System.Configuration;

namespace Model.DataAccess
{
    public class MarioPizzaRepository : IMarioPizzaRepository
    {
        public IFoodRepository FoodRepository { get; }
        public IIngredientRepository IngredientRepository { get; }
        public IFoodIngredientRepository FoodIngredientRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderElementRepository OrderElementRepository { get; }
        public IOrderSubElementRepository OrderSubElementRepository { get; }
        private readonly OrmLiteConnectionFactory db;
        public MarioPizzaRepository()
        {
            db = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString, SqliteDialect.Provider);
            
            FoodRepository = new FoodRepository(db);
            IngredientRepository = new IngredientRepository(db);
            FoodIngredientRepository = new FoodIngredientRepository(db);
            OrderElementRepository = new OrderElementRepository(db);
            OrderSubElementRepository = new OrderSubElementRepository(db);
            OrderRepository = new OrderRepository(db);
        }
    }
}
