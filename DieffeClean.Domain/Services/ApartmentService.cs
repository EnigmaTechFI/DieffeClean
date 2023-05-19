using DieffeClean.Domain.Data;
using DieffeClean.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DieffeClean.Domain.Services;

public class ApartmentService : IApartmentService
{
    private readonly ApplicationDbContext _dbContext;

    public ApartmentService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Apartment> GetAll()
    {
        return _dbContext.Apartments.ToList();
    }

    public Apartment GetById(Guid id)
    {
        return _dbContext.Apartments.SingleOrDefault(s => s.Id == id);
    }

    public void Create(Apartment apartment)
    {
        _dbContext.Apartments.Add(apartment);
        _dbContext.SaveChanges();
    }

    public void Update(Apartment apartment)
    {
        _dbContext.Apartments.Update(apartment);
        _dbContext.SaveChanges();
    }

    public List<Apartment> GetAllWithReservationsByNow()
    {
        return _dbContext.Apartments.Include(s => s.Reservations.Where(s => s.CheckOut >= DateTime.Now)).ToList();
    }

    public List<Apartment> GetAllWithReservationsByNowByUserId(string userId)
    {
        return _dbContext.Apartments
            .Include(s => s.Reservations.Where(s => s.CheckOut >= DateTime.Now))
            .Include(s => s.UserApartments)
            .Where(s => s.UserApartments.Any(s => s.MyUserId == userId))
            .ToList();
    }

    public List<Apartment> GetAllByUserId(string userId)
    {
        return _dbContext.Apartments
            .Include(s => s.UserApartments)
            .Where(s => s.UserApartments.Any(s => s.MyUserId == userId))
            .ToList();
    }

    public void Delete(Apartment apartment)
    {
        _dbContext.Apartments.Remove(apartment);
        _dbContext.SaveChanges();
    }
}