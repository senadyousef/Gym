using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class ChildrenRepository : Repository<Children>, IChildrenRepository
    {
        public ChildrenRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}   