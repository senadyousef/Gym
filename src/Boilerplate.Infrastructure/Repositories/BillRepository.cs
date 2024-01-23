using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class BillRepository : Repository<Bill>, IBillRepository
    {
        public BillRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    } 
}
