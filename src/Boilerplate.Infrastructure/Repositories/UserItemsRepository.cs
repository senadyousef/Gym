﻿using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class UserItemsRepository : Repository<UserItems>, IUserItemsRepository
    {
        public UserItemsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    } 
}
