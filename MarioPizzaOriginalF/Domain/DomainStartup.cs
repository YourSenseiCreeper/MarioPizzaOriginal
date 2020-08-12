using Model;
using Model.DataAccess;
using ServiceStack.OrmLite;
using System.Configuration;
using MarioPizzaOriginal.Domain.DataAccess;
using TinyIoC;
using System;

namespace MarioPizzaOriginal.Domain
{
    public class DomainStartup
    {
        private OrmLiteConnectionFactory db;
        private TinyIoCContainer container;
        public DomainStartup(TinyIoCContainer container)
        {
            db = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString, SqliteDialect.Provider);
            this.container = container;
            RegisterRepository<FoodRepository, IFoodRepository, IRepository<Food>>(repo => new FoodRepository(repo));
            RegisterRepository<FoodIngredientRepository, IFoodIngredientRepository, IRepository<FoodIngredient>>
                (repo => new FoodIngredientRepository(repo));
            RegisterRepository<IngredientRepository, IIngredientRepository, IRepository<Ingredient>>(repo => new IngredientRepository(repo));
            RegisterRepository<OrderRepository, IOrderRepository, IRepository<Order>>(repo => new OrderRepository(repo));
            RegisterRepository<OrderElementRepository, IOrderElementRepository, IRepository<OrderElement>>(repo => new OrderElementRepository(repo));
            RegisterRepository<OrderSubElementRepository, IOrderSubElementRepository, IRepository<OrderSubElement>>
                (repo => new OrderSubElementRepository(repo));
            RegisterRepository<UserRepository, IUserRepository, IRepository<User>>(repo => new UserRepository(repo));

        }

        private void RegisterRepository<T, K, V>(Func<OrmLiteConnectionFactory, T> repository) 
            where K : class where V : class where T : class, V, K
        {
            container.Register<K, T>(repository(db));
            container.Register<V, T>(repository(db));
        }
    }
}
