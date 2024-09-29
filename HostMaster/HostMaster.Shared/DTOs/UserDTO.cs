using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostMaster.Shared.DTOs;

public class UserDTO
{
	public Guid Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public string Document { get; set; }
	public string DocumentType { get; set; }

	public string UserType { get; set; }
	public string Password { get; set; }
	public string Role { get; set; }
}