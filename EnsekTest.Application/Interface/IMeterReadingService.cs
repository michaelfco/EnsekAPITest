using EnsekTest.Domain.Common;
using EnsekTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Application.Interface
{
    public interface IMeterReadingService
    {
        Task<MeterReadingProcessingResult> ProcessReadingsAsync(IEnumerable<MeterReading> readings);
        Task<IEnumerable<MeterReading>> GetAllMeterReadingsAsync();
    }
}
