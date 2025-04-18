using Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Config
{
    // used to configure custom SQL data types for properties in entity class
    // custom column names
    // nullness
    // ...etc
    public class AuthorConfigurations : IEntityTypeConfiguration<Author>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Author> builder)
        {
            builder.Property(x => x.Name).HasColumnType("nvarchar(200)");
            builder.Property(x => x.Country).HasColumnType("nvarchar(200)");
            builder.Property(x => x.Country).HasColumnType("nvarchar(max)");
        }
    }
}
