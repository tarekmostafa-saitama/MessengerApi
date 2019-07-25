using MessengerApi.Core.DbEntities;
using MessengerApi.Persistence.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MessengerApi.Persistence.Models
{
    public class StartupDbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));


            if (!roleManager.RoleExists("Member"))
            {
                var role = new IdentityRole();
                role.Name = "Member";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

            }


            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser() { Email = "admin@email.com", UserName = "admin" };
            var result = manager.Create(user, "Admin12345");

            manager.AddToRole(user.Id, "Admin");
        }
    }
}