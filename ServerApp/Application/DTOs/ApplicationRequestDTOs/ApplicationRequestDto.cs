namespace Application.DTOs.ApplicationRequestDTOs
{
    public class ApplicationRequestDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
