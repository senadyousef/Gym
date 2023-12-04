using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class ProfileRepository : Repository<User>, IProfileRepository
    {
        public ProfileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
