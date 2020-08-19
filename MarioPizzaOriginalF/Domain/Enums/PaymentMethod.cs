using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain.Enums
{
    [EnumAsInt]
    public enum PaymentMethod : int
    {
        CASH,TRANSFER,PAYPAL,BLIK,NOT_DEFINED,NONE
    }
}
