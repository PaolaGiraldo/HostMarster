using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.Repositories.Interfaces
{
    public interface ICustomersRepository
    {
        Task<ActionResponse<Customer>> AddAsync(CustomerDTO customerDTO);

        Task<ActionResponse<Customer>> GetAsync(int document);

        Task<ActionResponse<IEnumerable<Customer>>> GetAsync();

        Task<IEnumerable<Customer>> GetComboAsync();

        Task<ActionResponse<IEnumerable<Customer>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

        Task<ActionResponse<Customer>> UpdateAsync(CustomerDTO customerDTO);
    }
}