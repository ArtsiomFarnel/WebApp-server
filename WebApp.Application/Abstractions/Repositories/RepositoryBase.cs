using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WebApp.Application.Interfaces;
using WebApp.Infrastructure;

namespace WebApp.Application.Abstractions.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected DatabaseContext _context;
        public RepositoryBase(DatabaseContext repositoryContext)
        {
            _context = repositoryContext;
        }

        public IQueryable<TEntity> GetAll(bool trackChanges) =>
            !trackChanges?
                _context.Set<TEntity>()
                    .AsNoTracking() :
                _context.Set<TEntity>();

        public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges) =>
            !trackChanges ?
                _context.Set<TEntity>()
                    .Where(expression)
                    .AsNoTracking() :
                _context.Set<TEntity>()
                    .Where(expression);

        public void Create(TEntity entity) => 
            _context.Set<TEntity>().Add(entity);

        public void Update(TEntity entity) => 
            _context.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) => 
            _context.Set<TEntity>().Remove(entity);
    }
}
