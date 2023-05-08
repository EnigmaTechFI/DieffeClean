using DieffeClean.Domain.Model;
using DieffeClean.Domain.Services;
using DieffeClean.Presentation.Model.Apartment;

namespace DieffeClean.Presentation.Helper;

public class ApartmentHelper
{
    private readonly IApartmentService _apartmentService;

    public ApartmentHelper(IApartmentService apartmentService)
    {
        _apartmentService = apartmentService;
    }

    public ListApartmentsViewModel GetApartments()
    {
        return new ListApartmentsViewModel()
        {
            Apartments = _apartmentService.GetAll()
        };
    }

    public void CreateApartment(CreateApartmentViewModel model)
    {
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
        _apartmentService.Update(model.Apartment);
    }
}