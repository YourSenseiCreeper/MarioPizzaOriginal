using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Account
{
    [EnumAsInt]
    public enum AccountType
    {
        NONE,CASHIER,DRIVER,ROOT
    }
}
