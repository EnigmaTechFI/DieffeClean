using DieffeClean.Domain.Constants;
using DieffeClean.Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace DieffeClean.Domain.Data;

public class DbInitializer
{
        public static void SeedUsersAndRoles(UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager,
        ApplicationDbContext dbContext)
    {
        string[] roleNames = { Roles.SuperAdmin, Roles.Admin, Roles.CleaningUser };

        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = roleManager.RoleExistsAsync(roleName).Result;
            if (!roleExist)
                roleResult = roleManager.CreateAsync(new IdentityRole(roleName)).Result;
        }

        var adm1 = userManager.FindByEmailAsync("lorenzo.vettori11@gmail.com").Result;
        if (adm1 == null)
        {
            adm1 = new MyUser
            {
                UserName = "lore_vetto11",
                Email = "lorenzo.vettori11@gmail.com",
            };

            IdentityResult result = userManager.CreateAsync(adm1, "Antani123!").Result;
            var addedRole = userManager.AddToRolesAsync(adm1, new[] { Roles.SuperAdmin}).Result;
        }

        dbContext.SaveChanges();

    }
}