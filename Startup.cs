using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QLBanHang.Models;

[assembly: OwinStartupAttribute(typeof(QLBanHang.Startup))]
namespace QLBanHang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //InitUserRole();
        }
        private void InitUserRole() 
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //tạo role Admin
            if(!roleManager.RoleExists("Admin"))//chưa có
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                role = new IdentityRole();
                role.Name = "WebMaster";
                roleManager.Create(role);
                //tạo user
                var user = new ApplicationUser();
                user.UserName = "admin@qlbh.com";
                user.Email = "admin@qlbh.com";
                var check = userManager.Create(user, "123456");
                //đưa user vào role Admin
                if (check.Succeeded)
                    userManager.AddToRole(user.Id, "Admin");

                user = new ApplicationUser();
                user.UserName = "master1@qlbh.com";
                user.Email = "master1@qlbh.com";
                check = userManager.Create(user, "123456");
                //đưa user vào role WebMaster
                if (check.Succeeded)
                    userManager.AddToRole(user.Id, "WebMaster");

                user = new ApplicationUser();
                user.UserName = "master2@qlbh.com";
                user.Email = "master2@qlbh.com";
                check = userManager.Create(user, "123456");
                //đưa user vào role WebMaster
                if (check.Succeeded)
                    userManager.AddToRole(user.Id, "WebMaster");
            }    
        }
    }
}
