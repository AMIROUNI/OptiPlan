using Microsoft.IdentityModel.Tokens;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UserService : IUserService
{
    private readonly UserDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(UserDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        try
        {
            if (_context.Users == null)
            {
                throw new InvalidOperationException("Users DbSet is not initialized");
            }

            return await _context.Users.FindAsync(userId).AsTask();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user by ID {userId}: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> GetUserByTokenAsync(string token)
    {
        try
        {
            Console.WriteLine($"Raw Token: {token}");

            if (token.StartsWith("Bearer "))
                token = token[7..];

            // Load the secret key from AppSettings
            var secret = _configuration["AppSettings:Token"];
            if (string.IsNullOrWhiteSpace(secret))
            {
                Console.WriteLine("JWT secret not configured properly!");
                return null;
            }

            var key = Encoding.UTF8.GetBytes(secret);

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["AppSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["AppSettings:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            // Log all claims
            Console.WriteLine("Claims:");
            foreach (var claim in principal.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            var userIdClaim = principal.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (userIdClaim == null)
            {
                Console.WriteLine("NameIdentifier claim not found!");
                return null;
            }

            Console.WriteLine($"Extracted UserId: {userIdClaim.Value}");

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                Console.WriteLine($"Failed to parse UserId: {userIdClaim.Value}");
                return null;
            }

            return await _context.Users.FindAsync(userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return null;
        }
    }
}
