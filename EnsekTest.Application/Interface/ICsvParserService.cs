using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Application.Interface
{
    public interface ICsvParserService
    {
        public IEnumerable<T> Parse<T>(Stream csvStream);
    }
}
