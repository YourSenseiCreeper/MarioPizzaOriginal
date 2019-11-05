using MarioPizzaOriginal.Model;
using MarioPizzaOriginal.Model.Enums;
using System.Collections.Generic;

namespace MarioPizzaOriginal.DataAccess
{
    public interface IMarioPizzaRepository
    {
        //Pizza============================
        void AddPizza(Pizza pizza);
        Pizza GetPizza(int foodId);
        void EditPizza(Pizza editedPizza);
        void DeletePizza(int foodId);
        //Kebab=============================
        void AddKebab(Kebab kebab);
        Kebab GetKebab(int foodId);
        void EditKebab(Kebab editedKebab);
        void DeleteKebab(int foodId);
        //Tortilla==========================
        void AddTortilla(Tortilla tortilla);
        Tortilla GetTortilla(int foodId);
        void EditTortilla(Tortilla editedTortilla);
        void DeleteTortilla(int foodId);
        //Drink============================
        void AddDrink(Drink drink);
        Drink GetDrink(int foodId);
        void EditDrink(Drink editedDrink);
        void DeleteDrink(int foodId);
        //Ingredient=======================
        void AddIngredient(Ingredient ingredient);
        Ingredient GetIngredient(string ingredientName);
        Ingredient GetIngredient(int ingredientId);
        List<Ingredient> GetIngredientList();
        void EditIngredient(Ingredient editedIngredient);
        bool DeleteIngredient(string ingredientName);
        bool DeleteIngredient(int ingredientId);
        //Order============================
        MarioPizzaOrder GetOrder(int orderId);
        List<MarioPizzaOrder> GetAllOrders();
        bool OrderExists(int orderId);
        void AddOrder(MarioPizzaOrder order);
        void AddElementToOrder(int orderId, FoodSizeSauce element, double quantity);
        void ChangeOrderPriority(int orderId, OrderPriority newOrderPriority);
        void ChangeOrderStatus(int orderId, OrderStatus newOrderStatus);
        void DeleteOrder(int orderId);
        void DeleteElementFromOrder(int orderId, string elementName);
        void DeleteElementFromOrder(int orderId, int foodId);
        //All pizza, kebab, tortilla and drink
        List<FoodSizeSauce> GetAllFood();
        FoodSizeSauce GetFood(int foodId);
        FoodSizeSauce GetFood(string foodName);
        FoodSizeSauce GetFood(string foodName, FoodType foodType);
        void SaveData();
    }
}
