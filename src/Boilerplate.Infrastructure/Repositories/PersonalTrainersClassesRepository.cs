using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class PersonalTrainersClassesRepository : Repository<PersonalTrainersClasses>, IPersonalTrainersClassesRepository
    {
        public PersonalTrainersClassesRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}   