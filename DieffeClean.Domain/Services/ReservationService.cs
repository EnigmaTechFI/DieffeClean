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
}