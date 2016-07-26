using System;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.Dapper
{
    public class IdentityUser :IUser
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 使用有用户名的构造函数
        /// </summary>
        /// <param name="userName"></param>
        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        ///     邮箱是否通过验证，默认是false
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        ///     密码hash
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        ///     临时安全码
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        ///     手机号是否通过验证 默认是false
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        ///     两部验证是否开启
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        ///     账户锁定结束的结束时间
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        ///     是否锁定用户
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        ///     用户登录错误次数
        /// </summary>
        public virtual int AccessFailedCount { get; set; }
    }
}
