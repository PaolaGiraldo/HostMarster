using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using static MudBlazor.Colors;

namespace HostMaster.Backend.Repositories.Interfaces;

public interface IOpinionsRepository
{
    Task<ActionResponse<Opinion>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Opinion>>> GetAsync();

    Task<ActionResponse<Opinion>> AddAsync(OpinionDTO opinionDTO);

    Task<ActionResponse<IEnumerable<Opinion>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}