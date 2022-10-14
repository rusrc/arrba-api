using Microsoft.IdentityModel.Tokens;

namespace Arrba.Api.Jwt
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
