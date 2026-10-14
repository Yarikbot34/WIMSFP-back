using System.Text.Json.Serialization;

namespace Domain.DTO;

public class LoginDto
{
    public string Name { get; set; }
    public string Password { get; set; }

    [JsonConstructor]
    public LoginDto(){}
}
