using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using static MudBlazor.Colors;

namespace HostMaster.Backend.Repositories.Interfaces;

public interface IUsersRepository
{
    Task<ActionResponse<User>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<User>>> GetAsync();

    Task<ActionResponse<IEnumerable<User>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

    Task<ActionResponse<User>> AddAsync(UserDTO userDTO);
}