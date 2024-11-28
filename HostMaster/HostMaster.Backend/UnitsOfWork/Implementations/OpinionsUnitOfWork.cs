using HostMaster.Backend.Repositories.Implementations;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Backend.UnitsOfWork.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Implementations;

public class OpinionsUnitOfWork(IGenericRepository<Opinion> repository, IOpinionsRepository opinionsRepository) : GenericUnitOfWork<Opinion>(repository), IOpinionsUnitOfWork
{
    private readonly IOpinionsRepository _opinionsRepository = opinionsRepository;

    public override async Task<ActionResponse<IEnumerable<Opinion>>> GetAsync() => await _opinionsRepository.GetAsync();

    public override async Task<ActionResponse<Opinion>> GetAsync(int id) => await _opinionsRepository.GetAsync(id);

    public override async Task<ActionResponse<IEnumerable<Opinion>>> GetAsync(PaginationDTO pagination) => await _opinionsRepository.GetAsync(pagination);

    public async Task<ActionResponse<Opinion>> AddAsync(OpinionDTO opinionDTO) => await _opinionsRepository.AddAsync(opinionDTO);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _opinionsRepository.GetTotalRecordsAsync(pagination);
}