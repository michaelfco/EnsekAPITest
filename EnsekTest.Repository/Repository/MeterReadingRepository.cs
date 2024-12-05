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
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly EnsekDbContext _context;
        public MeterReadingRepository(EnsekDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ReadingExistsAsync(int accountId, DateTime date) =>
            await _context.MeterReadings.AnyAsync(mr => mr.AccountId == accountId && mr.MeterReadingDateTime == date);

        public async Task AddReadingAsync(MeterReading reading)
        {
            _context.MeterReadings.Add(reading);
            await _context.SaveChangesAsync();
        }

        public async Task<MeterReading?> GetLatestReadingAsync(int accountId)
        {
            return await _context.MeterReadings
                .Where(r => r.AccountId == accountId)
                .OrderByDescending(r => r.MeterReadingDateTime)
                .FirstOrDefaultAsync();
        }

        //ensek controller
        public async Task<IEnumerable<MeterReading>> GetAllMeterReadingsAsync()
        {
            return await _context.MeterReadings.ToListAsync();
        }
    }
}
