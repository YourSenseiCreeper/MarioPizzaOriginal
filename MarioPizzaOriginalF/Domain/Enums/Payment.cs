using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain.Enums
{
    [EnumAsInt]
    public enum Payment
    {
        AT_PLACE,FOREWARD,DELIVERY,NONE
    }
}
