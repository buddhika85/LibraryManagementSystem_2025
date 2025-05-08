using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class BorrowalReturnConfigurations : IEntityTypeConfiguration<BorrowalReturn>
    {
        public void Configure(EntityTypeBuilder<BorrowalReturn> builder)
        {
            builder.Property(x => x.AmountAccepted).HasColumnType("decimal(18,2)");
            builder.Property(x => x.PerDayLateFeeDollars).HasColumnType("decimal(18,2)");

            builder.HasOne(br => br.AppUser)
                .WithMany()
                .HasForeignKey(br => br.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
