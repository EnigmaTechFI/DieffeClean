namespace DieffeClean.Domain.Model;

public class Apartment
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PublicName { get; set; }
    public int NumberGuestrooms { get; set; } 
    public int NumberBathroom { get; set; }
    public int NumberBed { get; set; }
    public string? Note { get; set; }
    public string? WifiName { get; set; }
    public string? WifiPassword { get; set; }
    public string Address { get; set; }
    public int ZipCode { get; set; }
    public string City { get; set; }
    public string Nation { get; set; }
    public string Province { get; set; }
    public string IntercomName { get; set; }
    public string HostPhoneNumber { get; set; }
    public string EmailNotification { get; set; }
    public virtual List<Reservation> Reservations { get; set; }
    public virtual List<UserApartment> UserApartments { get; set; }
}