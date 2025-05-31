namespace TripClientApp.DTOs;

public class TripsRequest
{
    public int pageNum { get; set; }
    public int pageSize { get; set; }
    public int allPages { get; set; }
    public IEnumerable<TripGetDto> trips { get; set; }
}