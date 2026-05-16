using System.Runtime.Serialization;

namespace Demo.Domain.Models;

public enum Role
{
    [EnumMember(Value = "None")]
    None = 0,
    [EnumMember(Value = "Driver")]
    Driver = 1,
    [EnumMember(Value = "Dispatcher")]
    Dispatcher = 2,
    [EnumMember(Value = "Supervisor")]
    Supervisor = 3
}
