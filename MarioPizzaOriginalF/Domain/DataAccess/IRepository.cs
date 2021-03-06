﻿using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using System.Collections.Generic;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public interface IRepository<T>
    {
        T Get(int id);
        T GetWithReferences(int id);
        void Add(T newOne);
        void Save(T editOne);
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
        User GetUser(string username);
        void Logout(string username);
        bool IsPasswordCorrect(string username, string passwordHash);
    }
}
