namespace TripClientApp.DTOs;

public class ClientTripCreateDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Pesel { get; set; }
    public string? PaymentDate { get; set; }
    /*
        Tu niby powinny sie pojawic informacje odnosnie wycieczki (id, nazwa itd)
        Ale przeciez wyszukujemy po id tripa z routa, 
        wiec tu tez nie jestem do konca pewny i zakladam
        ze id z routa jest wazniejsze (no i zachowuje spojnosc danych)
     */
}