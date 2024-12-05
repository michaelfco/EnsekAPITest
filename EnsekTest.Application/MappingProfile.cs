using AutoMapper;
using EnsekTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EnsekTest.Application
{
    public class MappingProfile : Profile
    {
        //map
        public MappingProfile()
        {
            CreateMap<CsvMeterReading, MeterReading>()
          .ForMember(dest => dest.MeterReadingDateTime, opt => opt.MapFrom(src => DateTime.Parse(src.MeterReadingDateTime)))
          .ForMember(dest => dest.MeterReadValue, opt => opt.MapFrom(src => int.Parse(src.MeterReadValue)));


          CreateMap<CsvAccountReading, Account>()
        .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
        .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

        }
    }
}
