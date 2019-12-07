using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using Model;
using System.Collections.Generic;

namespace MarioPizzaOriginal.DataAccess
{
    public interface IMarioPizzaRepository
    {
        void AddIngredient(Ingredient ingredient);
        Ingredient GetIngredient(int ingredientId);
        List<Ingredient> GetAllIngredients();
        List<Ingredient> GetIngredientsForFood(int foodId);
        void EditIngredient(Ingredient editedIngredient);
        bool DeleteIngredient(int ingredientId);
        //Order============================
        MarioPizzaOrder GetOrder(int orderId);
        List<MarioPizzaOrder> GetAllOrders();
        List<MarioPizzaOrder> GetOrdersByStatus(OrderStatus status);
        List<OrderElement> GetOrderElements(int orderId);
        bool OrderExists(int orderId);
        bool OrderElementExists(int orderElementId);
        void AddOrder(MarioPizzaOrder order);
        void AddElementToOrder(int orderId, int foodId, double quantity);
        void ChangeOrderPriority(int orderId, OrderPriority newOrderPriority);
        void ChangeOrderStatus(int orderId, OrderStatus newOrderStatus);
        OrderStatus GetOrderStatus(int orderId);
        void DeleteOrder(int orderId);
        //void DeleteElementFromOrder(int orderId, string elementName);
        void DeleteElementFromOrder(int orderId, int foodId);
        //All pizza, kebab, tortilla and drink
        List<Food> GetAllFood();
        List<Food> GetFilteredFood();
        Food GetFood(int foodId);
        int OrderCount();
        int OrderElementsCount();
        void SaveData();
    }
}
