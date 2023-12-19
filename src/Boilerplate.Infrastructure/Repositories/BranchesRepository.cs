using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class BranchesRepository : Repository<Branches>, IBranchesRepository
    {
        public BranchesRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}   