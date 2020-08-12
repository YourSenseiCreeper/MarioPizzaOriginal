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
        [Ignore]
        public AccountType AccountType { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        [Ignore]
        public bool IsLogged { get; set; }
        [Ignore]
        public DateTime LastLogin { get; set; }

    }
}
