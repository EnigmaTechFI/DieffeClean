using DieffeClean.Presentation.Helper;
using DieffeClean.Presentation.Model.Apartment;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace DieffeClean.WebApp.Controllers;

public class ApartmentController : Controller
{
    private readonly ApartmentHelper _apartmentHelper;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public ApartmentController(ApartmentHelper apartmentHelper)
    {
        _apartmentHelper = apartmentHelper;
    }

    [HttpGet]
    public IActionResult List()
    {
        try
        {
            return View(_apartmentHelper.GetApartments());
        }
        catch (Exception e)
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]
    public IActionResult Info(Guid id)
    {
        try
        {
            return View(_apartmentHelper.GetApartmentById(id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public IActionResult Update(Guid id)
    {
        try
        {
            return View(_apartmentHelper.GetApartmentById(id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPost]
    public IActionResult Update(CreateApartmentViewModel model)
    {
        try
        {
            _apartmentHelper.UpdateApartment(model);
            return RedirectToAction("Info", new {id = model.Apartment.Id});
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(CreateApartmentViewModel model)
    {
        try
        {
            _apartmentHelper.CreateApartment(model);
            return RedirectToAction("List");
        }
        catch (Exception e)
        {
            return View(model);
        }
    }
}