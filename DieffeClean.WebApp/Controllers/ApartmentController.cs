using DieffeClean.Domain.Constants;
using DieffeClean.Domain.Model;
using DieffeClean.Presentation.Helper;
using DieffeClean.Presentation.Model.Apartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NToastNotify;

namespace DieffeClean.WebApp.Controllers;

public class ApartmentController : Controller
{
    private readonly ApartmentHelper _apartmentHelper;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly UserManager<MyUser> _userManager;
    private readonly IToastNotification _toastNotification;

    public ApartmentController(ApartmentHelper apartmentHelper, UserManager<MyUser> userManager, IToastNotification toastNotification)
    {
        _apartmentHelper = apartmentHelper;
        _userManager = userManager;
        _toastNotification = toastNotification;
    }
    [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin + ","+ Roles.CleaningUser)]
    [HttpGet]
    public async Task<IActionResult> List()
    {
        try
        {
            return View(await _apartmentHelper.GetApartments(await _userManager.GetUserAsync(User)));
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin + ","+ Roles.CleaningUser)]
    [HttpGet]
    public IActionResult Info(Guid id)
    {
        try
        {
            return View(_apartmentHelper.GetApartmentById(id));
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("List");
        }
    }

    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public IActionResult Update(Guid id)
    {
        try
        {
            return View(_apartmentHelper.GetApartmentById(id));
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return RedirectToAction("Info", new {id = id});
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpPost]
    public IActionResult Update(CreateApartmentViewModel model)
    {
        try
        {
            _apartmentHelper.UpdateApartment(model);
            _toastNotification.AddSuccessToastMessage("Appartamento aggiornato correttamente.");
            return RedirectToAction("Info", new {id = model.Apartment.Id});
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return View(model);
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [Authorize(Roles = Roles.SuperAdmin)]
    [HttpPost]
    public IActionResult Create(CreateApartmentViewModel model)
    {
        try
        {
            _apartmentHelper.CreateApartment(model);
            _toastNotification.AddSuccessToastMessage("Appartamento creato correttamente.");
            return RedirectToAction("List");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return View(model);
        }
    }
}