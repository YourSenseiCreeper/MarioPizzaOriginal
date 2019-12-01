using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using System.Collections.Generic;

namespace MarioPizzaOriginal.DataAccess
{
    public interface IMarioPizzaRepository
    {
        //Pizza============================
        //void AddPizza(Pizza pizza);
        //Pizza GetPizza(int foodId);
        //void EditPizza(Pizza editedPizza);
        //void DeletePizza(int foodId);
        //Kebab=============================
        //void AddKebab(Kebab kebab);
        //Kebab GetKebab(int foodId);
        //void EditKebab(Kebab editedKebab);
        //void DeleteKebab(int foodId);
        //Tortilla==========================
        //void AddTortilla(Tortilla tortilla);
        //Tortilla GetTortilla(int foodId);
        //void EditTortilla(Tortilla editedTortilla);
        //void DeleteTortilla(int foodId);
        //Drink============================
        //void AddDrink(Drink drink);
        //Drink GetDrink(int foodId);
        //void EditDrink(Drink editedDrink);
        //void DeleteDrink(int foodId);
        //Ingredient=======================
        void AddIngredient(Ingredient ingredient);
        Ingredient GetIngredient(int ingredientId);
        List<Ingredient> GetAllIngredients();
        List<Ingredient> GetIngredientsForFood(int foodId);
        void EditIngredient(Ingredient editedIngredient);
        bool DeleteIngredient(int ingredientId);
        //Order============================
        MarioPizzaOrder GetOrder(int orderId);
        List<MarioPizzaOrder> GetAllOrders();
        Dictionary<Food, double> GetOrderElements(int orderId);
        bool OrderExists(int orderId);
        void AddOrder(MarioPizzaOrder order);
        void AddElementToOrder(int orderId, Food element, double quantity);
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
        void SaveData();
    }
}
