using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace ST10291238_PROG7312_POE.Models
{
    public class Issue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        [Required]
        public string Location { get; set; } = string.Empty;
        public IssueCategory Category { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;

        [ValidateNever]
        public string AttachmentPaths { get; set; } = string.Empty;

        [ValidateNever]
        public string Status { get; set; } = "Submitted";
    }
}
