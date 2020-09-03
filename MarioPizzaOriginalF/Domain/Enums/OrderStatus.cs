using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain.Enums
{
    [EnumAsInt]
    public enum OrderStatus : int
    {
        WAITING,
        IN_PROGRESS,
        DELIVERY,
        DONE
    }

}
