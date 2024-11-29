﻿using HostMaster.Backend.Helpers;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

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
        await CheckUserAsync("user", "user", "user@yopmail.com", "322 000 4620", UserType.User);
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

    private string GetFilePath(string relativePath)
    {
        var basePath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Data\\" : "Data/";
        return Path.Combine(basePath, relativePath);
    }

    private async Task CheckCountriesDbAsync()
    {
        if (!_context.Countries.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedCountries.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckStatesDbAsync()
    {
        if (!_context.States.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedStates.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckCitiesDbAsync()
    {
        if (!_context.Cities.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedCities.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckAccommodationsDbAsync()
    {
        if (!_context.Accommodations.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedAccommodations.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckRoomsDbAsync()
    {
        if (!_context.Rooms.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedRooms.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckRoomsTypesDbAsync()
    {
        if (!_context.RoomTypes.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedRoomTypes.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckCustomersDbAsync()
    {
        if (!_context.Customers.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedCustomers.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckReservationsDbAsync()
    {
        if (!_context.Reservations.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedReservations.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckExtraServicesDbAsync()
    {
        if (!_context.ExtraServices.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedExtraServices.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckMaintenancesDbAsync()
    {
        if (!_context.Maintenances.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedMaintenances.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }

    private async Task CheckOpinionsDbAsync()
    {
        if (!_context.Opinions.Any())
        {
            var SQLScript = File.ReadAllText(GetFilePath("SeedOpinions.sql"));
            await _context.Database.ExecuteSqlRawAsync(SQLScript);
        }
    }
}