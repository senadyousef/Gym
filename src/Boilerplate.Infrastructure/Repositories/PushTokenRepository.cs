using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.Repositories
{
    public class PushTokenRepository : Repository<PushToken>, IPushTokenRepository
    {
        public PushTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
