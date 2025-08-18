using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {

        public Task<IEnumerable<User>> GetTeamByProjectId(Guid projectId);
        public  Task<User> findUserByUsername(string username);

        public  Task<IEnumerable<User>> getAllUserNotADMIN();


        public Task<User> GetUserByUsernameAsync(string name);
    }
}
