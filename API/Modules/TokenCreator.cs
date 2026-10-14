using Domain.Class;
using Services.Interfaces;

namespace API.BuilderFuctions;

internal class TokenCreator : ITokenCreator
{
    public string CreateToken(User user)
    {
        return JWTCreator.CreateToken(user);
    }
}
