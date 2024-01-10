using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Triepe.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Triepe.Data.Entities.Base;
using Triepe.Data.Extensions;
using Triepe.Domain.Abstractions.Repositories;
using Triepe.Domain.Exceptions;
using Triepe.Domain.Pagination;

namespace Triepe.Data.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository
        where TEntity : BaseEntity
    {
        protected IMapper Mapper { get; }
        protected TriepeDbContext DbContext { get; }
        protected DbSet<TEntity> Items { get; }

        public BaseRepository(TriepeDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Items = DbContext.Set<TEntity>();
            Mapper = mapper;
        }

        public virtual async Task<TOutput> GetByIdAsync<TOutput>(Guid id)
        {
            var output = await Mapper.ProjectTo<TOutput>(Items.AsNoTracking()
                .Where(c => c.Id == id))
                .FirstOrDefaultAsync();

            if (output is null)
            {
                throw new NotFoundException($"{typeof(TEntity).Name} was not found!");
            }

            return output;
        }

        public virtual async Task<PaginatedResult<TOutput>> GetPageAsync<TOutput>(int pageNumber, int pageSize, 
            Expression<Func<TOutput, bool>> filter = null)
        {
            var query = Items.AsQueryable();

            if (filter != null)
                query = query.Where(Mapper.Map<Expression<Func<TEntity, bool>>>(filter));

            return await Mapper.ProjectTo<TOutput>(query).PaginateAsync(pageNumber, pageSize);
        }

        public virtual async Task<TOutput> CreateAsync<TInput, TOutput>(TInput dto)
        {
            var entity = Mapper.Map<TEntity>(dto);

            await Items.AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return await Mapper.ProjectTo<TOutput>(Items.AsNoTracking()
                .Where(c => c.Id == entity.Id)).FirstOrDefaultAsync();
        }

        public virtual async Task UpdateAsync<TInput>(Guid id, TInput dto)
        {
            var entity = await Items
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"{typeof(TEntity).Name} was not found!");
            }

            Mapper.Map(dto, entity);

            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await Items
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"{typeof(TEntity).Name} was not found!");
            }

            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task<bool> HasAnyAsync(Guid id)
        {
            if (await Items.AnyAsync(x => x.Id == id)) return true;

            return false;
        }
    }

}
