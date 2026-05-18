using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IronIQ.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
    public DbSet<WorkoutDay> WorkoutDays => Set<WorkoutDay>();
    public DbSet<PlanExercise> PlanExercises => Set<PlanExercise>();
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<ExerciseLog> ExerciseLogs => Set<ExerciseLog>();
    public DbSet<SetLog> SetLogs => Set<SetLog>();
    public DbSet<CoinTransaction> CoinTransactions => Set<CoinTransaction>();
    public DbSet<ProgressionSuggestion> ProgressionSuggestions => Set<ProgressionSuggestion>();
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<UserAchievement> UserAchievements => Set<UserAchievement>();

    private IDbContextTransaction? _currentTransaction;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        _currentTransaction ??= await Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction is null) return;
        await _currentTransaction.CommitAsync(ct);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction is null) return;
        await _currentTransaction.RollbackAsync(ct);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }
}
