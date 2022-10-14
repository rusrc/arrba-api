using Microsoft.IdentityModel.Tokens;

namespace Arrba.Api.Jwt
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}
