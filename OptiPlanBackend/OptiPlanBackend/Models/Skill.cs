﻿namespace OptiPlanBackend.Models
{
    public class Skill
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Foreign Key
        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        // Optional attributes
        public int ProficiencyLevel { get; set; }  // Example: 1–5
        public int YearsExperience { get; set; }   // Optional
    }
}
