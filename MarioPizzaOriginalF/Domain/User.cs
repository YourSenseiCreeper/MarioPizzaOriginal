using System;
using MarioPizzaOriginal.Account;
using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class User
    {
        [AutoIncrement]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        [Reference]
        public AccountType AccountType { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsLogged { get; set; }
        public DateTime LastLogin { get; set; }
        [Ignore]
        public BaseRights Permissions { get; set; }

        public bool HasPermission(string methodName) => Permissions.HasPermission(methodName);

    }
}
