using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ApplicationRequestDTOs
{
    public class DeleteApplicationBatchDto
    {
        public int[] Ids { get; set; } = [];
    }
}
