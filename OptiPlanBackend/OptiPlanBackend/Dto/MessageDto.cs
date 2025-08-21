namespace OptiPlanBackend.Dto
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid DirectChatId { get; set; }
        public Guid SenderId { get; set; }
        public string DisplaySender { get; set; } = string.Empty;
        public string SenderUsername { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }

}
