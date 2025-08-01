﻿namespace OptiPlanBackend.Models
{
    public class Attachment
    {
            public Guid Id { get; set; }
            public string FileName { get; set; } = string.Empty;
            public string FilePath { get; set; } = string.Empty;
            public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

            public Guid WorkItemId { get; set; }
            public WorkItem WorkItem { get; set; }

            public Guid UploaderId { get; set; }
            public User Uploader { get; set; }
    }
}
