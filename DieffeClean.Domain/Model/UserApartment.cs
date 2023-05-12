namespace DieffeClean.Domain.Model;

public class UserApartment
{
    public Guid Id { get; set; }
    public string MyUserId { get; set; }
    public MyUser MyUser { get; set; }
    public Guid ApartmentId { get; set; }
    public Apartment Apartment { get; set; }
}