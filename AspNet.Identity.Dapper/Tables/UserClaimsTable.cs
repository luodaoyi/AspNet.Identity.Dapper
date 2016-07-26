using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;

namespace AspNet.Identity.Dapper
{
    public class UserClaimsTable
    {
        private IdentityDbContextDapper _database;

     
        /// <summary>
        /// 构造函数传入一个mysql连接实例
        /// </summary>
        /// <param name="database"></param>
        public UserClaimsTable(IdentityDbContextDapper database)
        {
            _database = database;
        }

        /// <summary>
        /// 使用用户id返回ClaimsIdentity声明实例
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(string userId)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            string commandText = "Select * from UserClaims where UserId = @userId";
            dynamic result   = _database.Connection.QueryAsync(commandText, new { UserId = userId }).Result;
            foreach(var c in result)
            {
                claims.AddClaim(new Claim(c.ClaimType, c.ClaimValue));
            }

            return claims;
        }

        /// <summary>
        /// 使用用户id删除所有的ClaimsIdentity声明
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            string commandText = "Delete from UserClaims where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("userId", userId);
            Task<int> result= _database.Connection.ExecuteAsync(commandText, new { userId = userId });
            return result.Result;
        }

        /// <summary>
        /// 在声明表中插入一条新的记录
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public int Insert(Claim userClaim, string userId)
        {
            string commandText = "Insert into UserClaims (ClaimValue, ClaimType, UserId) values (@value, @type, @userId)";
            return _database.Connection.ExecuteAsync(commandText, new
            {
                value = userClaim.Value,
                type = userClaim.Type,
                userId = userId
            }).Result;
        }

        /// <summary>
        /// 删除某用户的角色
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser user, Claim claim)
        {
            string commandText = "Delete from UserClaims where UserId = @userId and @ClaimValue = @value and ClaimType = @type";
            return _database.Connection.ExecuteAsync(commandText, new
            {
                userId = user.Id,
                value = claim.Value,
                type = claim.Type
            }).Result;
        }
    }
}
