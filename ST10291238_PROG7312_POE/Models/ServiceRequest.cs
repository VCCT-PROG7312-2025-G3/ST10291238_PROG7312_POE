using System;
using System.Collections.Generic;

namespace ST10291238_PROG7312_POE.Models
{
    public class ServiceRequest
    {
        public Guid Reference {  get; set; } = Guid.NewGuid();
        public string ResidentName { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Submitted";
        public int Pritority { get; set; } = 3;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<string> History { get; set; } = new List<string>();
    }
}
