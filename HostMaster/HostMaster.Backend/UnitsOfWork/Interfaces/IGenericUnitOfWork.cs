﻿using HostMaster.Shared.DTOs;
using HostMaster.Shared.Responses;

namespace HostMaster.Backend.UnitsOfWork.Interfaces;

public interface IGenericUnitOfWork<T> where T : class
{
	Task<ActionResponse<IEnumerable<T>>> GetAsync();

	Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination);

	Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

	Task<ActionResponse<T>> AddAsync(T model);

	Task<ActionResponse<T>> UpdateAsync(T model);

	Task<ActionResponse<T>> DeleteAsync(int id);

	Task<ActionResponse<T>> GetAsync(int id);
}