using System.Globalization;
using DieffeClean.Domain.Constants;
using DieffeClean.Domain.Model;
using DieffeClean.Domain.Services;
using DieffeClean.Presentation.Model.Reservation;
using DieffeClean.Utils.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DieffeClean.Presentation.Helper;

public class ReservationHelper
{
    private readonly IReservationService _reservationService;
    private readonly IApartmentService _apartmentService;
    private readonly UserManager<MyUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly IStaffService _staffService;



    public ReservationHelper(IReservationService reservationService, IStaffService staffService, IEmailSender emailSender, IApartmentService apartmentService, UserManager<MyUser> userManager)
    {
        _reservationService = reservationService;
        _apartmentService = apartmentService;
        _userManager = userManager;
        _emailSender = emailSender;
        _staffService = staffService;
    }

    public async Task<ListReservationViewModel> GetReservations(MyUser user)
    {

        var reservation = await _userManager.IsInRoleAsync(user, Roles.SuperAdmin)
            ? _reservationService.GetAll()
            : _reservationService.GetAllByUserId(user.Id);
        
        return new ListReservationViewModel()
        {
            Reservations = reservation
        };
    }

    public async Task<CreateReservationViewModel> GetCreateReservationViewModel(MyUser user)
    {
        var apartment = await _userManager.IsInRoleAsync(user, Roles.SuperAdmin)
            ? _apartmentService.GetAll()
            : _apartmentService.GetAllByUserId(user.Id);
        
        return new CreateReservationViewModel()
        {
            Apartments = apartment
        };
    }

    public async void CreateReservation(CreateReservationViewModel model)
    {
        ParseDates(model);
        ValidateReservationDate(model.Reservation, DateTime.Now);
        ValidateReservation(model.Reservation);
        _reservationService.Create(model.Reservation);
        
        //Per ogni email di pulizie collegato alla reservation
        var staffList = _staffService.GetStaff().Where(s => _userManager.IsInRoleAsync(s, "CleaningUser").Result).ToList();

        for (int i = 0; i < staffList.Count; i++)
        {
            if (staffList[i].UserApartments.Any(s => s.ApartmentId == model.Reservation.Apartment.Id))
            {
                var message = new Message(new (string, string)[] { (staffList[i].UserName, staffList[i].Email) },
                    "Nuova prenotazione", "Nuova penotazione");
                List<(string, string)> replacer = new List<(string, string)>
                {
                    ("[apartment]", model.Reservation.Apartment.Name),
                    ("[datain]", model.Reservation.CheckIn.ToString()),
                    ("[dataout]", model.Reservation.CheckOut.ToString())
                };
                var currentPath = Directory.GetCurrentDirectory();
                try
                {
                    await _emailSender.SendEmailAsync(message, currentPath + "/wwwroot/MailTemplate/new-reservation.html",
                        replacer);
                }
                catch (Exception e)
                {
                    throw new Exception("Emaildi notifica non inviata");
                }
            }
        }
    }

    private void ValidateReservation(Reservation reservation)
    {
        if (reservation.ApartmentId == Guid.Empty)
            throw new Exception("Selezionare appartamento.");
        if (reservation.NumberGuests <= 0)
            throw new Exception("Inserire numero ospiti.");
        if (string.IsNullOrEmpty(reservation.GuestFirstName))
            throw new Exception("Inserire nome ospite principale.");
        if (string.IsNullOrEmpty(reservation.GuestLastName))
            throw new Exception("Inserire cognome ospite principale.");
        if (string.IsNullOrEmpty(reservation.PhoneNumber))
            throw new Exception("Inserire telefono ospite principale.");
        if (string.IsNullOrEmpty(reservation.Email))
            throw new Exception("Inserire email ospite principale.");
    }

    public void UpdateReservation(CreateReservationViewModel model)
    {
        ParseDates(model);
        ValidateReservationDate(model.Reservation, DateTime.Now);
        ValidateReservation(model.Reservation);
        _reservationService.Update(model.Reservation);
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

        var allReservation = reservation.Id == Guid.Empty ? _reservationService.GetOpenedOrFutureReservationsByApartmentId(reservation.ApartmentId) : _reservationService.GetOpenedOrFutureReservationsByApartmentId(reservation.ApartmentId, reservation.Id);
            
        var dateIntervalIsValid= allReservation.All(a =>
        {
            if (reservation.Id == a.Id) return true;
            return reservation.CheckIn >= a.CheckOut || reservation.CheckOut <= a.CheckIn;
        });
        if (!dateIntervalIsValid)
            throw new Exception("Prenotazione esistente per le date inserite.");
    }

    public async Task<CreateReservationViewModel> GetCreateReservationViewModelException(CreateReservationViewModel model, MyUser user)
    {
        model.Apartments = await _userManager.IsInRoleAsync(user, Roles.SuperAdmin)
            ? _apartmentService.GetAll()
            : _apartmentService.GetAllByUserId(user.Id);
        return model;
    }

    public InfoReservationViewModel GetInfoReservationViewModel(Guid id)
    {
        return new InfoReservationViewModel()
        {
            Reservation = _reservationService.GetReservationById(id)
        };
    }

    public async Task<CreateReservationViewModel> GetUpdateReservationViewModel(Guid id, MyUser user)
    {
        var reservation = _reservationService.GetReservationById(id);
        var apartment = await _userManager.IsInRoleAsync(user, Roles.SuperAdmin)
            ? _apartmentService.GetAll()
            : _apartmentService.GetAllByUserId(user.Id);
        
        return new CreateReservationViewModel()
        {
            Apartments = apartment,
            Reservation = reservation,
            CheckIn = reservation.CheckIn.ToString("yyyy-MM-dd HH:mm"),
            CheckOut = reservation.CheckOut.ToString("yyyy-MM-dd HH:mm")
        };
    }

    public async Task<CalendarViewModel> GetCalendarViewModel(MyUser user)
    {
        var apartment = await _userManager.IsInRoleAsync(user, Roles.SuperAdmin)
            ? _apartmentService.GetAllWithReservationsByNow()
            : _apartmentService.GetAllWithReservationsByNowByUserId(user.Id);
        
        return new CalendarViewModel()
        {
            Apartments = apartment
        };
    }

    public void DeleteReservation(Guid id)
    {
        var reservation = _reservationService.GetReservationById(id);
        _reservationService.Delete(reservation);
    }
}