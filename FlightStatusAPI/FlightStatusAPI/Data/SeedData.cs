using Microsoft.Extensions.DependencyInjection;
using System;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        // Проверка, есть ли уже данные
        if (context.Flights.Any())
        {
            return; // БД уже заполнена
        }

        context.Flights.AddRange(
            new Flight { Origin = "ALA", Destination = "NQZ", Departure = DateTimeOffset.Now, Arrival = DateTimeOffset.Now.AddHours(6), Status = "InTime" },
            new Flight { Origin = "NQZ", Destination = "ALA", Departure = DateTimeOffset.Now.AddHours(1), Arrival = DateTimeOffset.Now.AddHours(7), Status = "Delayed" }
        );

        context.SaveChanges();
    }
}
