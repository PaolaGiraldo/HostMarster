using HostMaster.Backend.Repositories.Implementations;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace HostMaster.Backend.UnitsOfWork.Implementations;

public class UsersUnitOfWork : GenericUnitOfWork<User>, IUsersUnitOfWork
{
    private readonly IUsersRepository _usersRepository;

    public UsersUnitOfWork(IGenericRepository<User> repository, IUsersRepository usersRepository) : base(repository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<ActionResponse<User>> AddAsync(UserDTO userDTO) => await _usersRepository.AddAsync(userDTO);

    public override async Task<ActionResponse<IEnumerable<User>>> GetAsync() => await _usersRepository.GetAsync();

    public override async Task<ActionResponse<IEnumerable<User>>> GetAsync(PaginationDTO pagination) => await _usersRepository.GetAsync(pagination);
}