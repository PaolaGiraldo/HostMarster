using HostMaster.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.Responses;

public class ActionResponse<T>
{
    public bool Success;

    public bool WasSuccess { get; set; }

    public string? Message { get; set; }

    public T? Result { get; set; }
    public List<Room> Data { get; set; }
}