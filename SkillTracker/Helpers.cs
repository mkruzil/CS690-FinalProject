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

    public static bool TryParseInt(string? text, out int value) {
        string trimmed = TrimText(text);
        return int.TryParse(trimmed, out value);
    }
}
