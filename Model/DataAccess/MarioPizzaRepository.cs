using ServiceStack.OrmLite;
using System.Configuration;
using System.IO;

namespace Model.DataAccess
{
    public class MarioPizzaRepository : IMarioPizzaRepository
    {
        public IFoodRepository FoodRepository { get; }
        public IIngredientRepository IngredientRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderElementRepository OrderElementRepository { get; }
        public IOrderSubElementRepository OrderSubElementRepository { get; }
        private readonly OrmLiteConnectionFactory db;
        public MarioPizzaRepository()
        {
            db = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString, SqliteDialect.Provider);
            //string path = Directory.GetCurrentDirectory();
            //db = new OrmLiteConnectionFactory($"Data Source ={@"C:\Users\ARKADIUSZ\source\repos\MarioPizzaOriginal\MarioPizza.db"}; Version = 3;", SqliteDialect.Provider);

            FoodRepository = new FoodRepository(db);
            IngredientRepository = new IngredientRepository(db);
            OrderElementRepository = new OrderElementRepository(db);
            OrderSubElementRepository = new OrderSubElementRepository(db);
            OrderRepository = new OrderRepository(db);
        }
    }
}
