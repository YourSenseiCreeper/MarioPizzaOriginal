using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain.Enums
{
    [EnumAsInt]
    public enum PaymentMethod
    {
        CASH,TRANSFER,PAYPAL,BLIK,NOT_DEFINED,NONE
    }
}
