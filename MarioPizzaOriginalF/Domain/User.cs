using System;
using MarioPizzaOriginal.Account;
using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class User
    {
        [Index]
        [AutoIncrement]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public AccountType AccountType { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public bool IsLogged { get; set; }
        public DateTime LastLogin { get; set; }
        [Ignore]
        public BaseRights Permissions { get; set; }

    }
}
