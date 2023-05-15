using DieffeClean.Domain.Model;

namespace DieffeClean.Domain.Services;

public interface IStaffService 
{
    public List<MyUser> GetStaff();
    
    public MyUser GetStaffById(string id);

    public bool DeleteStaffById(string staffId);

    public void CreateUserApartments(List<UserApartment> userApartments);
    public void UpdateUserApartments(string UserId, List<UserApartment> Apartaments);

}