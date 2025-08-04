namespace OptiPlanBackend.Dto
{
    public class ChatRequestDto
    {
        public Guid? ConversationId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

}
