using Domain.Class;

namespace Services.Interfaces;

public interface ITokenCreator
{
    string CreateToken(User user);
}
