using System;
using System.Collections.Generic;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Tools;
using NLog;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class UserController
    {
        private readonly IUserRepository _userRepository;
        private readonly TinyIoCContainer _container;
        private readonly MenuCreator _userMenu;
        private readonly MenuCreator _preloginMenu;
        private readonly ViewHelper _viewHelper;
        private readonly IConsole _console;

        public UserController()
        {
            _container = TinyIoCContainer.Current;
            _console = _container.Resolve<IConsole>();
            _viewHelper = new ViewHelper(_console);
            _userRepository = _container.Resolve<IUserRepository>();
            _preloginMenu = MenuCreator.Create()
                .SetHeader("Zaloguj się lub zarejestruj")
                .AddOptionRange(new Dictionary<string, Action>
                {
                    {"Zaloguj się", Login},
                    {"Zarejestruj", Register}
                })
                .AddGoBackAction(Login)
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
            if (!GetCurrentUser().IsLogged) Environment.Exit(0);
        }

        public void UserMenu() => _userMenu.Present();

        public void Register()
        {
            var username = _viewHelper.AskForStringNotBlank("Podaj nazwę użytkownika (nie może być pusta!): ");
            var notUnique = _userRepository.UserExists(username);
            if (notUnique)
            {
                _viewHelper.WriteAndWait($"Użytkownik '{username}' już istnieje. Wybierz inną nazwę użytkownika!");
                return;
            }

            var passwordHash = Util.ToSHA256String(_viewHelper.AskForPassword("Podaj hasło dla konta: "));
            _userRepository.Register(username, passwordHash);
            _viewHelper.WriteAndWait("\nRejestracja zakończona pomyślnie!");
        }

        public void ResetPassword()
        {
            var passwordHash = Util.ToSHA256String(_viewHelper.AskForPassword("Podaj hasło dla konta: "));
            var currentUser = GetCurrentUser();
            ;
            if (currentUser.PasswordHash != passwordHash)
            {
                _viewHelper.WriteAndWait("Wpisano niepoprawne hasło!");
                return;
            }
            var firstPasswordHash = Util.ToSHA256String(_viewHelper.AskForPassword("Podaj NOWE hasło dla konta: "));
            var secondPasswordHash = Util.ToSHA256String(_viewHelper.AskForPassword("Powtórz NOWE hasło dla konta: "));

            if (firstPasswordHash != secondPasswordHash)
            {
                _viewHelper.WriteAndWait("Wpisano dwa różne hasła!");
                return;
            }

            currentUser.PasswordHash = firstPasswordHash;
            _userRepository.Save(currentUser);
            _viewHelper.WriteAndWait("Hasło zostało zmienione!");
        }

        public void Login()
        {
            var username = _viewHelper.AskForStringNotBlank("Podaj nazwę użytkownika: ");
            if (!_userRepository.Exists(u => u.Username == username))
            {
                _viewHelper.WriteAndWait("Użytkownik nie istnieje!");
                return;
            }
            var passwordHash = Util.ToSHA256String(_viewHelper.AskForPassword("Podaj hasło dla konta: "));
            var user = _userRepository.Authenticate(username, passwordHash);

            if (!user.IsLogged)
            {
                _viewHelper.WriteAndWait("Niepoprawne hasło!");
                return;
            }
            // wczytywanie roli dla użytkownika - lub zrobienie tego na poziomie autoryzacji

            _container.Register(user, "CurrentUser");
            _viewHelper.WriteAndWait($"\nPomyślnie zalogowałeś się na konto '{username}'!");
        }

        public void Logout()
        {
            var user = GetCurrentUser();
            user.IsLogged = false;
            _userRepository.Save(user);
            // redirect do menu z logowaniem?
            UserAuthentication();
        }

        public void ShowCurrentUserInfo()
        {
            var user = GetCurrentUser();
            var info = new List<string>
            {
                $"Id: #{user.UserId}", $"Nazwa użytkownika: {user.Username}", $"Typ konta: {user.Role.Name}",
                $"Konto utworzone dnia: {user.CreationTime}",  $"Ostatnie logowanie: {user.LastLogin}"
            };
            info.ForEach(_console.WriteLine);
            _console.ReadLine();
        }

        public void ShowAllAccounts()
        {
            ShowAccounts(_userRepository.GetAll());
        }

        public void ShowAccountInfo()
        {
            logger.Info("Show Account Info - test");
            if (UserNotExistsElseOut("Podaj id lub nazwę użytkownika: ", out var user))
                return;

            ShowOneUser(user);
        }

        private void ShowOneUser(User user)
        {
            var info = new List<string>
            {
                $"Id: #{user.UserId}", $"Nazwa użytkownika: {user.Username}", $"Typ konta: {user.Role.Name}",
                $"Konto utworzone dnia: {user.CreationTime}",  $"Ostatnie logowanie: {user.LastLogin}"
            };
            _console.Clear();
            info.ForEach(_console.WriteLine);
            _console.ReadLine();
        }

        private void ShowAccounts(List<User> userList)
        {
            _console.Clear();
            var header = $"{"Id",5} | {"Username",15}| {"Typ konta",10}| {"Data utworzenia", 15}| {"Ostatnie logowanie", 15}";
            _console.WriteLine(header);
            _console.WriteLine(new string('-', header.Length));
            userList.ForEach(x => {
                _console.WriteLine($"{x.UserId,5}| {x.Username,15}| {x.Role.Name,10} | {x.CreationTime, 15}| {x.LastLogin, 15}");
            });
            _viewHelper.WriteAndWait($"Znaleziono {userList.Count} pasujących produktów:");
        }

        private bool UserNotExistsElseOut(string message, out User user)
        {
            var idOrName = _viewHelper.AskForStringNotBlank(message);
            if (int.TryParse(idOrName, out var userId))
            {
                user = _userRepository.Get(userId);
                if (user != null) 
                    return false;
                _viewHelper.WriteAndWait($"Użytkownik o id {userId} nie istnieje!");
                return true;
            }
            else
            {
                user = _userRepository.Get(u => u.Username == idOrName);
                if (user != null) 
                    return false;
                _viewHelper.WriteAndWait($"Użytkownik o nazwie '{idOrName}' nie istnieje!");
                return true;
            }
        }

        private User GetCurrentUser() => _container.Resolve<User>("CurrentUser");
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
