using Model;
using Model.DataAccess;
using ServiceStack.OrmLite;
using System.Configuration;
using TinyIoC;

namespace MarioPizzaOriginal.Domain
{
    public class DomainStartup
    {
        public DomainStartup(TinyIoCContainer container)
        {
            var db = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["SqlLite"].ConnectionString, SqliteDialect.Provider);
            var foodRepository = new FoodRepository(db);
            container.Register<IFoodRepository, FoodRepository>(foodRepository);
            container.Register<IRepository<Food>, FoodRepository>(foodRepository);

            var foodIngredientRepository = new FoodIngredientRepository(db);
            container.Register<IFoodIngredientRepository, FoodIngredientRepository>(foodIngredientRepository);
            container.Register<IRepository<FoodIngredient>, FoodIngredientRepository>(foodIngredientRepository);

            var ingredientRepository = new IngredientRepository(db);
            container.Register<IIngredientRepository, IngredientRepository>(ingredientRepository);
            container.Register<IRepository<Ingredient>, IngredientRepository>(ingredientRepository);

            var orderRepository = new OrderRepository(db);
            container.Register<IOrderRepository, OrderRepository>(orderRepository);
            container.Register<IRepository<MarioPizzaOrder>, OrderRepository>(orderRepository);

            var orderElementRepository = new OrderElementRepository(db);
            container.Register<IOrderElementRepository, OrderElementRepository>(orderElementRepository);
            container.Register<IRepository<OrderElement>, OrderElementRepository>(orderElementRepository);

            var orderSubElementRepository = new OrderSubElementRepository(db);
            container.Register<IOrderSubElementRepository, OrderSubElementRepository>(orderSubElementRepository);
            container.Register<IRepository<OrderSubElement>, OrderSubElementRepository>(orderSubElementRepository);
        }
    }
}
