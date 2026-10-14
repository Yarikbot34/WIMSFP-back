using System.Text.Json.Serialization;

namespace Domain.DTO;

public class CreateUserDto
{
    public string Name { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    [JsonConstructor]
    public CreateUserDto(){}
}
