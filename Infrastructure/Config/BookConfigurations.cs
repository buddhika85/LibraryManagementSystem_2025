using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config
{
    internal class BookConfigurations : IEntityTypeConfiguration<Book>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Book> builder)
        {
            builder.Property(x => x.Title).HasColumnType("nvarchar(200)");           
            builder.Property(x => x.PictureUrl).HasColumnType("nvarchar(max)");
        }
    }
}
