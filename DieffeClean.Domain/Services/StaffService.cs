using DieffeClean.Domain.Data;
using DieffeClean.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DieffeClean.Domain.Services;

public class StaffService : IStaffService 
{
    private readonly ApplicationDbContext _dbContext;

    public StaffService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<MyUser> GetStaff()
    {
        return _dbContext.MyUsers.Include(s => s.UserApartments).ThenInclude(p => p.Apartment).ToList();
    }

    public void SetUserApartments(string UserId, List<Guid> Apartaments)
    {
        
        foreach (var apartmentId in Apartaments)
        {
            UserApartment Apart = new UserApartment();
            Apart.MyUserId = UserId;
            Apart.ApartmentId = apartmentId;
            
            _dbContext.UserApartments.Add(Apart);
        }
        
        _dbContext.SaveChanges();

    }
    
    public MyUser GetStaffById(string id)
    {
        return _dbContext.MyUsers.Include(s => s.UserApartments)
            .ThenInclude(p => p.Apartment)
            .SingleOrDefault<DieffeClean.Domain.Model.MyUser>(u => u.Id == id);
    }
    
    public bool DeleteStaffById(string staffId)
    {
        var user = _dbContext.MyUsers.FirstOrDefault(u => u.Id == staffId);
        if (user != null)
        {
            _dbContext.MyUsers.Remove(user);
            _dbContext.SaveChanges();
            return true;
        }
        return false;
    }

}