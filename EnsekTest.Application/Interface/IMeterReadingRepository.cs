using EnsekTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Application.Interface
{
    public interface IMeterReadingRepository
    {
        Task<bool> ReadingExistsAsync(int accountId, DateTime date);
        Task AddReadingAsync(MeterReading reading);
        Task<MeterReading?> GetLatestReadingAsync(int accountId);

        //ensek controller
        Task<IEnumerable<MeterReading>> GetAllMeterReadingsAsync();
    }
}
