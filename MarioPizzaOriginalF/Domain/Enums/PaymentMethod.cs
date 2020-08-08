using ServiceStack.DataAnnotations;

namespace Model.Enums
{
    [EnumAsInt]
    public enum PaymentMethod
    {
        CASH,TRANSFER,PAYPAL,BLIK,NOT_DEFINED,NONE
    }
}
