namespace TripClientApp.DTOs;

public class TripGetDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public IEnumerable<CountryDto> Countries { get; set; }
    public IEnumerable<ClientDto> Clients { get; set; }
}

public class CountryDto
{
    public string Name { get; set; }
}

public class ClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}