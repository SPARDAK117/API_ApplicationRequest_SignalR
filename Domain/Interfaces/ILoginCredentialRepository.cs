using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILoginCredentialRepository
    {
        Task<LoginCredential?> GetByUsernameOrEmailAsync(string input);
    }
}
