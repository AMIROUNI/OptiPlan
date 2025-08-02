using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Models;

namespace OptiPlanBackend.Dto
{
    public class AttachmentDto
    {
        [FromForm]
        public Guid WorkItemId { get; set; }

        [FromForm]
        public IFormFile File { get; set; }
    }

}
