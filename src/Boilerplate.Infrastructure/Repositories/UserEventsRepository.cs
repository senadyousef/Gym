using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class UserEventsRepository : Repository<UserEvents>, IUserEventsRepository
    {
        public UserEventsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    } 
}
