using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class UserAllServicesRepository : Repository<UserAllServices>, IUserAllServicesRepository
    {
        public UserAllServicesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    } 
}
