using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triepe.Domain.Pagination
{
    public record PaginatedResult<T>
    {
        public List<T> Content { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public bool Last { get; set; }
        public int Size { get; set; }
        public int Number { get; set; }
        public SortedData Sort { get; set; }
        public int NumberOfElements { get; set; }
        public bool First { get; set; }
        public bool Empty { get; set; }
        public static PaginatedResult<T> EmptyResult(int totalElements, int pageNumber, bool isSortingEmpty = true) 
            => new(new PaginatedResult<T>(new List<T>(), totalElements, pageNumber, 0, isSortingEmpty));

        public PaginatedResult(List<T> content, int totalElements, int pageNumber, int pageSize, bool isSortingEmpty = true)
        {
            Content = content;
            TotalElements = totalElements;
            Size = pageSize;
            Number = pageNumber;
            NumberOfElements = content.Count;
            TotalPages = NumberOfElements > 0 ? (int)Math.Ceiling(totalElements / (double)pageSize) : 0;
            First = pageNumber == 0;
            Last = pageNumber == TotalPages;
            Empty = !content.Any();
            Sort = new SortedData()
            {
                Empty = isSortingEmpty,
                Sorted = !isSortingEmpty,
                Unsorted = isSortingEmpty
            };
        }
    }

}
