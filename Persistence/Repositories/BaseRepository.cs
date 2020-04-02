using System;
using System.Linq;
using System.Linq.Expressions;
using GloEpidBot.Model.Domain;
using GloEpidBot.Model.Repositories;
using GloEpidBot.Persistence.Contexts;

namespace GloEpidBot.Persistence.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> FindAll()
        {
            return this._context.Set<T>();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this._context.Set<T>()
                .Where(expression);
        }
    }
   
}