using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MarioPizzaOriginal.Domain;
using Model.DataAccess;
using ServiceStack;
using TinyIoC;

namespace MarioPizzaOriginal.Controller
{
    public class UserController
    {
        private readonly IUserRepository _userRepository;
        private readonly TinyIoCContainer _container;

        public UserController(TinyIoCContainer container)
        {
            _container = container;
            _userRepository = container.Resolve<IUserRepository>();
        }

        public void Register()
        {
            bool notUnique = true;
            string username;
            do
            {
                username = ViewHelper.AskForStringNotBlank("Podaj nazwę użytkownika (nie może być pusta!): ");
                notUnique = _userRepository.UserExists(username);
                if (notUnique) ViewHelper.WriteAndWait($"Użytkownik '{username}' już istnieje. Wybierz inną nazwę użytkownika!");
            } while (notUnique);

            var hashBytes = SHA256.Create().ComputeHash(ViewHelper.AskForPassword("Podaj hasło dla konta: ", null).ToUtf8Bytes());
            var passwordHash = ViewHelper.ConvertSHAToString(hashBytes);
            _userRepository.Register(username, passwordHash);
            ViewHelper.WriteAndWait("\nRejestracja zakończona pomyślnie!");
        }

        public void ResetPassword()
        {

        }

        public void Login()
        {
            bool logged;
            string username;
            do
            {
                username = ViewHelper.AskForStringNotBlank("Podaj nazwę użytkownika: ");
                var hashBytes = SHA256.Create().ComputeHash(ViewHelper.AskForPassword("Podaj hasło dla konta: ", null).ToUtf8Bytes());
                var passwordHash = ViewHelper.ConvertSHAToString(hashBytes);
                logged = _userRepository.Authenticate(username, passwordHash);
                // opcja wyjścia z tego menu?
            } while (!logged);

            var user = _userRepository.GetUser(username);
            _container.Register(user, "CurrentUser");
            ViewHelper.WriteAndWait($"\nPomyślnie zalogowałeś się na konto '{username}'!");
        }

        public void Logout()
        {

        }

        public void ShowCurrentUserInfo()
        {
            var user = _container.Resolve<User>("CurrentUser");
            var info = new List<string>
            {
                $"Id: #{user.UserId}", $"Nazwa użytkownika: {user.Username}", $"Typ konta: {user.AccountType}",
                $"Konto utworzone dnia: {user.CreationTime}",  $"Ostatnie logowanie: {user.LastLogin}"
            };
            info.ForEach(line => Console.WriteLine(line));
        }

        public void ShowAllAccounts()
        {

        }

        public void ShowAccountInfo()
        {

        }
    }
}
