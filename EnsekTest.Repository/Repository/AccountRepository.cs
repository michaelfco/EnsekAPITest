using EnsekTest.Application.Interface;
using EnsekTest.Domain.Entities;
using EnsekTest.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Persistence.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly EnsekDbContext _context;
        public AccountRepository(EnsekDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetAccountAsync(int accountId) =>
        await _context.Accounts.FindAsync(accountId);

        public async Task<bool> AccountExistsAsync(int accountId) =>
            await _context.Accounts.AnyAsync(a => a.AccountId == accountId);

        //used to seed account first time only
        public async Task SeedAccountsAsync(IEnumerable<Account> accounts)
        {
            if (!_context.Accounts.Any())
            {
                _context.Accounts.AddRange(accounts);
                await _context.SaveChangesAsync();
            }
        }
    }
}
