using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class ItemPhotosRepository : Repository<ItemPhotos>, IItemPhotosRepository
    {
        public ItemPhotosRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}     