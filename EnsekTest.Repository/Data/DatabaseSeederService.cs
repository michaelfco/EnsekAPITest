using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using EnsekTest.Application.Interface;
using EnsekTest.Domain.Entities;
using EnsekTest.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Persistence.Data
{
    public class DatabaseSeederService : IDatabaseSeederService
    {
        private readonly EnsekDbContext _context;
        private readonly IMapper _mapper;
        public DatabaseSeederService(EnsekDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task SeedAccountsAsync(string filePath)
        {
            //is db seeded
            if (_context.Accounts.Any())
                return;

            //get csv data
            var accounts = ReadAccountsFromCsv(filePath);

            _context.Accounts.AddRange(accounts);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<T> Parse<T>(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<CsvAccountReading>();

            var mappedRecords = _mapper.Map<IEnumerable<T>>(records);
            return mappedRecords;
        }

        public IEnumerable<Account> ReadAccountsFromCsv(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            //map csv to account
            var records = csv.GetRecords<Account>().ToList();
            return records;
        }
    }
}
