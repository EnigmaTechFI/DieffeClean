using Microsoft.AspNetCore.Identity;

namespace DieffeClean.Domain.Model;

public class MyUser : IdentityUser
{
    public virtual List<UserApartment> UserApartments { get; set; }
}