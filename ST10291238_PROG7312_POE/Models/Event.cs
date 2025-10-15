namespace ST10291238_PROG7312_POE.Models
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now.AddDays(1);
        public string Location { get; set; } = string.Empty;
    }
}
