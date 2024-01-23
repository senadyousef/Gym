using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class CartsRepository : Repository<Carts>, ICartsRepository
    {
        public CartsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    } 
}
