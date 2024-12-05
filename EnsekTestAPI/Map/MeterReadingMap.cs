using CsvHelper.Configuration;
using EnsekTest.Domain.Entities;

namespace EnsekTest.API.Map;

public class MeterReadingMap : ClassMap<MeterReading>
{
    public MeterReadingMap()
    {
        Map(m => m.AccountId);
        Map(m => m.MeterReadingDateTime)
            .TypeConverterOption
            .Format("dd/MM/yyyy HH:mm");
        Map(m => m.MeterReadValue);
    }
}
