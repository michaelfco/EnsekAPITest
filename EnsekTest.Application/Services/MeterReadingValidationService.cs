using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnsekTest.Application.Services
{
    public class MeterReadingValidationService
    {
        //make it virtual in order to mock in test
        public virtual bool IsValidReading(string readingValue) =>
            Regex.IsMatch(readingValue, @"^\d{5}$");
    }
}
