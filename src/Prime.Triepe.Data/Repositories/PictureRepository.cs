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
    internal class PictureRepository : BaseRepository<Picture>, IPictureRepository
    {
        public PictureRepository(TriepeDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
