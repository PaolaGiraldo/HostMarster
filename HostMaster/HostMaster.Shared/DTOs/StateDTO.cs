using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.DTOs;

public class StateDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int CountryId { get; set; }
}