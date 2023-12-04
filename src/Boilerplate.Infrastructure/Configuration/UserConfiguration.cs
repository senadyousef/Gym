using System;
using Boilerplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BC = BCrypt.Net.BCrypt;

namespace Boilerplate.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email).IsRequired().HasMaxLength(254);
            builder.HasIndex(x => x.Email).IsUnique();



            builder.HasData(
                new User
                {
                    Id = 1,
                    Email = "superadmin@ss.com",
                    Role = "SuperAdmin",
                    Password = BC.HashPassword("superadmin")
                }
                //new User
                //{
                //    Id = 2,
                //    Email = "Owner@ss.com",
                //    Role = "Owner",
                //    Password = BC.HashPassword("Owner")
                //},
                //new User
                //{
                //    Id = 3,
                //    Email = "owneruser@ss.com",
                //    Role = "OwnerUser",
                //    Password = BC.HashPassword("owneruser")
                //},
                //new User
                //{
                //    Id = 4,
                //    Email = "contractoradmin@ss.com",
                //    Role = "Owner",
                //    Password = BC.HashPassword("contractoradmin")
                //},
                //new User
                //{
                //    Id = 5,
                //    Email = "contractoruser@ss.com",
                //    Role = "ContractorUser",
                //    Password = BC.HashPassword("contractoruser")
                //},
                //new User
                //{
                //    Id = 6,
                //    Email = "subcontractoruser@ss.com",
                //    Role = "SubContractorUser",
                //    Password = BC.HashPassword("subcontractoruser")
                // }
            );
        }
    }
}
