using System;
using ServiceStack.OrmLite;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public User Authenticate(string username, string passwordHash)
        {
            var dbConn = connection.Open();
            var theUser = Get(u => u.Username == username, true);
            theUser.IsLogged = theUser.PasswordHash == passwordHash;
            theUser.LastLogin = theUser.IsLogged ? DateTime.Now : theUser.LastLogin;
            dbConn.Update(theUser);
            dbConn.Close();
            theUser.PasswordHash = "";
            return theUser;
        }

        // TODO: do usunięcia jeżeli nie będzie użyte
        public bool IsPasswordCorrect(string username, string passwordHash)
        {
            return connection.Open().Exists<User>(u => u.Username == username && u.PasswordHash == passwordHash);
        }

        public bool UserExists(string username)
        {
            return connection.Open().Exists<User>(user => user.Username == username);
        }

        public void Register(string username, string passwordHash)
        {
            var dbConn = connection.Open();
            var freshUser = new User {Username = username, PasswordHash = passwordHash, CreationTime = DateTime.Now};
            dbConn.Insert(freshUser);
            dbConn.Close();
        }

        public void Logout(string username)
        {
            using (var dbConn = connection.Open())
            {
                dbConn.Update<User>(new { IsLogged = false }, u => u.Username == username);
            }
        }
    }
}
