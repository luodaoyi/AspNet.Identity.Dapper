using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AspNet.Identity.Dapper;
using Microsoft.AspNet.Identity;

namespace WebDemo.Models
{
    public class AppDbContext :IdentityDbContextDapper
    {
        static AppDbContext()
        {
            // 在第一次启动网站时初始化数据库添加管理员用户凭据和admin 角色到数据库

        }
        public AppDbContext(string connectionName)
            : base(connectionName)
        {
        }

        public static AppDbContext Create()
        {
            //获得数据库字符串链接名
            string name = ConfigurationManager.AppSettings["databaseConfName"];
            return new AppDbContext(name);
        }
    }

    public class AppUser :IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }

    public class AppRole :IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string name) : base(name) { }
        public AppRole(string name, string id) : base(name, id) { }
    }
}