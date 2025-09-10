using System;

namespace ST10291238_PROG7312_POE.Models
{
    public class Issue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public string Location { get; set; }
        public IssueCategory Category { get; set; }
        public string Description { get; set; }

        public string AttachmentPath { get; set; }

        public string Status { get; set; } = "Submitted";
    }
}
