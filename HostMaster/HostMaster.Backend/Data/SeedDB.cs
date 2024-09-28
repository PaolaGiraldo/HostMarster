using HostMaster.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesDbAsync();
        await CheckStatesDbAsync();
        await CheckCitiesDbAsync();
        await CheckAccommodationsDbAsync();
        await CheckRoomsTypesDbAsync();
        await CheckRoomsDbAsync();
        await CheckCountriesAsync();
    }

    private async Task CheckCountriesDbAsync()
    {
        if (!_context.Rooms.Any())
        {
            var SQLScript = File.ReadAllText("Data\\SeedCountries.sql");
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckStatesDbAsync()
    {
        if (!_context.Rooms.Any())
        {
            var SQLScript = File.ReadAllText("Data\\SeedStates.sql");
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckCitiesDbAsync()
    {
        if (!_context.Rooms.Any())
        {
            var SQLScript = File.ReadAllText("Data\\SeedCities.sql");
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckAccommodationsDbAsync()
    {
        if (!_context.Rooms.Any())
        {
            var SQLScript = File.ReadAllText("Data\\SeedAccommodations.sql");
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckRoomsDbAsync()
    {
        if (!_context.Rooms.Any())
        {
            var SQLScript = File.ReadAllText("Data\\SeedRooms.sql");
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckRoomsTypesDbAsync()
    {
        if (!_context.RoomTypes.Any())
        {
            var SQLScript = File.ReadAllText("Data\\SeedRoomTypes.sql");
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            _context.Countries.Add(new Country
            {
                Name = "Colombia",
                States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Antioquia",
                            Cities = new List<City>()
                            {
                                new City() { Name = "Medellín" },
                                new City() { Name = "Intagui" },
                                new City() { Name = "Envigado" },
                                new City() { Name = "Bello" },
                                new City() { Name = "Rionegro" }
                            }
                        },
                        new State()
                        {
                            Name = "Bogotá",
                            Cities = new List<City>()
                            {
                                new City() { Name = "Usaquen" },
                                new City() { Name = "Chapinero" },
                                new City() { Name = "Santa fe" },
                                new City() { Name = "Usme" },
                                new City() { Name = "Bosa" }
                            }
                        }
                    }
            });
        }

        await _context.SaveChangesAsync();
    }
}