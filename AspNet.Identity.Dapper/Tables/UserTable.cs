using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace AspNet.Identity.Dapper
{
    public class UserTable<TUser>
         where TUser : IdentityUser
    {
        private IdentityDbContextDapper _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserTable(IdentityDbContextDapper database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<string> GetUserName(string userId)
        {
            string commandText = "Select Name from Users where Id = @id";
            return _database.Connection.QueryFirstOrDefaultAsync<string>(commandText, new { id = userId });
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public Task<string> GetUserId(string userName)
        {
            string commandText = "Select Id from Users where UserName = @name";
            return _database.Connection.QueryFirstOrDefaultAsync<string>(commandText, new { name = userName });
        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public Task<TUser> GetUserById(string userId)
        {
            string commandText = "Select * from Users where Id = @id";
            return _database.Connection.QueryFirstOrDefaultAsync<TUser>(commandText, new { id = userId });
        }

        /// <summary>
        /// Returns a list of TUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public List<TUser> GetUserByName(string userName)
        {
            string commandText = "Select * from Users where UserName = @name";
            return _database.Connection.QueryAsync<TUser>(commandText, new { name = userName }).Result.AsList();
        }

        public List<TUser> GetUserByEmail(string email)
        {
            string commandText = "Select * from Users where Email = @email";
            return _database.Connection.QueryAsync<TUser>(commandText, new { email = email }).Result.AsList();
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            string commandText = "Select PasswordHash from Users where Id = @id";
            var passHash = _database.Connection.QueryAsync<string>(commandText, new { Id = userId }).Result.FirstOrDefault();
            if(string.IsNullOrEmpty(passHash))
            {
                return null;
            }

            return passHash;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(string userId, string passwordHash)
        {
            string commandText = "Update Users set PasswordHash = @pwdHash where Id = @id";
            return _database.Connection.ExecuteAsync(commandText, new
            {
                pwdHash = passwordHash,
                id = userId
            }).Result;
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            string commandText = "Select SecurityStamp from Users where Id = @id";
            return _database.Connection.QueryAsync<string>(commandText, new { id = userId }).Result.FirstOrDefault();
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> Insert(TUser user)
        {
            string commandText = @"Insert into Users (UserName, Id, PasswordHash, SecurityStamp,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled)
                values (@name, @id, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed,@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled)";
            return _database.Connection.ExecuteAsync(commandText, new
            {
                name = user.UserName,
                id = user.Id,
                pwdHash = user.PasswordHash,
                SecStamp=user.SecurityStamp,
                email = user.Email,
                emailconfirmed = user.EmailConfirmed,
                phonenumber = user.PhoneNumber,
                phonenumberconfirmed = user.PhoneNumberConfirmed,
                accesscount = user.AccessFailedCount,
                lockoutenabled = user.LockoutEnabled,
                lockoutenddate = user.LockoutEndDateUtc,
                twofactorenabled = user.TwoFactorEnabled
            });
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(string userId)
        {
            string commandText = "Delete from Users where Id = @userId";
            return _database.Connection.ExecuteAsync(commandText, new { userId = userId }).Result;
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(TUser user)
        {
            string commandText = @"Update Users set UserName = @userName, PasswordHash = @pswHash, SecurityStamp = @secStamp, 
                Email=@email, EmailConfirmed=@emailconfirmed, PhoneNumber=@phonenumber, PhoneNumberConfirmed=@phonenumberconfirmed,
                AccessFailedCount=@accesscount, LockoutEnabled=@lockoutenabled, LockoutEndDateUtc=@lockoutenddate, TwoFactorEnabled=@twofactorenabled  
                WHERE Id = @userId";
            return _database.Connection.ExecuteAsync(commandText, new
            {
                userName = user.UserName,
                pswHash = user.PasswordHash,
                secStamp = user.SecurityStamp,
                userId = user.Id,
                email = user.Email,
                emailconfirmed = user.EmailConfirmed,
                phonenumber = user.PhoneNumber,
                phonenumberconfirmed = user.PhoneNumberConfirmed,
                accesscount = user.AccessFailedCount,
                lockoutenabled = user.LockoutEnabled,
                lockoutenddate = user.LockoutEndDateUtc,
                twofactorenabled = user.TwoFactorEnabled
            }).Result;
        }


        public IEnumerable<TUser> GetAll()
        {
            string commandText = @"SELECT * FROM Users";
            return _database.Connection.QueryAsync<TUser>(commandText).Result;
        }
    }
}
