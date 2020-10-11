using System;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public interface IRepository<T>
    {
        T Get(int id);
        T Get(Expression<Func<T, bool>> condition, bool references=false);
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T, bool>> condition, bool references=false);
        void Add(T newOne);
        void Save(T editOne);
        void Remove(int id);
        void Remove(Expression<Func<T, bool>> condition);
        bool Exists(int id);
        bool Exists(Expression<Func<T, bool>> condition);
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
        Food GetFoodWithIngredients(int foodId);
        double CalculatePriceForFood(int foodId);
    }
    public interface IOrderRepository : IRepository<Order> 
    {
        List<Order> GetByStatus(OrderStatus status);
        double CalculatePriceForOrder(int orderId);
        Order GetOrderWithAllElements(int orderId);
        void DeleteOrderWithAllElements(int orderId);
    }
    public interface IOrderElementRepository : IRepository<OrderElement> 
    {
        void AddToOrder(int orderId, int foodId, double quantity);
        void RemoveFromOrder(int orderId, int foodId);
        List<OrderElement> GetElements(int orderId);
        bool IsElementInOrder(int orderId, int orderElementId);
    }
    public interface IOrderSubElementRepository : IRepository<OrderSubElement> 
    {
        List<OrderSubElement> GetSubElements(int orderElementId);
    }

    public interface IUserRepository : IRepository<User>
    {
        User Authenticate(string username, string passwordHash);
        bool UserExists(string username);
        void Register(string username, string passwordHash);
        void Logout(string username);
        bool IsPasswordCorrect(string username, string passwordHash);
    }

    public interface IRoleRepository : IRepository<Role>
    {
        List<string> GetPrivileges(int roleId);
        void UpdateDefaultRoles();
    }
}
