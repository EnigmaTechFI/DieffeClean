using DieffeClean.Domain.Data;
using DieffeClean.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DieffeClean.Domain.Services;

public class ReservationService : IReservationService 
{
    private readonly ApplicationDbContext _dbContext;

    public ReservationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Reservation> GetAll()
    {
        return _dbContext.Reservations.Include(s => s.Apartment).ToList();
    }

    public List<Reservation> GetOpenedOrFutureReservationsByApartmentId(Guid apartmentId)
    {
        return _dbContext.Reservations
            .Where(s => s.ApartmentId == apartmentId && s.CheckOut.Date >= DateTime.Now.Date)
            .ToList();
    }
    public List<Reservation> GetOpenedOrFutureReservationsByApartmentId(Guid apartmentId, Guid reservationid)
    {
        return _dbContext.Reservations
            .Where(s => s.ApartmentId == apartmentId && s.CheckOut.Date >= DateTime.Now.Date && s.Id != reservationid)
            .ToList();
    }

    public void Create(Reservation reservation)
    {
        _dbContext.Reservations.Add(reservation);
        _dbContext.SaveChanges();
    }

    public Reservation GetReservationById(Guid id)
    {
        return _dbContext.Reservations.Include(s => s.Apartment).SingleOrDefault(s => s.Id == id);
    }

    public void Update(Reservation reservation)
    {
        _dbContext.Reservations.Update(reservation);
        _dbContext.SaveChanges();
    }

    public List<Reservation> GetAllByNow()
    {
        return _dbContext.Reservations.Where(s => s.CheckOut >= DateTime.Now).ToList();
    }

    public List<Reservation> GetAllByUserId(string userId)
    {
        return _dbContext.Reservations
            .Include(s => s.Apartment)
            .ThenInclude(s => s.UserApartments)
            .Where(s => s.Apartment.UserApartments.Any(s => s.MyUserId == userId))
            .ToList();
    }
}