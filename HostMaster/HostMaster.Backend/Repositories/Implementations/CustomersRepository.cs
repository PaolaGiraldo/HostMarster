using HostMaster.Backend.Data;
using HostMaster.Backend.Helpers;
using HostMaster.Backend.Repositories.Interfaces;
using HostMaster.Shared.DTOs;
using HostMaster.Shared.Entities;
using HostMaster.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Repositories.Implementations;

public class CustomersRepository : GenericRepository<Customer>, ICustomersRepository
{
    private readonly DataContext _context;

    public CustomersRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    async Task<ActionResponse<Customer>> ICustomersRepository.AddAsync(CustomerDTO customerDTO)
    {
        var customer = new Customer
        {
            DocumentNumber = customerDTO.DocumentNumber,
            DocumentType = customerDTO.DocumentType,
            FirstName = customerDTO.FirstName,
            LastName = customerDTO.LastName,
            Email = customerDTO.Email,
            PhoneNumber = customerDTO.PhoneNumber
        };

        _context.Add(customer);

        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Customer>
            {
                WasSuccess = true,
                Result = customer
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Customer>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Customer>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    /*
    async Task<ActionResponse<IEnumerable<Customer>>> ICustomersRepository.GetByRoomIdAsync(int Id)
    {
        {
            var customer = await _context.RoomTypes
            .Where(r => r.Id == Id)
            .ToListAsync();

            return new ActionResponse<IEnumerable<Customer>>
            {
                WasSuccess = true,
                Result = customer
            };
        }
    }
    */

    async Task<ActionResponse<Customer>> ICustomersRepository.GetAsync(int document)
    {
        var customer = await _context.Customers
             .FirstOrDefaultAsync(r => r.DocumentNumber == document);

        if (customer == null)
        {
            return new ActionResponse<Customer>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Customer>
        {
            WasSuccess = true,
            Result = customer
        };
    }

    async Task<ActionResponse<IEnumerable<Customer>>> ICustomersRepository.GetAsync()
    {
        var customer = await _context.Customers
           .ToListAsync();

        return new ActionResponse<IEnumerable<Customer>>
        {
            WasSuccess = true,
            Result = customer
        };
    }

    async Task<ActionResponse<IEnumerable<Customer>>> ICustomersRepository.GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Customers
           .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.DocumentNumber.ToString().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Customer>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.DocumentNumber)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    async Task<IEnumerable<Customer>> ICustomersRepository.GetComboAsync()
    {
        return await _context.Customers
            .ToListAsync();
    }

    async Task<ActionResponse<int>> ICustomersRepository.GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.DocumentNumber.ToString().Contains(pagination.Filter));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }

    async Task<ActionResponse<Customer>> ICustomersRepository.UpdateAsync(CustomerDTO customerDTO)
    {
        var cityalreadyexists = await _context.Customers.FindAsync(customerDTO.DocumentNumber);
        if (cityalreadyexists == null)
        {
            return new ActionResponse<Customer>
            {
                WasSuccess = false,
                Message = "ERR004"
            };
        }

        var customer = new Customer
        {
            Id = customerDTO.Id,
            FirstName = customerDTO.FirstName,
            LastName = customerDTO.LastName,
            DocumentType = customerDTO.DocumentType
        };

        _context.Update(customer);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Customer>
            {
                WasSuccess = true,
                Result = customer
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Customer>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Customer>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }
}