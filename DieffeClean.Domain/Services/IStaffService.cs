using DieffeClean.Domain.Model;

namespace DieffeClean.Domain.Services;

public interface IStaffService 
{
    public List<MyUser> GetStaff();

    public void SetUserApartments(string UserId, string[] Apartaments);

    public MyUser GetStaffById(string id);

    public bool DeleteStaffById(string staffId);



}