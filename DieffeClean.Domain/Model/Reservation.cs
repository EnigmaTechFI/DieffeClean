namespace DieffeClean.Domain.Model;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid ApartmentId { get; set; }
    public Apartment Apartment { get; set; }
    public string GuestFirstName { get; set; }
    public string GuestLastName { get; set; }
    public string GuestFullName => this.GuestFirstName + " " + this.GuestLastName;
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CheckIn { get; set; }
    public bool CheckInAllDay { get; set; }
    public DateTime CheckOut { get; set; }
    public bool CheckOutAllDay { get; set; }
    public int NumberGuests { get; set; }
}