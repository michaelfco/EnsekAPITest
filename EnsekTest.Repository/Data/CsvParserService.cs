using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using EnsekTest.Application.Interface;
using EnsekTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Persistence.Data
{
    public class CsvParserService : ICsvParserService
    {
        private readonly IMapper _mapper;

        public CsvParserService(IMapper mapper)
        {
            _mapper = mapper;
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
            var records = csv.GetRecords<CsvMeterReading>();

            var mappedRecords = _mapper.Map<IEnumerable<T>>(records);
            return mappedRecords;
        }
       
    }
}
