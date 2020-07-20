using Model.DataAccess;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;

namespace Model
{
    public class DomainStartup
    {
        public DomainStartup(TinyIoCContainer container)
        {
            var db = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString, SqliteDialect.Provider);

            container.Register<IFoodRepository, FoodRepository>(new FoodRepository(db));
            container.Register<IFoodIngredientRepository, FoodIngredientRepository>(new FoodIngredientRepository(db));
            container.Register<IIngredientRepository, IngredientRepository>(new IngredientRepository(db));
            container.Register<IOrderRepository, OrderRepository>(new OrderRepository(db));
            container.Register<IOrderElementRepository, OrderElementRepository>(new OrderElementRepository(db));
            container.Register<IOrderSubElementRepository, OrderSubElementRepository>(new OrderSubElementRepository(db));
        }
    }
}
