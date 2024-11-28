using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using System.Threading.Tasks;
using static MudBlazor.Colors;
using System.Collections.Generic;

namespace HostMaster.Backend.UnitsOfWork.Interfaces;

public interface IOpinionsUnitOfWork
{
    Task<ActionResponse<Opinion>> AddAsync(OpinionDTO OpinionDTO);

    Task<ActionResponse<IEnumerable<Opinion>>> GetAsync();

    Task<ActionResponse<Opinion>> GetAsync(int opinionId);

    Task<ActionResponse<IEnumerable<Opinion>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}