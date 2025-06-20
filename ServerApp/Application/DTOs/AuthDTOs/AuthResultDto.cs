using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AuthDTOs
{
    public class AuthResultDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;    
        public string Role { get; set; } = string.Empty;
    }
}
