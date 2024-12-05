using EnsekTest.Application.Interface;
using EnsekTest.Domain.Common;
using EnsekTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Application.Services
{
    //SOLID: Single Responsibility Principle (focused on processing meter readings)
    //SOLID: Open/Closed Principle (is open for extension but closed for modification)
    //SOLID: Interface Segregation Principle  (interfaces are segregated)
    public class MeterReadingService: IMeterReadingService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly MeterReadingValidationService _validationService;

        //SOLID: Liskov Substitution Principle (relies on abstractions (IAccountRepository, IMeterReadingRepository) that can be replaced by other implementations.)
        //SOLID: Dependency Inversion Principle (depends on abstractions (interfaces))
        public MeterReadingService(IAccountRepository accountRepo,
                                   IMeterReadingRepository meterRepo,
                                   MeterReadingValidationService validationService)
        {
            _accountRepository = accountRepo;
            _meterReadingRepository = meterRepo;
            _validationService = validationService;
        }

        public async Task<MeterReadingProcessingResult> ProcessReadingsAsync(IEnumerable<MeterReading> readings)
        {
            int success = 0, failed = 0;

            foreach (var reading in readings)
            {
                var account = await _accountRepository.GetAccountAsync(reading.AccountId);
                if (account == null || !_validationService.IsValidReading(reading.MeterReadValue))
                {
                    failed++;
                    continue;
                }

                if (await _meterReadingRepository.ReadingExistsAsync(reading.AccountId, reading.MeterReadingDateTime))
                {
                    failed++;
                    continue;
                }

                var latestReading = await _meterReadingRepository.GetLatestReadingAsync(reading.AccountId);

                //snsure the new reading is not older than the latest read
                if (latestReading != null && reading.MeterReadingDateTime <= latestReading.MeterReadingDateTime)
                {
                    failed++;
                    continue;
                }

                await _meterReadingRepository.AddReadingAsync(reading);
                success++;
            }

            return new MeterReadingProcessingResult { Success = success, Failed = failed };
        }

        public async Task<IEnumerable<MeterReading>> GetAllMeterReadingsAsync()
        {
            return await _meterReadingRepository.GetAllMeterReadingsAsync();
        }
    }
}
