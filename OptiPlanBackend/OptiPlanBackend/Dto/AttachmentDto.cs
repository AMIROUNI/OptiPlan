using Microsoft.AspNetCore.Http;
using System;

namespace OptiPlanBackend.Dto
{
    public class AttachmentDto
    {
        public IFormFile File { get; set; } = null!;
        public Guid WorkItemId { get; set; }
    }
}
