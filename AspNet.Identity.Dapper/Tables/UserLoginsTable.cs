using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Dapper;

namespace AspNet.Identity.Dapper
{
    public class UserLoginsTable
    {
        private IdentityDbContextDapper _database;

        /// <summary>
        /// 传入数据库实例 的构造函数
        /// </summary>
        /// <param name="database"></param>
        public UserLoginsTable(IdentityDbContextDapper database)
        {
            _database = database;
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser user, UserLoginInfo login)
        {
            string commandText = "Delete from UserLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey";
            return _database.Connection.ExecuteAsync(commandText, new
            {
                UserId = user.Id,
                loginProvider = login.LoginProvider,
                providerKey = login.ProviderKey
            }).Result;
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            string commandText = "Delete from UserLogins where UserId = @userId";
           
            return _database.Connection.ExecuteAsync(commandText, new { UserId = userId }).Result;
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, UserLoginInfo login)
        {
            string commandText = "Insert into UserLogins (LoginProvider, ProviderKey, UserId) values (@loginProvider, @providerKey, @userId)";
            
            return _database.Connection.ExecuteAsync(commandText, new {
                loginProvider = login.LoginProvider,
                providerKey= login.ProviderKey,
                userId=user.Id
            }).Result;
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            string commandText = "Select UserId from UserLogins where LoginProvider = @loginProvider and ProviderKey = @providerKey";
            
            return _database.Connection.QueryAsync<string>(commandText, new
            {
                loginProvider = userLogin.LoginProvider,
                providerKey = userLogin.ProviderKey
            }).Result.FirstOrDefault();
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(string userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();
            string commandText = "Select * from UserLogins where UserId = @userId";

            dynamic rows = _database.Connection.QueryAsync(commandText, new
            {
                userId = userId
            }).Result;
            foreach(var item in rows)
            {
                logins.Add(new UserLoginInfo(item.LoginProvider, item.ProviderKey));
            }
            return logins;
        }
    }
}
