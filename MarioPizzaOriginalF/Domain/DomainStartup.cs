using ServiceStack.OrmLite;
using System.Configuration;
using MarioPizzaOriginal.Domain.DataAccess;
using TinyIoC;
using System;
using ServiceStack.Data;

namespace MarioPizzaOriginal.Domain
{
    public class DomainStartup
    {
        private readonly OrmLiteConnectionFactory db;
        private readonly TinyIoCContainer container;
        public DomainStartup()
        {
            db = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString, SqliteDialect.Provider);
            container = TinyIoCContainer.Current;
            container.Register<IDbConnectionFactory>(db);
            RegisterRepository<FoodRepository, IFoodRepository, IRepository<Food>>();
            RegisterRepository<FoodIngredientRepository, IFoodIngredientRepository, IRepository<FoodIngredient>>();
            RegisterRepository<IngredientRepository, IIngredientRepository, IRepository<Ingredient>>();
            RegisterRepository<OrderRepository, IOrderRepository, IRepository<Order>>();
            RegisterRepository<OrderElementRepository, IOrderElementRepository, IRepository<OrderElement>>();
            RegisterRepository<OrderSubElementRepository, IOrderSubElementRepository, IRepository<OrderSubElement>>();
            RegisterRepository<UserRepository, IUserRepository, IRepository<User>>();
        }

        private void RegisterRepository<T, TK, TV>() 
            where TK : class where TV : class where T : class, TV, TK
        {
            container.Register<TK, T>();
            container.Register<TV, T>();
        }
    }
}
