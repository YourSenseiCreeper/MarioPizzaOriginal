using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MarioPizzaOriginal.Account;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.DataAccess;
using ServiceStack;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class UserController
    {
        private readonly IUserRepository _userRepository;
        private readonly TinyIoCContainer _container;
        private readonly MenuCreator _userMenu;
        private readonly MenuCreator _preloginMenu;

        public UserController(TinyIoCContainer container)
        {
            _container = container;
            _userRepository = container.Resolve<IUserRepository>();
            _preloginMenu = MenuCreator.Create()
                .SetHeader("Zaloguj się lub zarejestruj")
                .AddOptionRange(new Dictionary<string, Action>
                {
                    {"Zaloguj się", Login}, // redirect do głównego menu
                    {"Zarejestruj", Register}
                })
                .AddFooter("Wyjdź");
            _userMenu = MenuCreator.Create()
                .SetHeader("Dostępne opcje - użytkownik")
                .AddOptionRange(new Dictionary<string, Action>
                {
                    {"Pokaż dane o zalogowanym użytkowniku", ShowCurrentUserInfo},
                    {"Pokaż wszystkie konta", ShowAllAccounts},
                    {"Pokaż szczegóły użytkownika", ShowAccountInfo},
                    {"Zmień hasło", ResetPassword},
                    {"Wyloguj", Logout}
                })
                .AddFooter("Powrót");
        }

        public void UserAuthentication()
        {
            _preloginMenu.Present();
            var currentUser = _container.Resolve<User>("CurrentUser");
            if (!currentUser.IsLogged) Environment.Exit(0);
        }

        public void UserMenu() => _userMenu.Present();

        public void Register()
        {
            string username = ViewHelper.AskForStringNotBlank("Podaj nazwę użytkownika (nie może być pusta!): ");
            bool notUnique = _userRepository.UserExists(username);
            if (notUnique)
            {
                ViewHelper.WriteAndWait($"Użytkownik '{username}' już istnieje. Wybierz inną nazwę użytkownika!");
                return;
            }

            var hashBytes = SHA256.Create().ComputeHash(ViewHelper.AskForPassword("Podaj hasło dla konta: ", null).ToUtf8Bytes());
            var passwordHash = ConvertSHAToString(hashBytes);
            _userRepository.Register(username, passwordHash);
            ViewHelper.WriteAndWait("\nRejestracja zakończona pomyślnie!");
        }

        public void ResetPassword()
        {
            var hashBytes = SHA256.Create().ComputeHash(ViewHelper.AskForPassword("Podaj hasło dla konta: ", null).ToUtf8Bytes());
            var passwordHash = ConvertSHAToString(hashBytes);
            var currentUser = _container.Resolve<User>("CurrentUser");
            ;
            if (currentUser.PasswordHash != passwordHash)
            {
                ViewHelper.WriteAndWait("Wpisano niepoprawne hasło!");
                return;
            }
            hashBytes = SHA256.Create().ComputeHash(ViewHelper.AskForPassword("Podaj NOWE hasło dla konta: ", null).ToUtf8Bytes());
            var firstPasswordHash = ConvertSHAToString(hashBytes);

            hashBytes = SHA256.Create().ComputeHash(ViewHelper.AskForPassword("Powtórz NOWE hasło dla konta: ", null).ToUtf8Bytes());
            var secondPasswordHash = ConvertSHAToString(hashBytes);

            if (firstPasswordHash != secondPasswordHash)
            {
                ViewHelper.WriteAndWait("Wpisano dwa różne hasła!");
                return;
            }

            currentUser.PasswordHash = firstPasswordHash;
            _userRepository.Save(currentUser);
            ViewHelper.WriteAndWait("Hasło zostało zmienione!");
        }

        public void Login()
        {
            string username = ViewHelper.AskForStringNotBlank("Podaj nazwę użytkownika: ");
            var hashBytes = SHA256.Create().ComputeHash(ViewHelper.AskForPassword("Podaj hasło dla konta: ", null).ToUtf8Bytes());
            var passwordHash = ConvertSHAToString(hashBytes);
            var user = _userRepository.Authenticate(username, passwordHash);

            if (!user.IsLogged)
            {
                ViewHelper.WriteAndWait("Niepoprawne hasło lub użytkownik nie istnieje!");
                return;
            }
            user.Permissions = BaseRights.GetAccountPermissions(user);

            _container.Register(user, "CurrentUser");
            ViewHelper.WriteAndWait($"\nPomyślnie zalogowałeś się na konto '{username}'!");
        }

        public void Logout()
        {
            var user = _container.Resolve<User>("CurrentUser");
            user.IsLogged = false;
            _userRepository.Save(user);
            // redirect do menu z logowaniem?
            UserAuthentication();
        }

        public void ShowCurrentUserInfo()
        {
            var user = _container.Resolve<User>("CurrentUser");
            var info = new List<string>
            {
                $"Id: #{user.UserId}", $"Nazwa użytkownika: {user.Username}", $"Typ konta: {user.AccountType}",
                $"Konto utworzone dnia: {user.CreationTime}",  $"Ostatnie logowanie: {user.LastLogin}"
            };
            info.ForEach(Console.WriteLine);
            Console.ReadLine();
        }

        public void ShowAllAccounts()
        {
            ShowAccounts(_userRepository.GetAll());
        }

        public void ShowAccountInfo()
        {
            var userId = ViewHelper.AskForInt("Podaj id użytkownika: ");
            if (!_userRepository.Exists(userId))
            {
                ViewHelper.WriteAndWait($"Użytkownik o id {userId} nie istnieje!");
                return;
            }
            ShowOneUser(_userRepository.Get(userId));
        }

        private void ShowOneUser(User user)
        {
            var info = new List<string>
            {
                $"Id: #{user.UserId}", $"Nazwa użytkownika: {user.Username}", $"Typ konta: {user.AccountType}",
                $"Konto utworzone dnia: {user.CreationTime}",  $"Ostatnie logowanie: {user.LastLogin}"
            };
            Console.Clear();
            info.ForEach(Console.WriteLine);
            Console.ReadLine();
        }

        private void ShowAccounts(List<User> userList)
        {
            Console.Clear();
            var header = $"{"Id",5} | {"Username",15}| {"Typ konta",10}| {"Data utworzenia", 15}| {"Ostatnie logowanie", 15}";
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));
            userList.ForEach(x => {
                Console.WriteLine($"{x.UserId,5}| {x.Username,15}| {x.AccountType,10} | {x.CreationTime, 15}| {x.LastLogin, 15}");
            });
            ViewHelper.WriteAndWait($"Znaleziono {userList.Count} pasujących produktów:");
            Console.ReadLine();
        }

        private string ConvertSHAToString(byte[] array)
        {
            var builder = new StringBuilder();
            foreach (var t in array)
            {
                builder.Append($"{t:X2}");
            }

            return builder.ToString().ToLower();
        }
    }
}
