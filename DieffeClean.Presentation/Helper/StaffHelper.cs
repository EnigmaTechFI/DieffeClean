using System.Text.RegularExpressions;
using DieffeClean.Domain.Constants;
using DieffeClean.Domain.Model;
using DieffeClean.Domain.Services;
using DieffeClean.Presentation.Model.Staff;
using DieffeClean.Utils.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NETCore.MailKit.Core;
using NLog;

namespace DieffeClean.Presentation.Helper;

public class StaffHelper
{
    private readonly IStaffService _staffService;
    private readonly IApartmentService _apartmentService;
    private readonly UserManager<MyUser> _userManager;
    private readonly IEmailSender _emailSender;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public StaffHelper(IStaffService staffService, IEmailSender emailSender, IApartmentService apartmentService, UserManager<MyUser> userManager)
    {
        _staffService = staffService;
        _apartmentService = apartmentService;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public ListStaffViewModel GetStaff(string role)
    {
        var staffList = _staffService.GetStaff().Where(s => _userManager.IsInRoleAsync(s, role).Result).ToList();

        return new ListStaffViewModel()
        {
            Staffs = staffList
        };
    }

    public NewStaffViewModel GetNewStaffViewModel()
    {
        return new NewStaffViewModel()
        {
            Apartments = _apartmentService.GetAll()
        };
    }
    
    public async Task NewStaff(NewStaffViewModel model, string role)
    {
        
        ValidateEmail(model.Staff.Email);
        ValidateUser(model.Staff.UserName);

        
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
                    _userManager.AddToRolesAsync(newUser, new[] { Roles.Admin }).GetAwaiter().GetResult();
                }
                else
                {
                    _userManager.AddToRolesAsync(newUser, new[] { Roles.CleaningUser }).GetAwaiter().GetResult();
                }

                var userApartments = new List<UserApartment>();
                foreach (var apartmentId in model.SelectedApartments)
                {
                    userApartments.Add(new UserApartment()
                    {
                        MyUserId = newUser.Id,
                        ApartmentId = apartmentId
                    });
                }
                _staffService.CreateUserApartments(userApartments);

                var message = new Message(new (string, string)[] { (model.Staff.UserName, model.Staff.Email) }, "Nuovo account", "Nuovo account");
                List<(string, string)> replacer = new List<(string, string)> { ("[user]", model.Staff.Email) , ("[password]", password)};
                var currentPath = Directory.GetCurrentDirectory();
                try
                {
                    await _emailSender.SendEmailAsync(message, currentPath + "/wwwroot/MailTemplate/new-account.html", replacer);
                }
                catch (Exception e)
                {
                    Logger.Error(e, e.Message);
                }
            }
            
        }
        else
        {
            throw new Exception("Email già presente");
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
    
    public NewStaffViewModel GetStaffById(string id)
    {
        return new NewStaffViewModel()
        {
            Staff = _staffService.GetStaffById(id),
            Apartments = _apartmentService.GetAll()
        };
    }
    
    private void ValidateEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        Regex regex = new Regex(pattern);

        if (!regex.IsMatch(email))
        {
            throw new Exception("Email non corretta");
        }
    }

    private void ValidateUser(string user)
    {
        if (user.Contains(" "))
        {
            throw new Exception("Username non corretta, non ci possono essere spazi");
        }
    }

    public void DeleteStaff(string staffId)
    {
        _staffService.DeleteStaffById(staffId);
    }

    public NewStaffViewModel GetNewStaffViewModelException(NewStaffViewModel model)
    {
        model.Apartments = _apartmentService.GetAll();
        return model;
    }
    
    public async Task UpdateStaff(string Id, NewStaffViewModel model)
    {
        
        var userApartments = new List<UserApartment>();
        foreach (var apartmentId in model.SelectedApartments)
        {
            userApartments.Add(new UserApartment()
            {
                MyUserId = Id,
                ApartmentId = apartmentId
            });
        }
        _staffService.UpdateUserApartments(Id, userApartments);
    }
    
}