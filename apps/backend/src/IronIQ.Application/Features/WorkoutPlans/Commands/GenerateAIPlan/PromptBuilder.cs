namespace IronIQ.Application.Features.WorkoutPlans.Commands.GenerateAIPlan;

public static class PromptBuilder
{
    public static string BuildGeneratePlanPrompt(GenerateAIPlanCommand command)
    {
        var equipment = command.AvailableEquipment.Count > 0
            ? string.Join(", ", command.AvailableEquipment)
            : "bodyweight only";

        var focus = command.FocusAreas.Count > 0
            ? string.Join(", ", command.FocusAreas)
            : "full body";

        return $"""
            You are a professional fitness coach. Create a structured workout plan as valid JSON.

            User profile:
            - Goal: {command.Goal}
            - Fitness level: {command.FitnessLevel}
            - Days per week: {command.DaysPerWeek}
            - Available equipment: {equipment}
            - Focus areas: {focus}

            Respond with ONLY a JSON object matching this exact structure (no markdown, no explanation):
            {{
              "name": "Plan name",
              "description": "Brief description",
              "days": [
                {{
                  "dayOfWeek": 0,
                  "name": "Day name (e.g. Push Day)",
                  "exercises": [
                    {{
                      "exerciseName": "Exercise name",
                      "order": 1,
                      "sets": 3,
                      "reps": 10,
                      "durationSeconds": null,
                      "restSeconds": 90,
                      "notes": null
                    }}
                  ]
                }}
              ]
            }}

            Rules:
            - dayOfWeek: 0=Monday, 1=Tuesday, ..., 6=Sunday
            - Use exactly {command.DaysPerWeek} days spread evenly through the week
            - 4-6 exercises per day
            - reps and durationSeconds are mutually exclusive (one must be null)
            - restSeconds: 60-180
            - Use common, well-known exercise names
            """;
    }
}
