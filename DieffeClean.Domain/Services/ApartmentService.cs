using DieffeClean.Domain.Data;
using DieffeClean.Domain.Model;

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
}