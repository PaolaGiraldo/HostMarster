using HostMaster.Backend.Repositories.Implementations;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Implementations
{
    public class CustomersUnitOfWork : GenericUnitOfWork<Customer>, ICustomersUnitOfWork
    {
        private readonly ICustomersRepository _customersRepository;

        public CustomersUnitOfWork(IGenericRepository<Customer> repository, ICustomersRepository customersRepository) : base(repository)
        {
            _customersRepository = customersRepository;
        }

        public async Task<ActionResponse<Customer>> AddAsync(CustomerDTO customerDTO) => await _customersRepository.AddAsync(customerDTO);

        public override async Task<ActionResponse<IEnumerable<Customer>>> GetAsync() => await _customersRepository.GetAsync();

        public override async Task<ActionResponse<Customer>> GetAsync(int document) => await _customersRepository.GetAsync(document);

        public override async Task<ActionResponse<IEnumerable<Customer>>> GetAsync(PaginationDTO pagination) => await _customersRepository.GetAsync(pagination);

        public async Task<IEnumerable<Customer>> GetComboAsync() => await _customersRepository.GetComboAsync();

        public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _customersRepository.GetTotalRecordsAsync(pagination);

        public async Task<ActionResponse<Customer>> UpdateAsync(CustomerDTO customerDTO) => await _customersRepository.UpdateAsync(customerDTO);
    }
}