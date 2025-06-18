using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApplicationRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int TypeId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = "submitted";
        public RequestType? Type { get; set; }
    }
}
