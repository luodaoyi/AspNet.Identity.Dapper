using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using AspNet.Identity.Dapper;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using WebDemo.Models;

namespace WebDemo
{
    public class EmailService :IIdentityMessageService
    {
        public string SmtpServer { get; set; }
        public string EmailUser { get; set; }
        public string EmailPwd { get; set; }
        public EmailService()
        {
            SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            EmailUser = ConfigurationManager.AppSettings["EmailUser"];
            EmailPwd = ConfigurationManager.AppSettings["EmailPassWord"];
        }
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage mailMessage = new System.Net.Mail.MailMessage(
                EmailUser,
                message.Destination,
                message.Subject,
                message.Body);
            //使用指定的邮件地址初始化MailAddress实例

            //电子邮件正文的编码
            mailMessage.BodyEncoding = Encoding.Default;
            mailMessage.Priority = MailPriority.High;
            mailMessage.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(SmtpServer, 25);
            //指定发件人的邮件地址和密码以验证发件人身份
            smtp.UseDefaultCredentials = true;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(EmailUser, EmailPwd);
            //设置SMTP邮件服务器
            return smtp.SendMailAsync(mailMessage);
        }
    }

    public class SmsService :IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // 在此处插入 SMS 服务可发送短信。
            return Task.FromResult(0);
        }
    }

    // 配置此应用程序中使用的应用程序用户管理器。UserManager 在 ASP.NET Identity 中定义，并由此应用程序使用。
    public class AppUserManager :UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        {
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(new UserStore<AppUser>(context.Get<AppDbContext>()));
            // 配置用户名的验证逻辑
            manager.UserValidator = new UserValidator<AppUser>(manager)
            {

                AllowOnlyAlphanumericUserNames = true,
                //邮箱不重名
                RequireUniqueEmail = true
            };

            // 配置密码的验证逻辑
            manager.PasswordValidator = new PasswordValidator
            {
                //长度
                RequiredLength = 6,
                //需要特殊字符!@#$等
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // 配置用户锁定默认值
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
            // 你可以编写自己的提供程序并将其插入到此处。
            manager.RegisterTwoFactorProvider("电话代码", new PhoneNumberTokenProvider<AppUser>
            {
                MessageFormat = "你的安全代码是 {0}"
            });
            manager.RegisterTwoFactorProvider("电子邮件代码", new EmailTokenProvider<AppUser>
            {
                Subject = "安全代码",
                BodyFormat = "你的安全代码是 {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if(dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<AppUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

    }

    // 配置要在此应用程序中使用的应用程序登录管理器。
    public class AppSignInManager :SignInManager<AppUser, string>
    {
        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AppUser user)
        {
            return user.GenerateUserIdentityAsync((AppUserManager)UserManager);
        }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context)
        {
            return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }

        public override Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {

            return base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
        }
    }

    //配置此应用程序中使用的应用程序角色管理器。RoleManager 在 ASP.NET Identity 中定义，并由此应用程序使用。
    public class AppRoleManager :RoleManager<AppRole>
    {
        public AppRoleManager(RoleStore<AppRole> roleStore)
            : base(roleStore)
        {
        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options,
            IOwinContext context)
        {
            return new AppRoleManager(new RoleStore<AppRole>(context.Get<AppDbContext>()));
        }
    }
}
