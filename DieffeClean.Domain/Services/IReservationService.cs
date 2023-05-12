using DieffeClean.Domain.Model;

namespace DieffeClean.Domain.Services;

public interface IReservationService 
{
    public List<Reservation> GetAll();
    public List<Reservation> GetOpenedOrFutureReservationsByApartmentId(Guid apartmentId);
    void Create(Reservation reservation);
}