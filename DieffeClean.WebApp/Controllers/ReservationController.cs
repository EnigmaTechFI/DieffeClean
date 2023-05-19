using DieffeClean.Domain.Constants;
using DieffeClean.Domain.Model;
using DieffeClean.Presentation.Helper;
using DieffeClean.Presentation.Model.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NToastNotify;

namespace DieffeClean.WebApp.Controllers;

public class ReservationController : Controller
{
    private readonly ReservationHelper _reservationHelper;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly UserManager<MyUser> _userManager;
    private readonly IToastNotification _toastNotification;

    public ReservationController(ReservationHelper reservationHelper, UserManager<MyUser> userManager, IToastNotification toastNotification)
    {
        _reservationHelper = reservationHelper;
        _userManager = userManager;
        _toastNotification = toastNotification;
    }
    
    [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin)]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            return View(await _reservationHelper.GetCreateReservationViewModel(await _userManager.GetUserAsync(User)));
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("List");
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateReservationViewModel model)
    {
        try
        {
            _reservationHelper.CreateReservation(model);
            _toastNotification.AddSuccessToastMessage("Prenotazione creata correttamente.");
            return RedirectToAction("List");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return View(await _reservationHelper.GetCreateReservationViewModelException(model, await _userManager.GetUserAsync(User)));
        }
    }
    [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin + ","+ Roles.CleaningUser)]
    [HttpGet]
    public async Task<IActionResult> List()
    {
        try
        {
            return View(await _reservationHelper.GetReservations(await _userManager.GetUserAsync(User)));
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            _toastNotification.AddErrorToastMessage("Sessione scaduta.");
            return RedirectToAction("Logout", "Auth");
        }
    }
    [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin + ","+ Roles.CleaningUser)]
    [HttpGet]
    public IActionResult Info(Guid id)
    {
        try
        {
            return View(_reservationHelper.GetInfoReservationViewModel(id));
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            _toastNotification.AddErrorToastMessage(e.Message);
            return RedirectToAction("List");
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin + ","+ Roles.CleaningUser)]
    [HttpPost]
    public IActionResult Delete(InfoReservationViewModel model)
    {
        try
        {
            _reservationHelper.DeleteReservation(model.Reservation.Id);
            _toastNotification.AddSuccessToastMessage("Prenotazione cancellata correttamente.");
            return RedirectToAction("List");
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            _toastNotification.AddErrorToastMessage("Errore durante la cancellazione. Si prega di riprovare.");
            return RedirectToAction("Info", new { id = model.Reservation.Id});
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin)]
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        try
        {
            return View(await _reservationHelper.GetUpdateReservationViewModel(id, await _userManager.GetUserAsync(User)));
        }
        catch (Exception e)
        {
            return RedirectToAction("Info", new {id = id});
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Update(CreateReservationViewModel model)
    {
        try
        {
            _reservationHelper.UpdateReservation(model);
            _toastNotification.AddSuccessToastMessage("Prenotazione aggiornata correttamente.");
            return RedirectToAction("List");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            Logger.Error(e, e.Message);
            return View(await _reservationHelper.GetCreateReservationViewModelException(model, await _userManager.GetUserAsync(User)));
        }
    }
    
    [Authorize(Roles = Roles.SuperAdmin + ","+ Roles.Admin + ","+ Roles.CleaningUser)]
    [HttpGet]
    public async Task<IActionResult> Calendar()
    {
        try
        {
            return View(await _reservationHelper.GetCalendarViewModel(await _userManager.GetUserAsync(User)));
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            _toastNotification.AddErrorToastMessage("Sessione scaduta.");
            return RedirectToAction("Logout", "Auth");
        }
    }
    
}