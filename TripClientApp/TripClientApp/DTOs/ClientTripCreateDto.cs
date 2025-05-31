using System.ComponentModel.DataAnnotations;

namespace TripClientApp.DTOs;

public class ClientTripCreateDto
{
    [Required]
    [MaxLength(120)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string LastName { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Telephone { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Pesel { get; set; }
    public string? PaymentDate { get; set; }
    /*
        Tu niby powinny sie pojawic informacje odnosnie wycieczki (id, nazwa itd)
        Ale przeciez wyszukujemy po id tripa z routa, 
        wiec tu tez nie jestem do konca pewny i zakladam
        ze id z routa jest wazniejsze (no i zachowuje spojnosc danych)
     */
}