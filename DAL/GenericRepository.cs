using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DotNetUniversity.Data;
using Microsoft.EntityFrameworkCore;

namespace DotNetUniversity.DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal SchoolContext _schoolContext;
        internal DbSet<TEntity> _dbSet;

        public GenericRepository(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
            _dbSet = schoolContext.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includedProperties = ""
        )
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includedProperty in includedProperties.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includedProperty);
            }

            return orderBy?.Invoke(query).ToList() ?? query.ToList();
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _schoolContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            Delete(GetById(id));
        }

        public virtual void Delete(TEntity entity)
        {
            if (_schoolContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }
    }
}
