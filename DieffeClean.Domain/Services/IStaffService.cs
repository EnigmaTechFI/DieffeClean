using DieffeClean.Domain.Model;

namespace DieffeClean.Domain.Services;

public interface IStaffService 
{
    public List<MyUser> GetStaff();
   
}