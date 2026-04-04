namespace SkillTracker;

public static class Helpers {
    public static int GetNextSkillId(List<Skill> skills) {
        int nextSkillId = 1;

        foreach (var skill in skills) {
            if (skill.Id >= nextSkillId) {
                nextSkillId = skill.Id + 1;
            }
        }

        return nextSkillId;
    }

    public static int GetNextGoalId(List<Goal> goals) {
        int nextGoalId = 1;

        foreach (var goal in goals) {
            if (goal.Id >= nextGoalId) {
                nextGoalId = goal.Id + 1;
            }
        }

        return nextGoalId;
    }

    public static int GetNextActivityId(List<Activity> activities) {
        int nextActivityId = 1;

        foreach (var activity in activities) {
            if (activity.Id >= nextActivityId) {
                nextActivityId = activity.Id + 1;
            }
        }

        return nextActivityId;
    }

    public static string TrimText(string? text) {
        if (text == null) {
            return "";
        }

        return text.Trim();
    }

    public static int? ParseIntOrNull(string? text) {
        string trimmed = TrimText(text);
        try {
            return int.Parse(trimmed);
        } catch (Exception ex) when (ex is FormatException or OverflowException) {
            return null;
        }
    }

    public static int CountGoalsForSkill(List<Goal> goals, int skillId) {
        int count = 0;
        foreach (var goal in goals) {
            if (goal.SkillId == skillId) {
                count = count + 1;
            }
        }
        return count;
    }

    public static int CountCompletedGoalsForSkill(List<Goal> goals, int skillId) {
        int count = 0;
        foreach (var goal in goals) {
            if (goal.SkillId == skillId && goal.Status == ProgressStatus.Completed) {
                count = count + 1;
            }
        }
        return count;
    }

    public static int CountActivitiesForGoal(List<Activity> activities, int goalId) {
        int count = 0;
        foreach (var activity in activities) {
            if (activity.GoalId == goalId) {
                count = count + 1;
            }
        }
        return count;
    }

    public static int CalculateProgressPercent(int completedGoals, int totalGoals) {
        if (totalGoals <= 0) {
            return 0;
        }
        return (completedGoals * 100) / totalGoals;
    }

    public static void Pause(string? message = null) {
        if (!string.IsNullOrEmpty(message)) {
            Console.WriteLine(message);
        }

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    public static Skill? PromptForSkill(
        List<Skill> skills,
        string prompt,
        string invalidIdMessage = "Invalid skill ID.",
        string? notFoundMessage = null
    ) {
        Console.Write(prompt);
        int? id = ParseIntOrNull(TrimText(Console.ReadLine()));
        if (id is null) {
            Pause(invalidIdMessage);
            return null;
        }

        foreach (var skill in skills) {
            if (skill.Id == id.Value) {
                return skill;
            }
        }

        Pause(notFoundMessage ?? invalidIdMessage);
        return null;
    }

    public static Goal? PromptForGoal(
        List<Goal> goals,
        string prompt,
        string invalidIdMessage = "Invalid goal ID.",
        string? notFoundMessage = null
    ) {
        Console.Write(prompt);
        int? id = ParseIntOrNull(TrimText(Console.ReadLine()));
        if (id is null) {
            Pause(invalidIdMessage);
            return null;
        }

        foreach (var goal in goals) {
            if (goal.Id == id.Value) {
                return goal;
            }
        }

        Pause(notFoundMessage ?? invalidIdMessage);
        return null;
    }

    public static Activity? PromptForActivity(
        List<Activity> activities,
        string prompt,
        string invalidIdMessage = "Invalid activity ID.",
        string? notFoundMessage = null
    ) {
        Console.Write(prompt);
        int? id = ParseIntOrNull(TrimText(Console.ReadLine()));
        if (id is null) {
            Pause(invalidIdMessage);
            return null;
        }

        foreach (var activity in activities) {
            if (activity.Id == id.Value) {
                return activity;
            }
        }

        Pause(notFoundMessage ?? invalidIdMessage);
        return null;
    }

    public static void WriteSkills(List<Skill> skills, string? heading = null) {
        if (heading != null) {
            Console.WriteLine(heading);
        }

        foreach (var skill in skills) {
            Console.WriteLine($"{skill.Id}. {skill.Name}");
        }
    }

    public static void WriteGoals(List<Goal> goals, bool labelStatus = true) {
        foreach (var goal in goals) {
            if (labelStatus) {
                Console.WriteLine($"{goal.Id}. {goal.Title} (Status: {goal.Status})");
            }
            else {
                Console.WriteLine($"{goal.Id}. {goal.Title} ({goal.Status})");
            }
        }
    }

    public static void WriteActivities(List<Activity> activities) {
        foreach (var activity in activities) {
            Console.WriteLine($"{activity.Id}. {activity.Title} ({activity.Date:g})");
        }
    }
}
