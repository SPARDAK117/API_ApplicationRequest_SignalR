namespace Domain.Entities
{
    public class ApplicationRequest
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "submitted";
        public int TypeId { get; set; }
        public RequestType? Type { get; set; }
    }
}
