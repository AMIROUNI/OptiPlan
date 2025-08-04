namespace OptiPlanBackend.Services.Interfaces
{
    public interface IChatBotService
    {


        public  Task<string> AskBotAsync(string userMessage);
    }
}
