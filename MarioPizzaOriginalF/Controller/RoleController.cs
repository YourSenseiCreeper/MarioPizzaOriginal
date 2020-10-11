using MarioPizzaOriginal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using MarioPizzaOriginal.Account;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Tools;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class RoleController
    {
        private readonly IRoleRepository _roleRepository;
        private readonly MenuCreator _roleMenu;
        private readonly ViewHelper _viewHelper;
        private readonly IConsole _console;
        public RoleController()
        {
            var container = TinyIoCContainer.Current;
            _console = container.Resolve<IConsole>();
            _viewHelper = new ViewHelper(_console);
            _roleRepository = container.Resolve<IRoleRepository>();
            _roleMenu = MenuCreator.Create()
                .SetHeader("Dostępne opcje - role: ")
                .AddOptionRange(new Dictionary<string, Action>
                {
                    {"Lista wszystkich ról", GetAllRoles},
                    {"Pokaż uprawnienia dla roli", GetRolePrivileges},
                    {"Dodaj nową rolę", AddNewRole},
                    {"Zarządzaj prawami dla roli", ManagePrivileges},
                    // {"Zapisz obecne prawa jako domyślne dla roli", SaveCurrentPrivileges}
                })
                .AddFooter("Powrót");
        }

        public void RoleMenu() => _roleMenu.Present();

        public void AddNewRole()
        {
            var newRole = new Role
            {
                Name = _viewHelper.AskForStringNotBlank("Podaj nazwę nowej roli: "),
                LastUpdateDate = DateTime.Now,
                Privileges = _viewHelper.AskForListFromPrepared("Wszystkie dostępne prawa:", "Podaj nową wartość: ",
                    input => input, Rights.All.ToList(), "Dodaj prawo", "Usuń prawo")
            };

            _roleRepository.Add(newRole);
            _console.WriteLine($"Dodano rolę: {newRole.Name}");
            _viewHelper.WriteAndWait("Z uprawnieniami: "+string.Join(", ", newRole.Privileges));
        }

        public void GetAllRoles()
        {
            _console.Clear();
            _console.WriteLine("Lista wszystkich ról: ");
            ShowRoles(_roleRepository.GetAll());
            _console.ReadLine();
        }

        // public void SaveCurrentPrivileges()
        // {
        //     Console.Clear();
        //     var currentUser = TinyIoCContainer.Current.Resolve<User>("currentUser");
        //     var currentRole = new Role{ 
        //         Name = currentUser.AccountType.ToString(), 
        //         Privileges = currentUser.BaseRights.Permissions
        //     };
        //     DescribeRole(currentRole);
        //     if (ViewHelper.AskForYesNo("Chcesz zapisać obecną rolę w bazie?"))
        //         _roleRepository.Save(currentRole);
        // }

        public void ManagePrivileges()
        {
            if (RoleNotExistsElseOut("Podaj id lub nazwę roli do której chcesz edytować uprawnienia: ", out var selectedRole))
                return;

            selectedRole.Privileges = _viewHelper.AskForListFromPrepared("Wszystkie dostępne prawa:", "Podaj nową wartość: ",
                input => input, Rights.All.ToList(), "Dodaj prawo", "Usuń prawo", selectedRole.Privileges);

            _roleRepository.Save(selectedRole);
        }

        public void GetRolePrivileges()
        {
            if (RoleNotExistsElseOut("Podaj id lub nazwę roli: ", out var currentRole)) 
                return;

            DescribeRole(currentRole);
        }

        private void ShowRoles(List<Role> roles)
        {
            var header = $"{"Id",5}|{"Nazwa",15}|{"Prawa",15}|{"Ostatnia modyfikacja",15}";
            _console.WriteLine(new string('=', header.Length));
            roles.ForEach(x =>
            {
                _console.WriteLine($"{x.RoleId,5}|{x.Name,15}|{"...",15}|{x.LastUpdateDate,15}");
            });
        }

        private void DescribeRole(Role role)
        {
            var text = new List<string>
            {
                $"Id: {role.RoleId}",
                $"Nazwa: {role.Name}",
                $"Ostatnia modyfikacja: {role.LastUpdateDate}",
                $"Prawa: {string.Join(", ", role.Privileges)}"
            };
            text.ForEach(_console.WriteLine);
            _console.ReadLine();
        }
        private bool RoleNotExistsElseOut(string message, out Role role)
        {
            role = null;
            var idOrName = _viewHelper.AskForStringNotBlank(message);
            if (int.TryParse(idOrName, out var roleId))
            {
                role = _roleRepository.Get(roleId);
                if (role != null) 
                    return false;
                _viewHelper.WriteAndWait($"Rola o numerze {roleId} nie istnieje!");
                return true;
            }

            role = _roleRepository.Get(r => r.Name == idOrName);
            if (role != null) 
                return false;
            _viewHelper.WriteAndWait($"Rola o nazwie '{idOrName}' nie istnieje!");
            return true;
        }
    }
}
