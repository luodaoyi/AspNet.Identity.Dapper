using System;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.Dapper
{
    public class IdentityRole :IRole
    {
        /// <summary>
        /// 用户角色默认构造函数
        /// </summary>
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 有角色名的构造函数
        /// </summary>
        /// <param name="name"></param>
        public IdentityRole(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// 有名字和id的构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// 角色ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
