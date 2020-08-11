using SerratusTest.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerratusApi.Model
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int NumberOfPages { get; set; }
    }
}
