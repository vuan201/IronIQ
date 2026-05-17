using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronIQ.Infrastructure.Persistence.Configurations;

public class CoinTransactionConfiguration : IEntityTypeConfiguration<CoinTransaction>
{
    public void Configure(EntityTypeBuilder<CoinTransaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Reason)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.ExternalTransactionId)
            .HasMaxLength(200);

        builder.Property(t => t.Type)
            .HasConversion<string>();

        builder.HasIndex(t => t.ExternalTransactionId)
            .IsUnique()
            .HasFilter("\"ExternalTransactionId\" IS NOT NULL");

        builder.HasIndex(t => t.UserId);
    }
}
