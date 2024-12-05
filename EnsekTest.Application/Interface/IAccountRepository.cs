using EnsekTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Application.Interface
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountAsync(int accountId);
        Task<bool> AccountExistsAsync(int accountId);
        Task SeedAccountsAsync(IEnumerable<Account> accounts);
    }
}
