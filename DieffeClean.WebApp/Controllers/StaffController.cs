using DieffeClean.Domain.Model;
using DieffeClean.Presentation.Helper;
using DieffeClean.Presentation.Model.Staff;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DieffeClean.WebApp.Controllers;

public class StaffController : Controller
{
    private readonly StaffHelper _staffHelper;
    private readonly UserManager<MyUser> _userManager;


    public StaffController(StaffHelper staffHelper, UserManager<MyUser> userManager)
    {
        _staffHelper = staffHelper;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult HostList()
    {
        try
        {
            return View(_staffHelper.GetStaff("Admin"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet]
    public IActionResult NewHost()
    {
        try
        {
            return View(_staffHelper.GetNewStaffViewModel());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet]
    public IActionResult CleanList()
    {
        try
        {
            return View(_staffHelper.GetStaff("CleaningUser"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet]
    public IActionResult NewClean()
    {
        try
        {
            return View(_staffHelper.GetNewStaffViewModel());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> NewHost(NewStaffViewModel model)
    {
        try
        {
            _staffHelper.NewStaff(model, "Admin", await _userManager.GetUserAsync(this.User));
            return View();
        }
        catch (Exception e)
        {
            //qui poi andrà ricaricato l'elenco apparamenti, guardare in reservation
            return View();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> NewClean(NewStaffViewModel model)
    {
        try
        {
            _staffHelper.NewStaff(model, "CleaningUser", await _userManager.GetUserAsync(this.User));
            return View(); //redirect to list
        }
        catch (Exception e)
        {
            //qui poi andrà ricaricato l'elenco apparamenti, guardare in reservation
            return View();
        }
    }
}