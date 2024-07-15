using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class AllServicesRepository : Repository<AllServices>, IAllServicesRepository
    {
        public AllServicesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    } 
}
