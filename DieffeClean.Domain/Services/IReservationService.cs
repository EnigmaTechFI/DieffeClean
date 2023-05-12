using DieffeClean.Domain.Model;

namespace DieffeClean.Domain.Services;

public interface IReservationService 
{
    public List<Reservation> GetAll();
    public List<Reservation> GetOpenedOrFutureReservationsByApartmentId(Guid apartmentId);
    public List<Reservation> GetOpenedOrFutureReservationsByApartmentId(Guid apartmentId, Guid reservationid);
    void Create(Reservation reservation);
    Reservation GetReservationById(Guid id);
    void Update(Reservation reservation);
    List<Reservation> GetAllByNow();
}