using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ApplicationRequestDTOs
{
    public class CreateApplicationRequestDto
    {
        public int TypeId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
