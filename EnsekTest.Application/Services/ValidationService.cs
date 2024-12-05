using EnsekTest.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Application.Services;

public class ValidationService : IValidationService
{
    //validate number
    public bool IsValidReading(string meterReadValue)
    {
        if (string.IsNullOrEmpty(meterReadValue))
            return false;

        ///check if it's a valid integer
        return int.TryParse(meterReadValue, out _);  
    }
}
