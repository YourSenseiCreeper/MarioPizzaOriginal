using System;
using ServiceStack.OrmLite;

namespace MarioPizzaOriginal.Domain.DataAccess
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly OrmLiteConnectionFactory db;

        public UserRepository(OrmLiteConnectionFactory dbConnection) : base(dbConnection)
        {
            db = dbConnection;
        }

        public User Authenticate(string username, string passwordHash)
        {
            var dbConn = db.Open();
            var theUser = dbConn.Single<User>(u => u.Username == username);
            theUser.IsLogged = theUser.PasswordHash == passwordHash;
            theUser.LastLogin = theUser.IsLogged ? DateTime.Now : theUser.LastLogin;
            dbConn.Update(theUser);
            dbConn.Close();
            return theUser;
        }

        public bool UserExists(string username)
        {
            return db.Open().Exists<User>(user => user.Username == username);
        }

        public void Register(string username, string passwordHash)
        {
            var dbConn = db.Open();
            var freshUser = new User {Username = username, PasswordHash = passwordHash, CreationTime = DateTime.Now};
            dbConn.Insert(freshUser);
            dbConn.Close();
        }

        public User GetUser(string username)
        {
            return db.Open().Single<User>(u => u.Username == username);
        }

        public void Logout(string username)
        {
            using (var dbConn = db.Open())
            {
                dbConn.Update<User>(new { IsLogged = false }, u => u.Username == username);
            }
        }
    }
}
