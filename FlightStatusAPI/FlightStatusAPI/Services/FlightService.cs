using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

public class FlightService : IFlightService
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _cache;

    public FlightService(ApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public IEnumerable<Flight> GetFlights(string origin, string destination)
    {
        string cacheKey = $"flights_{origin}_{destination}";
        
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<Flight> flights))
        {
            var query = _context.Flights.AsQueryable();

            if (!string.IsNullOrEmpty(origin))
            {
                query = query.Where(f => f.Origin == origin);
            }

            if (!string.IsNullOrEmpty(destination))
            {
                query = query.Where(f => f.Destination == destination);
            }

            flights = query.ToList();

            // Установить кэш с временем жизни 5 минут
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, flights, cacheEntryOptions);
        }

        return flights;
    }

    public void AddFlight(Flight flight)
    {
        _context.Flights.Add(flight);
        _context.SaveChanges();

        // Очистить кэш после добавления нового рейса
        _cache.Remove($"flights_{flight.Origin}_{flight.Destination}");
    }

    public void UpdateFlight(int id, Flight flight)
    {
        var existingFlight = _context.Flights.Find(id);
        if (existingFlight != null)
        {
            existingFlight.Origin = flight.Origin;
            existingFlight.Destination = flight.Destination;
            existingFlight.Departure = flight.Departure;
            existingFlight.Arrival = flight.Arrival;
            existingFlight.Status = flight.Status;

            _context.SaveChanges();

            // Очистить кэш после обновления рейса
            _cache.Remove($"flights_{existingFlight.Origin}_{existingFlight.Destination}");
        }
    }
}
