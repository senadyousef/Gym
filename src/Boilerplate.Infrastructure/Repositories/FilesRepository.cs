using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class FilesRepository : Repository<Files>, IFilesRepository
    {
        public FilesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    } 
}
