using System.Linq;
using GloEpidBot.Model.Domain;
using GloEpidBot.Persistence.Contexts;

namespace GloEpidBot.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Report> FindAll()
        {
            return this._context.Set<Report>();
        } 
    }
}