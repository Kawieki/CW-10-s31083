using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TripClientApp.DTOs;
using TripClientApp.Exceptions;
using TripClientApp.Models;

namespace TripClientApp.Services;

public interface IDbService
{
    public Task<TripsRequest> GetAllTripsAsync(int page ,int pageSize);
    public Task DeleteClientAsync(int id);
    public Task<ClientTripGetDto> AddClientToTrip(int tripId, ClientTripCreateDto clientDto);
}

public class DbService(TripsDbContext data) : IDbService
{
    public async Task<TripsRequest> GetAllTripsAsync(int page, int pageSize)
    {
        var totalCount = await data.Trips.CountAsync();
        
        var trips = await data.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripGetDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(country => new CountryDto
                {
                    Name = country.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDto
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            })
            .ToListAsync();
        
        return new TripsRequest
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = (int)Math.Ceiling((double)totalCount / pageSize),
            trips = trips
        };
    }

    public async Task DeleteClientAsync(int id)
    {
        var client = await data.Clients.FirstOrDefaultAsync(c => c.IdClient == id);

        if (client is null)
        {
            throw new NotFoundException($"Client with id: {id} has not been found!");
        }

        var hasTrips = await data.ClientTrips.AnyAsync(ct => ct.IdClient == client.IdClient);
        
        if (hasTrips)
        {
            throw new ClientHasTripsException(
                "The client already has assigned trips and cannot perform this operation.");
        }

        data.Clients.Remove(client);
        await data.SaveChangesAsync();
    }

    public async Task<ClientTripGetDto> AddClientToTrip(int tripId, ClientTripCreateDto clientDto)
    {
        /* Warunek 1:  Sprawdzić czy klient o takim numerze PESEL już
         istnieje - jeśli tak zwracamy błąd */
        
        var clientExists = await data.Clients.AnyAsync(c => c.Pesel.Equals(clientDto.Pesel));
        if (clientExists)
        {
            throw new ClientAlreadyExistsException("Client already exists!");
        }
        
        /* ========================== WAŻNE!!!!!! ==============================
         Byl jeszcze warunek: Czy klient o takim numerze PESEL jest już zapisany na
           daną wycieczkę - jeśli tak, zwracamy błąd.
           Nie dokonca rozumiem ktory z warunkow ma piorytet w zadaniu,
           jakby nie jest jasne czy przypisujemy istniejacego klienta do wycieczki
           czy tworzymy nowego klienta i przypisujemy mu wycieczke.
           ale zadecydowalem ze wzialem 2 opcje i tworze nowego klienta,
           wiec klient napewno nie bedzie mial wycieczeki.
           ========================== WAŻNE!!!!!! ==============================
         */

        var trip = await data.Trips.FirstOrDefaultAsync(t => t.IdTrip == tripId);
        
        if (trip is null)
        {
            throw new NotFoundException($"Trip with id: {tripId} does not exist!");
        }
        
        if (trip.DateFrom < DateTime.UtcNow)
        {
            throw new TripAlreadyStartedException("The trip has already started");
        }

        DateTime? paymentDateParsed = clientDto.PaymentDate != null ? ParseDate(clientDto.PaymentDate) : null;
        
        var newClient = new Client
        {
            FirstName = clientDto.FirstName,
            LastName = clientDto.LastName,
            Email = clientDto.Email,
            Telephone = clientDto.Telephone,
            Pesel = clientDto.Pesel
        };
        
        data.Clients.Add(newClient);
        await data.SaveChangesAsync();
        

        var newClientTrip = new ClientTrip
        {
            IdClient = newClient.IdClient,
            IdTrip = trip.IdTrip,
            PaymentDate = paymentDateParsed,
            RegisteredAt = DateTime.UtcNow
        };
        data.ClientTrips.Add(newClientTrip);
        await data.SaveChangesAsync();
        
        return new ClientTripGetDto
        {
            FirstName = newClient.FirstName,
            LastName = newClient.LastName,
            Email = newClient.Email,
            Telephone = newClient.Telephone,
            Pesel = newClient.Pesel,
            IdTrip = tripId,
            TripName = newClientTrip.IdTripNavigation.Name,
            paymentDate = newClientTrip.PaymentDate
        };
    }

    private DateTime ParseDate(string date)
    {
        // nie wiem ile formatow powinno byc akceptowanych ale dalem prawie wszystkie :~~~D :3 
        var acceptedFormats = new[]
        {
            "yyyy-MM-dd",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy/MM/dd",
            "yyyy/MM/dd HH:mm:ss",
            "dd/MM/yyyy",
            "dd/MM/yy",
            "dd-MM-yyyy",
            "dd-MM-yy",
            "MM/dd/yyyy",
            "M/d/yyyy",
            "MM/dd/yy",
            "M/d/yy",
            "yyyyMMdd"
        };
        
        if (!DateTime.TryParseExact(date, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            throw new FormatException("Invalid date format. Accepted formats: yyyy-MM-dd, dd/MM/yyyy, MM/dd/yyyy, etc.");
        }
        return parsedDate;
    }
}