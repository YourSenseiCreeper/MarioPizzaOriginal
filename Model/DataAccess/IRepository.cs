using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using MarioPizzaOriginal.Filter;
using System;
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
    }

    public interface IIngredientRepository : IRepository<Ingredient>
    {
        List<Ingredient> GetIngredientsForFood(int foodId);
    }
    public interface IFoodRepository : IRepository<Food> 
    {
        string GetName(int foodId);
        List<Tuple<string, double>> GetIngredients(int foodId);
        double CalculatePriceForFood(int foodId);
        List<Food> Filter(FoodFilter filter);
    }
    public interface IOrderRepository : IRepository<MarioPizzaOrder> 
    {
        int OrderNextId();
        List<MarioPizzaOrder> GetByStatus(OrderStatus status);
        double CalculatePriceForOrder(int orderId);
    }
    public interface IOrderElementRepository : IRepository<OrderElement> 
    {
        int OrderElementNextId();
        void AddToOrder(int orderId, int foodId, double quantity);
        void RemoveFromOrder(int orderId, int foodId);
        List<OrderElement> GetElements(int orderId);
    }
    public interface IOrderSubElementRepository : IRepository<OrderSubElement> 
    {
        List<OrderSubElement> GetSubElements(int orderElementId);
    }
}
