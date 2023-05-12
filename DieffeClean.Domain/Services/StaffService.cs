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
}