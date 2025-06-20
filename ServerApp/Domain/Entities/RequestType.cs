using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RequestType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<ApplicationRequest> ApplicationRequests { get; set; } = new List<ApplicationRequest>();
    }
}
