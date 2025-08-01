using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class InvitationService : IInvitationService
    {

        private readonly IInvitationRepository _invitationRepository;

        public InvitationService(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }
        public  async Task<bool> CreateAsync(Invitation invitation)
        {
             await _invitationRepository.AddAsync(invitation);
            return await _invitationRepository.SaveChangesAsync();

        }

        public Task<bool> DeleteAsync(Invitation invitation)
        {
                _invitationRepository.Delete(invitation);
                return _invitationRepository.SaveChangesAsync();
        }

        public Task<IEnumerable<Invitation>> GetAllAsync()
        {
             return _invitationRepository.GetAllAsync();
        }

        public Task<Invitation?> GetByIdAsync(Guid id)
        {
           return _invitationRepository.GetByIdAsync(id);
        }

        public Task<bool> UpdateAsync(Invitation invitation)
        {
            _invitationRepository.Update(invitation);
            return _invitationRepository.SaveChangesAsync();
        }


        public async Task<Team> GetTeamWithProjectAsync(Guid teamId)
        {
            return await _invitationRepository.GetTeamWithProjectAsync(teamId);
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _invitationRepository.GetUserByIdAsync(userId);
        }


        public async Task<IEnumerable<Invitation>> GetByUserIdAsync(Guid userId)
        {
            return await _invitationRepository.GetByUserIdAsync(userId);
        }
    }
}
