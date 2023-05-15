using DieffeClean.Domain.Constants;
using DieffeClean.Domain.Model;
using DieffeClean.Presentation.Helper;
using DieffeClean.Presentation.Model.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DieffeClean.WebApp.Controllers;

[Authorize(Roles = Roles.SuperAdmin)]
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
            _staffHelper.NewStaff(model, "Admin");
            return RedirectToAction("HostList");
        }
        catch (Exception e)
        {
            return View(_staffHelper.GetNewStaffViewModelException(model));
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> NewClean(NewStaffViewModel model)
    {
        try
        {
            _staffHelper.NewStaff(model, "CleaningUser");
            return RedirectToAction("CleanList");
        }
        catch (Exception e)
        {
            return RedirectToAction("NewClean");
        }
    }
    
    [HttpGet]
    public IActionResult InfoHost(string id)
    {
        try
        {
            return View(_staffHelper.GetStaffById(id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet]
    public IActionResult InfoClean(string id)
    {
        try
        {
            return View(_staffHelper.GetStaffById(id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public RedirectToActionResult DeleteStaffHost(string staffId)
    {
        try
        {
            _staffHelper.DeleteStaff(staffId);
            return RedirectToAction("HostList");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("HostList");        }
    }
    
    public RedirectToActionResult DeleteStaffClean(string staffId)
    {
        try
        {
            _staffHelper.DeleteStaff(staffId);
            return RedirectToAction("CleanList");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("CleanList");        
        }
    }
}