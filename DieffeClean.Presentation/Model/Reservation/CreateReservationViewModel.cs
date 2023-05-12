namespace DieffeClean.Presentation.Model.Reservation;

public class CreateReservationViewModel
{
    public Domain.Model.Reservation Reservation { get; set; }
    public List<Domain.Model.Apartment> Apartments { get; set; }
    public string CheckIn { get; set; }
    public string CheckOut { get; set; }
}