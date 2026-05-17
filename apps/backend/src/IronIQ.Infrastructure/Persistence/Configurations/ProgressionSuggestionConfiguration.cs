using IronIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronIQ.Infrastructure.Persistence.Configurations;

public class ProgressionSuggestionConfiguration : IEntityTypeConfiguration<ProgressionSuggestion>
{
    public void Configure(EntityTypeBuilder<ProgressionSuggestion> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.ExerciseName).IsRequired().HasMaxLength(100);
        builder.HasIndex(s => s.SessionId);
        builder.HasIndex(s => s.UserId);
    }
}
