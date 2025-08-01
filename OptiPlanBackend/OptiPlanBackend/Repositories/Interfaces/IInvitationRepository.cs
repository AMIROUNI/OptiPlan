using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface IInvitationRepository: IGenericRepository<Invitation>
    {

        Task<Team> GetTeamWithProjectAsync(Guid teamId);
        Task<User> GetUserByIdAsync(Guid userId);

        Task<IEnumerable<Invitation>> GetByUserIdAsync(Guid userId);


    }
}
