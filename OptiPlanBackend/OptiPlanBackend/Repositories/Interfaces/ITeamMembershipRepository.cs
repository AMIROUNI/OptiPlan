using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface ITeamMembershipRepository : IGenericRepository<TeamMembership>
    {  
        Task<IEnumerable<TeamMembership>> GetByProjectIdAsync(Guid projectId);
        Task<IEnumerable<TeamMembership>> GetByUserIdAsync(Guid userId);
        Task<TeamMembership?> GetByUserAndProjectIdAsync(Guid userId, Guid projectId);
        
    }
    
 }

