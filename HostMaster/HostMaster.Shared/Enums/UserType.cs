using System.ComponentModel;

namespace HostMaster.Shared.Enums;

public enum UserType
{
    [Description("Administrador")]
    Admin,

    [Description("Usuario")]
    Customer,

    [Description("Empleado")]
    Employee
}