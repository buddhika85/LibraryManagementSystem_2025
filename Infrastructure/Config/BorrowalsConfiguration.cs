using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Config
{
    public class BorrowalsConfiguration : IEntityTypeConfiguration<Borrowals>
    {
        public void Configure(EntityTypeBuilder<Borrowals> builder)
        {
            builder.HasOne(b => b.AppUser)
                .WithMany()
                .HasForeignKey(b => b.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.BorrowalReturn)
                .WithOne(br => br.Borrowals)
                .HasForeignKey<BorrowalReturn>(br => br.BorrowalsId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
