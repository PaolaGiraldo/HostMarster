using HostMaster.Backend.Data;
using HostMaster.Backend.Helpers;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Repositories.Implementations;

public class UsersRepository : GenericRepository<User>, IUsersRepository
{
    private readonly DataContext _dataContext;

    public UsersRepository(DataContext context) : base(context)
    {
        _dataContext = context;
    }

    public async Task<ActionResponse<User>> AddAsync(UserDTO userDTO)
    {
        var user = new User
        {
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Email = userDTO.Email,
            PhoneNumber = userDTO.Phone,
        };

        _dataContext.Users.Add(user);
        try
        {
            await _dataContext.SaveChangesAsync();
            return new ActionResponse<User>
            {
                WasSuccess = true,
                Result = user
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<User>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<User>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public Task<IdentityResult> AddUserAsync(User user, string password)
    {
        throw new NotImplementedException();
    }

    public Task AddUserToRoleAsync(User user, string roleName)
    {
        throw new NotImplementedException();
    }

    public Task CheckRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }

    public override async Task<ActionResponse<IEnumerable<User>>> GetAsync()
    {
        var users = await _dataContext.Users.ToListAsync();

        return new ActionResponse<IEnumerable<User>>
        {
            WasSuccess = true,
            Result = users
        };
    }

    public override async Task<ActionResponse<IEnumerable<User>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _dataContext.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.FirstName.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<User>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.FirstName)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public Task<User> GetUserAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUserInRoleAsync(User user, string roleName)
    {
        throw new NotImplementedException();
    }
}