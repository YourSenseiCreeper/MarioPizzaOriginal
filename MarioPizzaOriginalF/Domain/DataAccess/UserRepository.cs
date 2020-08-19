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

        public bool Authenticate(string username, string passwordHash)
        {
            var dbConn = db.Open();
            Console.WriteLine(passwordHash);
            bool? logged = dbConn.Single<bool?>($"select 1 from user where username = \"{username}\" and passwordHash = \"{passwordHash}\"");
            //bool logged = Query();//dbConn.Exists<User>(user => user.Username == username && user.PasswordHash == passwordHash);
            if (logged != null)
            {
                // var dbConn = db.Open();
                dbConn.Update<User>(new {IsLogged = true}, u => u.Username == username);
                dbConn.BeginTransaction().Commit();
            }
            return logged != null ? true : false;
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
            dbConn.BeginTransaction().Commit();
            // db.Open().Save(freshUser);
        }

        public User GetUser(string username)
        {
            return db.Open().Single<User>(u => u.Username == username);
        }

        public void Logout(string username)
        {
            db.Open().Update<User>(new { IsLogged = false }, u => u.Username == username);
        }
    }
}
