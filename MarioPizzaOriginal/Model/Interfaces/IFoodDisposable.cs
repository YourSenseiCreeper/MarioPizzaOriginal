using MarioPizzaOriginal.Model.Enums;

namespace MarioPizzaOriginal.Model
{
    public interface IFoodDisposable
    {
        void TakeOrder();
        void ChangeStatus(OrderStatus orderStatus);
        OrderStatus GetStatus();
        void Dispose();
    }
}
