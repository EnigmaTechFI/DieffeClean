using DieffeClean.Domain.Constants;
using DieffeClean.Domain.Model;
using DieffeClean.Presentation.Helper;
using DieffeClean.Presentation.Model.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NToastNotify;

namespace DieffeClean.WebApp.Controllers;

[Authorize(Roles = Roles.SuperAdmin)]
public class StaffController : Controller
{
    private readonly StaffHelper _staffHelper;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly IToastNotification _toastNotification;


    public StaffController(StaffHelper staffHelper, IToastNotification toastNotification)
    {
        _staffHelper = staffHelper;
        _toastNotification = toastNotification;
    }

    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public IActionResult HostList()
    {
        try
        {
            return View(_staffHelper.GetStaff("Admin"));
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public IActionResult NewHost()
    {
        try
        {
            return View(_staffHelper.GetNewStaffViewModel());
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public IActionResult CleanList()
    {
        try
        {
            return View(_staffHelper.GetStaff("CleaningUser"));
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public IActionResult NewClean()
    {
        try
        {
            return View(_staffHelper.GetNewStaffViewModel());
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpPost]
    public async Task<IActionResult> NewHost(NewStaffViewModel model)
    {

        try
        {
            await _staffHelper.NewStaff(model, "Admin");
            _toastNotification.AddSuccessToastMessage("Account creato correttamente.");
            return RedirectToAction("HostList");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return View(_staffHelper.GetNewStaffViewModelException(model));
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpPost]
    public async Task<IActionResult> NewClean(NewStaffViewModel model)
    {
        try
        {
            await _staffHelper.NewStaff(model, "CleaningUser");
            _toastNotification.AddSuccessToastMessage("Account creato correttamente.");
            return RedirectToAction("CleanList");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("NewClean");
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public IActionResult InfoHost(string id)
    {
        try
        {
            return View(_staffHelper.GetStaffById(id));
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("HostList");
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public IActionResult InfoClean(string id)
    {
        try
        {
            return View(_staffHelper.GetStaffById(id));
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("CleanList");
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    public RedirectToActionResult DeleteStaffHost(string staffId)
    {
        try
        {
            _staffHelper.DeleteStaff(staffId);
            _toastNotification.AddSuccessToastMessage("Account eliminato correttamente.");
            return RedirectToAction("HostList");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("HostList");        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    public RedirectToActionResult DeleteStaffClean(string staffId)
    {
        try
        {
            _staffHelper.DeleteStaff(staffId);
            _toastNotification.AddSuccessToastMessage("Account eliminato correttamente.");
            return RedirectToAction("CleanList");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("CleanList");        
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public IActionResult UpdateClean(string id)
    {
        try
        {
            return View(_staffHelper.GetStaffById(id));
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("InfoClean", new {id = id});
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public IActionResult UpdateHost(string id)
    {
        try
        {
            return View(_staffHelper.GetStaffById(id));
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("InfoHost", new {id = id});
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpPost]
    public async Task<RedirectToActionResult> UpdateClean(string Id, NewStaffViewModel model)
    {
        try
        {
            _staffHelper.UpdateStaff(Id, model);
            _toastNotification.AddSuccessToastMessage("Account modificato correttamente.");
            return RedirectToAction("CleanList");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("CleanList");
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpPost]
    public async Task<RedirectToActionResult> UpdateHost(string Id, NewStaffViewModel model)
    {
        try
        {
            _staffHelper.UpdateStaff(Id, model);
            _toastNotification.AddSuccessToastMessage("Account modificato correttamente.");
            return RedirectToAction("HostList");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("HostList");
        }
    }
}