using System.Globalization;
using DieffeClean.Domain.Model;
using DieffeClean.Domain.Services;
using DieffeClean.Presentation.Model.Reservation;

namespace DieffeClean.Presentation.Helper;

public class ReservationHelper
{
    private readonly IReservationService _reservationService;
    private readonly IApartmentService _apartmentService;

    public ReservationHelper(IReservationService reservationService, IApartmentService apartmentService)
    {
        _reservationService = reservationService;
        _apartmentService = apartmentService;
    }

    public ListReservationViewModel GetReservations()
    {
        return new ListReservationViewModel()
        {
            Reservations = _reservationService.GetAll()
        };
    }

    public CreateReservationViewModel GetCreateReservationViewModel()
    {
        return new CreateReservationViewModel()
        {
            Apartments = _apartmentService.GetAll()
        };
    }

    public void CreateReservation(CreateReservationViewModel model)
    {
        ParseDates(model);
        ValidateReservationDate(model.Reservation, DateTime.Now);
        _reservationService.Create(model.Reservation);
    }
    
    private void ParseDates(CreateReservationViewModel model)
    {
        var parseCheckInDate = DateTime.TryParse(model.CheckIn, new CultureInfo("it-it"), DateTimeStyles.None, out var resCheckIn);
        model.Reservation.CheckIn = parseCheckInDate ? resCheckIn : DateTime.MinValue;
        var parseCheckOutDate = DateTime.TryParse(model.CheckOut, new CultureInfo("it-it"), DateTimeStyles.None, out var resCheckOut);
        model.Reservation.CheckOut = parseCheckOutDate ? resCheckOut : DateTime.MinValue;
    }
    
    private void ValidateReservationDate(Reservation reservation, DateTime now)
    {
        if (reservation.CheckIn >= reservation.CheckOut ||
            DateTime.Compare(reservation.CheckIn.ToUniversalTime(), now) < 0)
            throw new Exception("Le date di checkin inserite non sono corrette.");

        var allReservation = _reservationService.GetOpenedOrFutureReservationsByApartmentId(reservation.ApartmentId);
            
        var dateIntervalIsValid= allReservation.All(a =>
        {
            if (reservation.Id == a.Id) return true;
            return reservation.CheckIn >= a.CheckOut || reservation.CheckOut <= a.CheckIn;
        });
        if (!dateIntervalIsValid)
            throw new Exception("Prenotazione esistente per le date inserite.");
            
    }

    public CreateReservationViewModel GetCreateReservationViewModelException(CreateReservationViewModel model)
    {
        model.Apartments = _apartmentService.GetAll();
        return model;
    }
}