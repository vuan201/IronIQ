using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronIQ.Infrastructure.Persistence.Configurations;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Status).HasConversion<string>();
        builder.Property(s => s.Notes).HasMaxLength(1000);

        builder.HasMany(s => s.ExerciseLogs)
            .WithOne()
            .HasForeignKey(l => l.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ExerciseLogConfiguration : IEntityTypeConfiguration<ExerciseLog>
{
    public void Configure(EntityTypeBuilder<ExerciseLog> builder)
    {
        builder.HasKey(l => l.Id);

        builder.HasMany(l => l.Sets)
            .WithOne()
            .HasForeignKey(s => s.ExerciseLogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Exercise)
            .WithMany()
            .HasForeignKey(l => l.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class SetLogConfiguration : IEntityTypeConfiguration<SetLog>
{
    public void Configure(EntityTypeBuilder<SetLog> builder)
    {
        builder.HasKey(s => s.Id);
    }
}
