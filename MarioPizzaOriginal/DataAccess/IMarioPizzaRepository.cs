using MarioPizzaOriginal.Model;
using MarioPizzaOriginal.Model.Enums;
using System.Collections.Generic;

namespace MarioPizzaOriginal.DataAccess
{
    public interface IMarioPizzaRepository
    {
        //Pizza============================
        Pizza GetPizza(int foodId);
        void EditPizza(Pizza editedPizza);
        void DeletePizza(int foodId);
        //Kebab=============================
        Kebab GetKebab(int foodId);
        void EditKebab(Kebab editedKebab);
        void DeleteKebab(int foodId);
        //Tortilla==========================
        Tortilla GetTortilla(int foodId);
        void EditTortilla(Tortilla editedTortilla);
        void DeleteTortilla(int foodId);
        //Ingredient=======================
        void AddIngredient(string ingredientName, UnitOfMeasure unitOfMeasure, double amoutOfUOM);
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
        void AddOrder(Dictionary<FoodSizeSauce, double> orderList, OrderPriority orderPriority);
        void AddElementToOrder(int orderId, FoodSizeSauce element, double quantity);
        void ChangeOrderPriority(int orderId, OrderPriority newOrderPriority);
        void DeleteOrder(int orderId);
        void DeleteElementFromOrder(int orderId, string elementName);
        void DeleteElementFromOrder(int orderId, int foodId);
        void SaveData();
    }
}
