using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class TopOfTopRepository : Repository<TopOfTop>, ITopOfTopRepository
    {
        public TopOfTopRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}

