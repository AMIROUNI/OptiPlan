using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {

        private readonly UserDbContext _context;
        private readonly IUploadService _uploadService;
        public UserProfileRepository(UserDbContext context, IUploadService uploadService) : base(context)
        {
            _context = context;
            _uploadService = uploadService;
        }

        public async Task<UserProfile?> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
            }

            return await _context.UserProfiles
                .Include(up => up.Skills)
                .AsNoTracking()
                .FirstOrDefaultAsync(up => up.UserId == userId)
                .ConfigureAwait(false);
        }







        public async Task<User> InitializeProfileAsync(Guid userId, InitializeProfileDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .ThenInclude(p => p.Skills)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

          
            if (dto.Avatar != null)
                user.AvatarUrl = await _uploadService.UploadImageAsync(dto.Avatar, "avatars");

            if (dto.Background != null)
                user.BackGround = await _uploadService.UploadImageAsync(dto.Background, "backgrounds");

            // Update/Create UserProfile
            if (user.Profile == null)
            {
                user.Profile = new UserProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id
                };
            }

            user.Profile.Bio = dto.Bio;
            user.firstLogin = false;
          
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
