using IronIQ.Application.Common.Interfaces;
using IronIQ.Domain.Entities;
using IronIQ.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IronIQ.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IExerciseRepository>();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        if (await repo.ExistsSystemAsync()) return;

        logger.LogInformation("Seeding exercise library...");

        var exercises = new List<Exercise>
        {
            // ── CHEST ──────────────────────────────────────────────────
            Exercise.CreateSystem("Bench Press", "Classic barbell chest press on flat bench.",
                [MuscleGroup.Chest], [Equipment.Barbell, Equipment.Bench], Difficulty.Intermediate),
            Exercise.CreateSystem("Incline Dumbbell Press", "Press at 30-45° incline targeting upper chest.",
                [MuscleGroup.Chest], [Equipment.Dumbbell, Equipment.Bench], Difficulty.Intermediate),
            Exercise.CreateSystem("Push-Up", "Bodyweight chest press on the floor.",
                [MuscleGroup.Chest, MuscleGroup.Triceps], [Equipment.Bodyweight], Difficulty.Beginner),
            Exercise.CreateSystem("Cable Fly", "Cable crossover fly for chest isolation.",
                [MuscleGroup.Chest], [Equipment.Cable], Difficulty.Intermediate),
            Exercise.CreateSystem("Dumbbell Fly", "Chest fly on flat bench.",
                [MuscleGroup.Chest], [Equipment.Dumbbell, Equipment.Bench], Difficulty.Beginner),

            // ── BACK ───────────────────────────────────────────────────
            Exercise.CreateSystem("Pull-Up", "Overhand grip vertical pulling movement.",
                [MuscleGroup.Back, MuscleGroup.Biceps], [Equipment.PullUpBar], Difficulty.Intermediate),
            Exercise.CreateSystem("Barbell Row", "Bent-over barbell row for mid/upper back.",
                [MuscleGroup.Back], [Equipment.Barbell], Difficulty.Intermediate),
            Exercise.CreateSystem("Lat Pulldown", "Cable lat pulldown for back width.",
                [MuscleGroup.Back], [Equipment.Cable], Difficulty.Beginner),
            Exercise.CreateSystem("Seated Cable Row", "Horizontal cable row for back thickness.",
                [MuscleGroup.Back], [Equipment.Cable], Difficulty.Beginner),
            Exercise.CreateSystem("Deadlift", "Compound posterior chain movement.",
                [MuscleGroup.Back, MuscleGroup.Legs, MuscleGroup.Glutes], [Equipment.Barbell], Difficulty.Advanced),

            // ── SHOULDERS ──────────────────────────────────────────────
            Exercise.CreateSystem("Overhead Press", "Barbell shoulder press standing or seated.",
                [MuscleGroup.Shoulders], [Equipment.Barbell], Difficulty.Intermediate),
            Exercise.CreateSystem("Lateral Raise", "Dumbbell lateral raise for shoulder width.",
                [MuscleGroup.Shoulders], [Equipment.Dumbbell], Difficulty.Beginner),
            Exercise.CreateSystem("Front Raise", "Dumbbell front raise for anterior deltoid.",
                [MuscleGroup.Shoulders], [Equipment.Dumbbell], Difficulty.Beginner),
            Exercise.CreateSystem("Face Pull", "Cable face pull for rear deltoids and rotator cuff.",
                [MuscleGroup.Shoulders, MuscleGroup.Back], [Equipment.Cable], Difficulty.Beginner),

            // ── BICEPS ─────────────────────────────────────────────────
            Exercise.CreateSystem("Barbell Curl", "Classic barbell bicep curl.",
                [MuscleGroup.Biceps], [Equipment.Barbell], Difficulty.Beginner),
            Exercise.CreateSystem("Dumbbell Hammer Curl", "Neutral grip curl for brachialis.",
                [MuscleGroup.Biceps], [Equipment.Dumbbell], Difficulty.Beginner),
            Exercise.CreateSystem("Incline Dumbbell Curl", "Full stretch bicep curl on incline bench.",
                [MuscleGroup.Biceps], [Equipment.Dumbbell, Equipment.Bench], Difficulty.Intermediate),

            // ── TRICEPS ────────────────────────────────────────────────
            Exercise.CreateSystem("Tricep Dip", "Bodyweight or weighted dip for triceps.",
                [MuscleGroup.Triceps, MuscleGroup.Chest], [Equipment.Bodyweight], Difficulty.Intermediate),
            Exercise.CreateSystem("Skull Crusher", "EZ bar or dumbbell lying tricep extension.",
                [MuscleGroup.Triceps], [Equipment.Barbell, Equipment.Bench], Difficulty.Intermediate),
            Exercise.CreateSystem("Cable Pushdown", "Rope or bar cable tricep pushdown.",
                [MuscleGroup.Triceps], [Equipment.Cable], Difficulty.Beginner),

            // ── LEGS ───────────────────────────────────────────────────
            Exercise.CreateSystem("Squat", "Barbell back squat — king of leg exercises.",
                [MuscleGroup.Legs, MuscleGroup.Glutes], [Equipment.Barbell], Difficulty.Intermediate),
            Exercise.CreateSystem("Romanian Deadlift", "Hip-hinge for hamstrings and glutes.",
                [MuscleGroup.Legs, MuscleGroup.Glutes], [Equipment.Barbell], Difficulty.Intermediate),
            Exercise.CreateSystem("Leg Press", "Machine compound leg press.",
                [MuscleGroup.Legs, MuscleGroup.Glutes], [Equipment.Machine], Difficulty.Beginner),
            Exercise.CreateSystem("Leg Extension", "Machine quad isolation.",
                [MuscleGroup.Legs], [Equipment.Machine], Difficulty.Beginner),
            Exercise.CreateSystem("Leg Curl", "Machine hamstring isolation.",
                [MuscleGroup.Legs], [Equipment.Machine], Difficulty.Beginner),
            Exercise.CreateSystem("Walking Lunge", "Dumbbell or bodyweight walking lunges.",
                [MuscleGroup.Legs, MuscleGroup.Glutes], [Equipment.Dumbbell], Difficulty.Beginner),
            Exercise.CreateSystem("Calf Raise", "Standing or seated calf raise.",
                [MuscleGroup.Calves], [Equipment.Machine], Difficulty.Beginner),

            // ── ABS ────────────────────────────────────────────────────
            Exercise.CreateSystem("Plank", "Isometric core stability exercise.",
                [MuscleGroup.Abs], [Equipment.Bodyweight], Difficulty.Beginner),
            Exercise.CreateSystem("Crunch", "Classic abdominal crunch.",
                [MuscleGroup.Abs], [Equipment.Bodyweight], Difficulty.Beginner),
            Exercise.CreateSystem("Hanging Leg Raise", "Hanging from bar, raise legs for lower abs.",
                [MuscleGroup.Abs], [Equipment.PullUpBar], Difficulty.Intermediate),

            // ── CARDIO / FULL BODY ─────────────────────────────────────
            Exercise.CreateSystem("Burpee", "Full body conditioning exercise.",
                [MuscleGroup.FullBody, MuscleGroup.Cardio], [Equipment.Bodyweight], Difficulty.Intermediate),
            Exercise.CreateSystem("Kettlebell Swing", "Hip-hinge power movement.",
                [MuscleGroup.Glutes, MuscleGroup.Back, MuscleGroup.Cardio], [Equipment.Kettlebell], Difficulty.Intermediate),
        };

        await repo.AddRangeAsync(exercises);
        await uow.SaveChangesAsync();
        logger.LogInformation("Seeded {Count} exercises.", exercises.Count);
    }

    public static async Task SeedAchievementsAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IAchievementRepository>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        if (await repo.AnyAsync()) return;

        logger.LogInformation("Seeding achievements...");

        var achievements = new List<Achievement>
        {
            // Sessions
            Achievement.Create("first_step",    "Bước đầu tiên",       "Hoàn thành buổi tập đầu tiên.",          "🏁", AchievementType.Sessions, 1,   1),
            Achievement.Create("warming_up",    "Đang khởi động",      "Hoàn thành 5 buổi tập.",                 "💪", AchievementType.Sessions, 5,   2),
            Achievement.Create("on_track",      "Đúng hướng",          "Hoàn thành 10 buổi tập.",                "🎯", AchievementType.Sessions, 10,  3),
            Achievement.Create("dedicated",     "Kiên định",           "Hoàn thành 25 buổi tập.",                "⚡", AchievementType.Sessions, 25,  4),
            Achievement.Create("iron_warrior",  "Iron Warrior",        "Hoàn thành 50 buổi tập.",                "🛡️", AchievementType.Sessions, 50,  5),
            Achievement.Create("legend",        "Huyền thoại",         "Hoàn thành 100 buổi tập.",               "🏆", AchievementType.Sessions, 100, 6),
            // Streak
            Achievement.Create("hot_start",     "Khởi đầu bốc lửa",   "Duy trì streak 3 ngày liên tiếp.",       "🔥", AchievementType.Streak,   3,   7),
            Achievement.Create("week_strong",   "Tuần mạnh mẽ",        "Duy trì streak 7 ngày liên tiếp.",       "🌟", AchievementType.Streak,   7,   8),
            Achievement.Create("two_weeks",     "Hai tuần bất bại",    "Duy trì streak 14 ngày liên tiếp.",      "💫", AchievementType.Streak,   14,  9),
            Achievement.Create("monthly",       "Tháng của sắt thép",  "Duy trì streak 30 ngày liên tiếp.",      "🎖️", AchievementType.Streak,   30,  10),
            // Sets
            Achievement.Create("first_sets",    "Cái đầu tiên",        "Hoàn thành tổng cộng 50 set.",           "🎯", AchievementType.Sets,     50,  11),
            Achievement.Create("rep_grinder",   "Máy nghiền rep",      "Hoàn thành tổng cộng 200 set.",          "⚙️", AchievementType.Sets,     200, 12),
            Achievement.Create("iron_plates",   "Tạ sắt",              "Hoàn thành tổng cộng 500 set.",          "🏋️", AchievementType.Sets,     500, 13),
            Achievement.Create("volume_king",   "Vua Volume",          "Hoàn thành tổng cộng 1000 set.",         "👑", AchievementType.Sets,     1000, 14),
            Achievement.Create("unstoppable",   "Không thể ngăn cản",  "Hoàn thành tổng cộng 2000 set.",         "🔱", AchievementType.Sets,     2000, 15),
        };

        await db.Achievements.AddRangeAsync(achievements);
        await db.SaveChangesAsync();
        logger.LogInformation("Seeded {Count} achievements.", achievements.Count);
    }
}
