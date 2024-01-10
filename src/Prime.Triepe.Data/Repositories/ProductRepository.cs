using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triepe.Data.Entities;
using Triepe.Data.Repositories.Base;
using Triepe.Domain.Abstractions.Repositories;

namespace Triepe.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(TriepeDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
