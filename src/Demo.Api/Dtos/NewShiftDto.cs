using Demo.Business.Models;

namespace Demo.Api.Dtos;

public class NewShiftDto
{
    public string Name { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.None;

    public DateTime StartTimeUtc { get; set; } = default;

    public DateTime EndTimeUtc { get; set; } = default;

}
