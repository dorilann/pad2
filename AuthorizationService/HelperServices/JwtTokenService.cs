using System.Text;
using AuthorizationService.Model;
using MongoDB.Driver;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;


public class JwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IMongoCollection<User> _userCollection;

    public JwtTokenService(IConfiguration configuration, IMongoDatabase database)
    {
        _userCollection = database.GetCollection<User>("Users");
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        try
        {

        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? _configuration["Jwt:Key"];
        var issuer = Environment.GetEnvironmentVariable("ISSUER") ?? _configuration["Jwt:Issuer"];
        var audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? _configuration["Jwt:Audience"];

        var data = Encoding.UTF8.GetBytes(jwtKey);
        var securityKey = new SymmetricSecurityKey(data);

        var claims = new Dictionary<string, object>
        {
            [ClaimTypes.Name] = user.Username,
            [ClaimTypes.Role] = user.Role,
            [ClaimTypes.Sid] = user.Id
        };
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Claims = claims,
            IssuedAt = null,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(120),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JsonWebTokenHandler();
        handler.SetDefaultTimesOnTokenCreation = false;
        var tokenString = handler.CreateToken(descriptor);


            return tokenString;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            return "";
        }
    }
}
