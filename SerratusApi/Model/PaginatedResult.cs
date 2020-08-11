using SerratusTest.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerratusApi.Model
{
    public class PaginatedResult
    {
        public IEnumerable<AccessionSection> AccessionSections { get; set; }
        public IEnumerable<FamilySection> FamilySections { get; set; }
        public int NumberOfPages { get; set; }
    }
}
