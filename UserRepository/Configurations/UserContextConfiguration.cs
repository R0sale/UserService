using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepository.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData
            (
                new IdentityRole 
                { 
                    Id = "409e52ba-c288-463b-a6f1-5b1c0f17e17a",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = "bb32d8a3-7c76-4df5-9d1c-aba247879311",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            );
        }
    }
}
