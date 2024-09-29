using HostMaster.Shared.Entities;
using HostMaster.Shared.Enums;
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

        await CheckCountriesAsync();
        await CheckDbAsync();
    }

    private async Task CheckDbAsync()
    {
        if (!_context.Rooms.Any())
        {
            var SQLScript = File.ReadAllText("Data\\Seed.sql");
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

            _context.Countries.Add(new Country
            {
                Name = "United States",
                States = new List<State>()
        {
            new State()
            {
                Name = "California",
                Cities = new List<City>()
                {
                    new City() { Name = "Los Angeles" },
                    new City() { Name = "San Francisco" },
                    new City() { Name = "San Diego" },
                    new City() { Name = "Sacramento" },
                    new City() { Name = "San Jose" }
                }
            },
            new State()
            {
                Name = "Texas",
                Cities = new List<City>()
                {
                    new City() { Name = "Houston" },
                    new City() { Name = "Dallas" },
                    new City() { Name = "Austin" },
                    new City() { Name = "San Antonio" },
                    new City() { Name = "Fort Worth" }
                }
            }
        }
            });

            _context.Countries.Add(new Country
            {
                Name = "United Kingdom",
                States = new List<State>()
        {
            new State()
            {
                Name = "England",
                Cities = new List<City>()
                {
                    new City() { Name = "London" },
                    new City() { Name = "Manchester" },
                    new City() { Name = "Birmingham" },
                    new City() { Name = "Liverpool" },
                    new City() { Name = "Leeds" }
                }
            },
            new State()
            {
                Name = "Scotland",
                Cities = new List<City>()
                {
                    new City() { Name = "Edinburgh" },
                    new City() { Name = "Glasgow" },
                    new City() { Name = "Aberdeen" },
                    new City() { Name = "Dundee" },
                    new City() { Name = "Inverness" }
                }
            }
        }
            });
        }

        await _context.SaveChangesAsync();
    }

    private async Task CheckUserAsync(string firstName, string lastName, string email, string phone, UserType userType)
    {
        if (!_context.Users.Any())
        {
            _context.Users.Add(new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phone,
                UserType = userType,
                DocumentType = "CC",
                Document = "22113344"
            });
        }
        await _context.SaveChangesAsync();
    }
}