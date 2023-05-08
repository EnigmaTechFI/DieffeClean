using DieffeClean.Domain.Services;
using DieffeClean.Presentation.Model.Reservation;

namespace DieffeClean.Presentation.Helper;

public class ReservationHelper
{
    private readonly IReservationService _reservationService;

    public ReservationHelper(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    public ListReservationViewModel GetReservations()
    {
        return new ListReservationViewModel()
        {
            Reservations = _reservationService.GetAll()
        };
    }
}