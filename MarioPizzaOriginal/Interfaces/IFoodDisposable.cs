using MarioPizzaOriginal.Domain.Enums;

namespace MarioPizzaOriginal.Domain
{
    public interface IFoodDisposable
    {
        void TakeOrder();
        void ChangeStatus(OrderStatus orderStatus);
        OrderStatus GetStatus();
        void Dispose();
    }
}
