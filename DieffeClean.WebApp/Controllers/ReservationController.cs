using DieffeClean.Presentation.Helper;
using DieffeClean.Presentation.Model.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace DieffeClean.WebApp.Controllers;

public class ReservationController : Controller
{
    private readonly ReservationHelper _reservationHelper;

    public ReservationController(ReservationHelper reservationHelper)
    {
        _reservationHelper = reservationHelper;
    }

    [HttpGet]
    public IActionResult Calendar()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View(_reservationHelper.GetCreateReservationViewModel());
    }
    
    [HttpPost]
    public IActionResult Create(CreateReservationViewModel model)
    {
        try
        {
            _reservationHelper.CreateReservation(model);
            return RedirectToAction("List");
        }
        catch (Exception e)
        {
            return View(_reservationHelper.GetCreateReservationViewModelException(model));
        }
    }
    
    [HttpGet]
    public IActionResult List()
    {
        try
        {
            return View(_reservationHelper.GetReservations());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet]
    public IActionResult Info(Guid id)
    {
        try
        {
            return View(_reservationHelper.GetInfoReservationViewModel(id));
        }
        catch (Exception e)
        {
            return RedirectToAction("List");
        }
    }
    
    [HttpGet]
    public IActionResult Update(Guid id)
    {
        try
        {
            return View(_reservationHelper.GetUpdateReservationViewModel(id));
        }
        catch (Exception e)
        {
            return RedirectToAction("Info", new {id = id});
        }
    }
    
    [HttpPost]
    public IActionResult Update(CreateReservationViewModel model)
    {
        try
        {
            
            _reservationHelper.UpdateReservation(model);
            return RedirectToAction("List");
        }
        catch (Exception e)
        {
            return View(_reservationHelper.GetCreateReservationViewModelException(model));
        }
    }
    
}