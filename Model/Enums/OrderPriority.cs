using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain.Enums
{
    [EnumAsInt]
    public enum OrderPriority
    {
        NONE,
        LOW,
        NORMAL,
        URGENT,
        HIGH
    }
}
