using OptiPlanBackend.Dto;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<List<KpiDto>> GetUserKpis(Guid userId); 
    }
}
