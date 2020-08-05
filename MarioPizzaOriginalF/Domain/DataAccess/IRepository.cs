using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using System.Collections.Generic;

namespace Model.DataAccess
{
    public interface IRepository<T>
    {
        T Get(int id);
        void Add(T newOne);
        void Edit(T editOne);
        void Remove(int id);
        List<T> GetAll();
        bool Exists(int id);
        int Count();
        List<T> Query(string queryString);
    }

    public interface IIngredientRepository : IRepository<Ingredient>
    {
        List<Ingredient> GetIngredientsForFood(int foodId);
    }
    public interface IFoodIngredientRepository : IRepository<FoodIngredient>
    {
        void DeleteFoodIngredients(int foodId);
    }
    public interface IFoodRepository : IRepository<Food> 
    {
        string GetName(int foodId);
        Dictionary<string, double> GetIngredients(int foodId);
        double CalculatePriceForFood(int foodId);
    }
    public interface IOrderRepository : IRepository<MarioPizzaOrder> 
    {
        int OrderNextId();
        List<MarioPizzaOrder> GetByStatus(OrderStatus status);
        double CalculatePriceForOrder(int orderId);
        //List<MarioPizzaOrder> Filter(OrderFilter filter);
    }
    public interface IOrderElementRepository : IRepository<OrderElement> 
    {
        int OrderElementNextId();
        void AddToOrder(int orderId, int foodId, double quantity);
        void RemoveFromOrder(int orderId, int foodId);
        List<OrderElement> GetElements(int orderId);
        bool IsElementInOrder(int orderId, int orderElementId);
    }
    public interface IOrderSubElementRepository : IRepository<OrderSubElement> 
    {
        List<OrderSubElement> GetSubElements(int orderElementId);
    }
}
