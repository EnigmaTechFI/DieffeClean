using DieffeClean.Domain.Model;

namespace DieffeClean.Domain.Services;

public interface IApartmentService
{
    public List<Apartment> GetAll();
    public Apartment GetById(Guid id);
    void Create(Apartment apartment);
    void Update(Apartment apartment);
    List<Apartment> GetAllWithReservationsByNow();
    List<Apartment> GetAllWithReservationsByNowByUserId(string userId);
    List<Apartment> GetAllByUserId(string userId);
}