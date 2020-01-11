using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal
{
    [EnumAsInt]
    public enum UnitOfMeasure
    {
        KILOGRAM,
        LITR,
        SZCZYPTA,
        OPAKOWANIE_S,
        OPAKOWANIE_M,
        OPAKOWANIE_L,
        SLOIK_200G,
        SLOIK_500G,
        SLOIK_1KG,
        NONE
    }
}