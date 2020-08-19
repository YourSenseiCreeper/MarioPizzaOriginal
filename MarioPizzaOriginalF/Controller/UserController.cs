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
        private readonly Dictionary<string, Action> _menuActions;
        private readonly Dictionary<string, Action> _preloginActions;

        public UserController(TinyIoCContainer container)
        {
            _container = container;
            _userRepository = container.Resolve<IUserRepository>();
            _preloginActions = new Dictionary<string, Action>
            {
                {"Zaloguj się", Login}, // redirect do głównego menu
                {"Zarejestruj", Register}
            };
            _menuActions = new Dictionary<string, Action>
            {
                {"Pokaż dane o zalogowanym użytkowniku", ShowCurrentUserInfo},
                {"Pokaż wszystkie konta", ShowAllAccounts},
                {"Pokaż szczegóły użytkownika", ShowAccountInfo},
                {"Wyloguj", Logout}
            };
        }

        public void UserAuthentication()
        {
            ViewHelper.Menu("Zaloguj się lub zarejestruj", _preloginActions, "Wyjdź");
            var currentUser = _container.Resolve<User>("CurrentUser");
            if (!currentUser.IsLogged) Environment.Exit(0);
        }

        public void UserMenu()
        {
            ViewHelper.Menu("Dostępne opcje - użytkownik", _menuActions, "Powrót");
        }

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

        }

        public void Login()
        {
            string username = ViewHelper.AskForStringNotBlank("Podaj nazwę użytkownika: ");
            var hashBytes = SHA256.Create().ComputeHash(ViewHelper.AskForPassword("Podaj hasło dla konta: ", null).ToUtf8Bytes());
            var passwordHash = ConvertSHAToString(hashBytes);
            bool logged = _userRepository.Authenticate(username, passwordHash);

            if (!logged)
            {
                ViewHelper.WriteAndWait("Niepoprawne hasło lub użytkownik nie istnieje!");
                return;
            }
            var user = _userRepository.GetUser(username);
            user.Permissions = BaseRights.GetAccountPermissions(user);

            _container.Register(user, "CurrentUser");
            ViewHelper.WriteAndWait($"\nPomyślnie zalogowałeś się na konto '{username}'!");
        }

        public void Logout()
        {
            var user = _container.Resolve<User>("CurrentUser");
            _userRepository.Logout(user.Username);
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

        }

        public void ShowAccountInfo()
        {

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
