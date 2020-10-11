using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Account;
using Newtonsoft.Json;
using NLog;
using ServiceStack;
using ServiceStack.OrmLite;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        // Override due to JSON to List mapping
        public new Role Get(int id)
        {
            var dto = connection.Open().SingleById<RoleDto>(id);
            return Role.FromDto(dto);
        }

        public new void Save(Role role)
        {
            using (var dbConn = connection.Open())
            {
                dbConn.Save(Role.ToDto(role));
            }
        }

        public List<string> GetPrivileges(int roleId)
        {
            return new List<string>();
        }

        public void UpdateDefaultRoles()
        {
            rightsTemplates.ForEach(UpdateRights);
        }

        private void UpdateRights(string roleName, IEnumerable<string> hardcodedRights)
        {
            var roleInDatabase = Get(r => r.Name == roleName);
            if (roleInDatabase != null)
            {
                var databaseRoleRightsJson = JsonConvert.SerializeObject(roleInDatabase.Privileges);
                var databaseRoleRightsHash = Util.ToSHA256String(databaseRoleRightsJson);
                var hardcodedRightsJson = JsonConvert.SerializeObject(hardcodedRights);
                var hardcodedRightsJsonHash = Util.ToSHA256String(hardcodedRightsJson);
                if (hardcodedRightsJsonHash != databaseRoleRightsHash)
                {
                    roleInDatabase.Privileges = new List<string>(hardcodedRights);
                    Save(roleInDatabase);
                    Log.Info("Prawa dla roli {RoleName} zostały zaktualizowane!", roleName);
                }
                else
                {
                    Log.Info("Prawa dla roli {RoleName} są aktualne!", roleName);
                }
            }
            else
            {
                Add(new Role
                {
                    Name = roleName,
                    Privileges = new List<string>(hardcodedRights),
                    LastUpdateDate = DateTime.Now
                });
            }
        }

        private Dictionary<string, IEnumerable<string>> rightsTemplates =
            new Dictionary<string, IEnumerable<string>>
            {
                {"ROOT", Rights.All},
                {"BASIC", Rights.Basic}
            };

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    }
}
