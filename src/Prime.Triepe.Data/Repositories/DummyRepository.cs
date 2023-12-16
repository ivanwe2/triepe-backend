using AutoMapper;
using Prime.Triepe.Data.Entities;
using Prime.Triepe.Domain.Abstractions.Repositories;

namespace Prime.Triepe.Data.Repositories
{
    public class DummyRepository : BaseRepository<BaseEntity>, IDummyRepository
    {
        public DummyRepository(TriepeDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
