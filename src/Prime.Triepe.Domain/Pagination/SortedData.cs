using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triepe.Domain.Pagination
{
    public class SortedData
    {
        public bool Empty { get; set; } = true;
        public bool Sorted { get; set; } = false;
        public bool Unsorted { get; set; } = true;
    }

}
