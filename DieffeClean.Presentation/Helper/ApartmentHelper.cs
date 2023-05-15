using DieffeClean.Domain.Constants;
using DieffeClean.Domain.Model;
using DieffeClean.Domain.Services;
using DieffeClean.Presentation.Model.Apartment;
using Microsoft.AspNetCore.Identity;

namespace DieffeClean.Presentation.Helper;

public class ApartmentHelper
{
    private readonly IApartmentService _apartmentService;
    private readonly UserManager<MyUser> _userManager;

    public ApartmentHelper(IApartmentService apartmentService, UserManager<MyUser> userManager)
    {
        _apartmentService = apartmentService;
        _userManager = userManager;
    }

    public async Task<ListApartmentsViewModel> GetApartments(MyUser user)
    {
        var apartment = await _userManager.IsInRoleAsync(user, Roles.SuperAdmin)
            ? _apartmentService.GetAll()
            : _apartmentService.GetAllByUserId(user.Id);
        
        return new ListApartmentsViewModel()
        {
            Apartments = apartment
        };
    }

    public void CreateApartment(CreateApartmentViewModel model)
    {
        ValidateApartment(model.Apartment);
        _apartmentService.Create(model.Apartment);
    }

    public CreateApartmentViewModel GetApartmentById(Guid id)
    {
        return new CreateApartmentViewModel()
        {
            Apartment = _apartmentService.GetById(id)
        };
    }

    public void UpdateApartment(CreateApartmentViewModel model)
    {
        ValidateApartment(model.Apartment);
        _apartmentService.Update(model.Apartment);
    }

    private void ValidateApartment(Apartment apartment)
    {
        if (string.IsNullOrEmpty(apartment.Name))
            throw new Exception("Inserire nome dell'appartamento");
        if (string.IsNullOrEmpty(apartment.Address))
            throw new Exception("Inserire indirizzo dell'appartamento");
        if (string.IsNullOrEmpty(apartment.Nation))
            throw new Exception("Inserire nazione dell'appartamento");
        if (string.IsNullOrEmpty(apartment.Province))
            throw new Exception("Inserire povincia dell'appartamento");
        if (string.IsNullOrEmpty(apartment.City))
            throw new Exception("Inserire città dell'appartamento");
        if (apartment.ZipCode < 10000 || apartment.ZipCode > 99999)
            throw new Exception("Il codice postale deve essere un valore composto da 5 cifre");

    }
}