using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace AspNet.Identity.Dapper
{
    public class UserRolesTable
    {
        private IdentityDbContextDapper _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserRolesTable(IdentityDbContextDapper database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(string userId)
        {
            List<string> roles = new List<string>();
            string commandText = "Select Roles.Name from UserRoles, Roles where UserRoles.UserId = @userId and UserRoles.RoleId = Roles.Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            return _database.Connection.QueryAsync<string>(commandText, new { userId = userId }).Result.ToList();

        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            string commandText = "Delete from UserRoles where UserId = @userId";
            return _database.Connection.ExecuteAsync(commandText, new { UserId = userId }).Result;
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, string roleId)
        {
            string commandText = "Insert into UserRoles (UserId, RoleId) values (@userId, @roleId)";
            return _database.Connection.ExecuteAsync(commandText, new { userId=user.Id, roleId=roleId }).Result;
        }
    }
}
