using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace OptiPlanBackend.Models { 

public class DirectChat
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? User1Id { get; set; }
        public User? User1 { get; set; }

        public Guid? User2Id { get; set; }
        public User? User2 { get; set; }

        public Guid? ProjectId { get; set; }
        public Project? Project { get; set; }

        public ICollection<DirectMessage> Messages { get; set; } = new List<DirectMessage>();
    }
}
