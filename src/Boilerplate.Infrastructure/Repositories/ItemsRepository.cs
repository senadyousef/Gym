using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class ItemsRepository : Repository<Items>, IItemsRepository
    {
        public ItemsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    } 
}
