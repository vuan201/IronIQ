using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronIQ.Infrastructure.Persistence.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.Difficulty).HasConversion<string>();

        var listComparer = new ValueComparer<List<string>>(
            (a, b) => a != null && b != null && a.SequenceEqual(b),
            v => v.Aggregate(0, (a, c) => HashCode.Combine(a, c.GetHashCode())),
            v => v.ToList());

        builder.Property(e => e.MuscleGroups)
            .HasConversion(
                v => string.Join(',', v.Select(m => m.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => Enum.Parse<MuscleGroup>(s))
                      .ToList())
            .Metadata.SetValueComparer(new ValueComparer<List<MuscleGroup>>(
                (a, b) => a != null && b != null && a.SequenceEqual(b),
                v => v.Aggregate(0, (a, c) => HashCode.Combine(a, c.GetHashCode())),
                v => v.ToList()));

        builder.Property(e => e.Equipment)
            .HasConversion(
                v => string.Join(',', v.Select(eq => eq.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => Enum.Parse<Equipment>(s))
                      .ToList())
            .Metadata.SetValueComparer(new ValueComparer<List<Equipment>>(
                (a, b) => a != null && b != null && a.SequenceEqual(b),
                v => v.Aggregate(0, (a, c) => HashCode.Combine(a, c.GetHashCode())),
                v => v.ToList()));
    }
}
