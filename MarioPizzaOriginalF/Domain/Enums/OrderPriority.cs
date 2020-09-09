using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain.Enums
{
    [EnumAsInt]
    public enum OrderPriority
    {
        LOW,
        NORMAL,
        URGENT,
        HIGH
    }
}
