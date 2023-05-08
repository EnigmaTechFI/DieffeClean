using DieffeClean.Domain.Model;

namespace DieffeClean.Domain.Services;

public interface IReservationService 
{
    List<Reservation> GetAll();
}