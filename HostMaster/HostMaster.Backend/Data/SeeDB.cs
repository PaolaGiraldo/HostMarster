using HostMaster.Backend.Helpers;
using HostMaster.Backend.UnitsOfWork.Implementations;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork)
    {
        _context = context;
        _usersUnitOfWork = usersUnitOfWork;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesAsync();
        await CheckDbAsync();
        await CheckUserAsync("Javier", "Pedroza", "javierpedroza@yopmail.com", "322 311 4620", UserType.Admin);
    }

    private async Task CheckDbAsync()
    {
        if (!_context.Rooms.Any())
        {
            var SQLScript = File.ReadAllText("Data\\Seed.sql");
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckAccommodationsAsync()
    {
        if (!_context.Accommodations.Any())
        {
            _context.Accommodations.Add(new Accommodation
            {
                Name = "Hotel de la Montaña",
                Address = "Calle 10 # 20 - 30",

                City = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Medellín"),
                CityId = await _context.Cities.Where(x => x.Name == "Medellín").Select(x => x.Id).FirstOrDefaultAsync(),
                Description = "Hotel de la montaña es un hotel de lujo ubicado en el centro de la ciudad",
                PhoneNumber = "322 311 4620",
            });
        }
        await _context.SaveChangesAsync();
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