using HostMaster.Backend.Helpers;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace HostMaster.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public SeedDb(DataContext context, IFileStorage fileStorage, IUsersUnitOfWork usersUnitOfWork)
    {
        _context = context;
        _fileStorage = fileStorage;
        _usersUnitOfWork = usersUnitOfWork;
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
        await CheckCustomersDbAsync();
        await CheckReservationsDbAsync();
        await CheckRolesAsync();
        await CheckUserAsync("admin", "admin", "admin@yopmail.com", "322 311 4620", UserType.Admin);
        await CheckExtraServicesDbAsync();
        await CheckMaintenancesDbAsync();
        await CheckOpinionsDbAsync();
    }

    private async Task CheckRolesAsync()
    {
        await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
        await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
    }

    private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, UserType userType)
    {
        var user = await _usersUnitOfWork.GetUserAsync(email);
        if (user == null)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Name == "Colombia");
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Country = country!,
                UserType = userType,
            };

            await _usersUnitOfWork.AddUserAsync(user, "123456");
            await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());

            var token = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
            await _usersUnitOfWork.ConfirmEmailAsync(user, token);
        }

        return user;
    }

    private async Task CheckCountriesDbAsync()
    {
        if (!_context.Countries.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedCountries.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckStatesDbAsync()
    {
        if (!_context.States.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedStates.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckCitiesDbAsync()
    {
        if (!_context.Cities.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedCities.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckAccommodationsDbAsync()
    {
        if (!_context.Accommodations.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedAccommodations.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckRoomsDbAsync()
    {
        if (!_context.Rooms.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedRooms.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckRoomsTypesDbAsync()
    {
        if (!_context.RoomTypes.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedRoomTypes.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckCustomersDbAsync()
    {
        if (!_context.Customers.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedCustomers.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckReservationsDbAsync()
    {
        if (!_context.Reservations.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedReservations.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckExtraServicesDbAsync()
    {
        if (!_context.ExtraServices.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedExtraServices.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckMaintenancesDbAsync()
    {
        if (!_context.Maintenances.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedMaintenances.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckOpinionsDbAsync()
    {
        if (!_context.Opinions.Any())
        {
            var sqlScriptPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedOpinions.sql");
            var SQLScript = File.ReadAllText(sqlScriptPath);
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }
}