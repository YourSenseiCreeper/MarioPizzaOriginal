using System;
using Model.DataAccess;
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
            bool logged = db.Open().Exists<User>(user => user.Username == username && user.PasswordHash == passwordHash);
            if (logged)
            {
                var user = db.Open().Single<User>(u => u.Username == username);
                user.IsLogged = logged;
                db.Open().Update(user);
            }
            return logged;
        }

        public bool UserExists(string username)
        {
            return db.Open().Exists<User>(user => user.Username == username);
        }

        public void Register(string username, string passwordHash)
        {
            var freshUser = new User {Username = username, PasswordHash = passwordHash, CreationTime = DateTime.Now};
            db.Open().Save(freshUser);
        }

        public User GetUser(string username)
        {
            return db.Open().Single<User>(u => u.Username == username);
        }
    }
}
