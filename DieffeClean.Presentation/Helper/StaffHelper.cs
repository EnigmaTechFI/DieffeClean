using DieffeClean.Domain.Constants;
using DieffeClean.Domain.Model;
using DieffeClean.Domain.Services;
using DieffeClean.Presentation.Model.Staff;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;

namespace DieffeClean.Presentation.Helper;

public class StaffHelper
{
    private readonly IStaffService _staffService;
    private readonly IApartmentService _apartmentService;
    private readonly UserManager<MyUser> _userManager;
    private readonly IEmailService _emailService;


    public StaffHelper(IStaffService staffService, IApartmentService apartmentService, UserManager<MyUser> userManager, IEmailService emailService)
    {
        _staffService = staffService;
        _apartmentService = apartmentService;
        _userManager = userManager;
        _emailService = emailService;
    }

    public ListStaffViewModel GetStaff(string role)
    {
        //Qui fare il ciclaggio e togliere i role che non vanno bene.
        return new ListStaffViewModel()
        {
            Staffs = _staffService.GetStaff()
        };
    }

    public NewStaffViewModel GetNewStaffViewModel()
    {
        return new NewStaffViewModel()
        {
            Apartaments = _apartmentService.GetAll()
        };
    }
    
    public void NewStaff(NewStaffViewModel model, string role, MyUser User)
    {
        
        /*ParseDates(model);  //dopo vagliare la validazione
        ValidateReservationDate(model.Reservation, DateTime.Now);
        */
        
        var adm = _userManager.FindByEmailAsync(model.Staff.Email).Result;
        if (adm == null)
        {

            MyUser newUser = new MyUser();
            newUser.UserName = model.Staff.UserName;
            newUser.Email = model.Staff.Email;

            var password = PasswordGenerator();

            IdentityResult result = _userManager.CreateAsync(newUser, password).Result;

            if (result.Succeeded)
            {
                if (role == "Admin")
                {
                    _userManager.AddToRolesAsync(newUser, new[] { Roles.Admin });
                }
                else
                {
                    _userManager.AddToRolesAsync(newUser, new[] { Roles.CleaningUser });
                }

                //_emailService.SendEmailNewAccount(newUser.Email, password, role);
                
                foreach(var loc in model.Apartaments)
                {
                    newUser.UserApartments.Add(new UserApartment()
                    {
                        ApatmentId = loc.Id,
                        MyUserId = newUser.Id,
                    });
                }
            }
        }
    }
    
    private string PasswordGenerator()
    {

        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var minusc = "abcdefghijklmnopqrstuvwxyz";
        var num = "0123456789";
        var specialChar = "!$&()=?^*@#";
        var stringChars = new char[9];
        var random = new Random();

        for (int i = 0; i < 2; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }
        for (int i = 2; i < 5; i++)
        {
            stringChars[i] = minusc[random.Next(minusc.Length)];
        }
        for (int i = 5; i < stringChars.Length; i++)
        {
            stringChars[i] = num[random.Next(num.Length)];
        }
        stringChars[8] = specialChar[random.Next(num.Length)];

        return new String(stringChars);
    }
    
}