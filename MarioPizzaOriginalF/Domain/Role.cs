using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;

namespace MarioPizzaOriginal.Domain
{
    public class Role
    {
        [AutoIncrement]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public List<string> Privileges { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public static Role FromDto(RoleDto dto) =>  new Role
        {
            RoleId = dto.RoleId,
            Name = dto.Name,
            Privileges = dto.Privileges == null ? new List<string>() : 
                JsonConvert.DeserializeObject<List<string>>(dto.Privileges),
            LastUpdateDate = dto.LastUpdateDate
        };

        public static RoleDto ToDto(Role role) => new RoleDto
        {
            RoleId = role.RoleId,
            Name = role.Name,
            Privileges = $"[{string.Join(",",role.Privileges.Select(priv => $"'{priv}'"))}]",
            LastUpdateDate = role.LastUpdateDate
        };

        public RoleDto ToDto() => ToDto(this);
    }
}
