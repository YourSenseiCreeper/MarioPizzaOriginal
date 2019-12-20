using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccess
{
    public interface IMarioPizzaRepository
    {
        IFoodRepository FoodRepository { get; }
        IIngredientRepository IngredientRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderElementRepository OrderElementRepository { get; }
        IOrderSubElementRepository OrderSubElementRepository { get; }
    }
}
