using System;
using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    // [Schema("MarioPizza")]
    [Alias("Role")]
    public class RoleDto
    {
        [AutoIncrement]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Privileges { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
