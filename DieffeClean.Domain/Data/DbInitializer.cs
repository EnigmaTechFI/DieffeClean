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

        var adm1 = userManager.FindByEmailAsync("fedefalconecescutti@hotmail.com").Result;
        if (adm1 == null)
        {
            adm1 = new MyUser
            {
                UserName = "fedefalcone",
                Email = "fedefalconecescutti@hotmail.com",
            };

            IdentityResult result = userManager.CreateAsync(adm1, "SKhvs547!").Result;
            var addedRole = userManager.AddToRolesAsync(adm1, new[] { Roles.SuperAdmin}).Result;
        }
        
        var adm2 = userManager.FindByEmailAsync("francescodaiuto91@gmail.com").Result;
        if (adm2 == null)
        {
            adm2 = new MyUser
            {
                UserName = "francescodaiuto",
                Email = "francescodaiuto91@gmail.com",
            };

            IdentityResult result = userManager.CreateAsync(adm2, "SKhvs547!").Result;
            var addedRole = userManager.AddToRolesAsync(adm2, new[] { Roles.SuperAdmin}).Result;
        }
        
        var adm3 = userManager.FindByEmailAsync("alessiocelo@gmail.com").Result;
        if (adm3 == null)
        {
            adm3 = new MyUser
            {
                UserName = "alessiocelo",
                Email = "alessiocelo@gmail.com",
            };

            IdentityResult result = userManager.CreateAsync(adm3, "SKhvs547!").Result;
            var addedRole = userManager.AddToRolesAsync(adm3, new[] { Roles.SuperAdmin}).Result;
        }

        dbContext.SaveChanges();

    }
}