using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            FoodRepository = new FoodRepository(db);
            IngredientRepository = new IngredientRepository(db);
            OrderElementRepository = new OrderElementRepository(db);
            OrderSubElementRepository = new OrderSubElementRepository(db);
            OrderRepository = new OrderRepository(db);
        }
    }
}
