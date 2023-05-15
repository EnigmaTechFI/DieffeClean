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

    public MyUser GetStaffById(string id)
    {
        return _dbContext.MyUsers
            .Include(s => s.UserApartments)
            .ThenInclude(p => p.Apartment)
            .SingleOrDefault(u => u.Id == id);
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

    public void UpdateUserApartments(string UserId, List<UserApartment> userApartments)
    {

        var userApartmentsToDelete = _dbContext.UserApartments.Where(ua => ua.MyUserId == UserId);
        _dbContext.UserApartments.RemoveRange(userApartmentsToDelete);
        _dbContext.SaveChanges();

        _dbContext.UserApartments.AddRange(userApartments);
        _dbContext.SaveChanges();
        
    }

    public void CreateUserApartments(List<UserApartment> userApartments)
    {
        _dbContext.UserApartments.AddRange(userApartments);
        _dbContext.SaveChanges();
    }
}