using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class EventsRepository : Repository<Events>, IEventsRepository
    {
        public EventsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    } 
}
