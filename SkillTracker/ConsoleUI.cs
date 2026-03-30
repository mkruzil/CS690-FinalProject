namespace SkillTracker;

public class ConsoleUI {
    private readonly FileSaver fileSaver = new();

    private const string SkillsFilename = "data/skills.json";
    private const string GoalsFilename = "data/goals.json";
    private const string ActivitiesFilename = "data/activities.json";

    public void Show() {
        bool running = true;

        while (running) {
            Console.Clear();
            Console.WriteLine("=== Skill Tracker ===");
            Console.WriteLine();
            Console.WriteLine("[Manage Skills]");
            Console.WriteLine();
            Console.WriteLine("   Add");
            Console.WriteLine("   1. Skill");
            Console.WriteLine("   2. Goal");
            Console.WriteLine("   3. Activity");
            Console.WriteLine();
            Console.WriteLine("   Update");
            Console.WriteLine("   4. Skill");
            Console.WriteLine("   5. Goal");
            Console.WriteLine("   6. Activity");
            Console.WriteLine();
            Console.WriteLine("   Delete");
            Console.WriteLine("   7. Skill");
            Console.WriteLine("   8. Goal");
            Console.WriteLine("   9. Activity");
            Console.WriteLine();
            Console.WriteLine("[View Progress]");
            Console.WriteLine();
            Console.WriteLine("   10. View Skills");
            Console.WriteLine();
            Console.WriteLine("[Session]");
            Console.WriteLine();
            Console.WriteLine("   11. Exit");
            Console.WriteLine();
            Console.Write("Select an option: ");

            string choice = Helpers.TrimText(Console.ReadLine());

            switch (choice) {
                case "1":
                    AddSkill();
                    break;
                case "2":
                    AddGoal();
                    break;
                case "3":
                    AddActivity();
                    break;
                case "4":
                    UpdateSkill();
                    break;
                case "5":
                    UpdateGoal();
                    break;
                case "6":
                    UpdateActivity();
                    break;
                case "7":
                    DeleteSkill();
                    break;
                case "8":
                    DeleteGoal();
                    break;
                case "9":
                    DeleteActivity();
                    break;
                case "10":
                    ViewSkills();
                    break;
                case "11":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private void AddSkill() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);

        Console.Clear();
        Console.WriteLine("=== Add Skill ===");
        Console.Write("Enter skill name: ");
        string name = Helpers.TrimText(Console.ReadLine());

        if (string.IsNullOrWhiteSpace(name)) {
            Console.WriteLine("Skill name cannot be empty.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        foreach (var skill in skills) {
            if (skill.Name == name) {
                Console.WriteLine("A skill with that name already exists.");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }
        }

        int nextSkillId = Helpers.GetNextSkillId(skills);

        var newSkill = new Skill {
            Id = nextSkillId,
            Name = name
        };
        skills.Add(newSkill);

        fileSaver.SaveData(SkillsFilename, skills);

        Console.WriteLine("Skill added successfully.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void AddGoal() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);

        Console.Clear();
        Console.WriteLine("=== Add Goal ===");

        if (skills.Count == 0) {
            Console.WriteLine("No skills found. Please add a skill.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Available Skills:");
        foreach (var skill in skills) {
            Console.WriteLine($"{skill.Id}. {skill.Name}");
        }

        Console.Write("Enter skill ID: ");
        string skillInput = Helpers.TrimText(Console.ReadLine());

        int? parsedSkillId = Helpers.ParseIntOrNull(skillInput);
        if (parsedSkillId is null) {
            Console.WriteLine("Invalid skill ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int selectedSkillId = parsedSkillId.Value;

        bool isValidSkill = false;

        foreach (var existingSkill in skills) {
            if (existingSkill.Id == selectedSkillId) {
                isValidSkill = true;
                break;
            }
        }

        if (!isValidSkill) {
            Console.WriteLine("Invalid skill ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter goal title: ");
        string title = Helpers.TrimText(Console.ReadLine());

        if (string.IsNullOrWhiteSpace(title)) {
            Console.WriteLine("Goal title cannot be empty.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int nextGoalId = Helpers.GetNextGoalId(goals);

        var newGoal = new Goal {
            Id = nextGoalId,
            SkillId = selectedSkillId,
            Title = title,
            Status = ProgressStatus.NotStarted
        };
        goals.Add(newGoal);

        fileSaver.SaveData(GoalsFilename, goals);

        Console.WriteLine("Goal added successfully.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void AddActivity() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Add Activity ===");

        if (skills.Count == 0) {
            Console.WriteLine("No skills found. Add a skill first.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Available Skills:");
        foreach (var skill in skills) {
            Console.WriteLine($"{skill.Id}. {skill.Name}");
        }

        Console.Write("Enter skill ID: ");
        string skillInput = Helpers.TrimText(Console.ReadLine());

        int? parsedSkillId = Helpers.ParseIntOrNull(skillInput);
        if (parsedSkillId is null) {
            Console.WriteLine("Invalid skill ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int selectedSkillId = parsedSkillId.Value;

        bool isValidSkill = false;

        foreach (var existingSkill in skills) {
            if (existingSkill.Id == selectedSkillId) {
                isValidSkill = true;
                break;
            }
        }

        if (!isValidSkill) {
            Console.WriteLine("Invalid skill ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        var skillGoals = new List<Goal>();
        foreach (var existingGoal in goals) {
            if (existingGoal.SkillId == selectedSkillId) {
                skillGoals.Add(existingGoal);
            }
        }

        if (skillGoals.Count == 0) {
            Console.WriteLine("No goals found for this skill. Please add a goal.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Available Goals:");
        foreach (var goal in skillGoals) {
            Console.WriteLine($"{goal.Id}. {goal.Title} ({goal.Status})");
        }

        Console.Write("Enter goal ID: ");
        string goalInput = Helpers.TrimText(Console.ReadLine());

        int? parsedGoalId = Helpers.ParseIntOrNull(goalInput);
        if (parsedGoalId is null) {
            Console.WriteLine("Invalid goal ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int selectedGoalId = parsedGoalId.Value;

        bool isValidGoal = false;
        foreach (var existingGoal in skillGoals) {
            if (existingGoal.Id == selectedGoalId) {
                isValidGoal = true;
                break;
            }
        }

        if (!isValidGoal) {
            Console.WriteLine("Invalid goal ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        foreach (var existingGoal in skillGoals) {
            if (existingGoal.Id == selectedGoalId) {
                if (existingGoal.Status == ProgressStatus.NotStarted) {
                    existingGoal.Status = ProgressStatus.InProgress;
                }
                break;
            }
        }

        Console.Write("Enter activity title: ");
        string title = Helpers.TrimText(Console.ReadLine());

        if (string.IsNullOrWhiteSpace(title)) {
            Console.WriteLine("Activity title cannot be empty.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int nextActivityId = Helpers.GetNextActivityId(activities);

        var newActivity = new Activity {
            Id = nextActivityId,
            GoalId = selectedGoalId,
            Title = title,
            Date = DateTime.Now
        };
        activities.Add(newActivity);

        fileSaver.SaveData(ActivitiesFilename, activities);
        fileSaver.SaveData(GoalsFilename, goals);

        Console.WriteLine("Activity added successfully.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void UpdateSkill() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);

        Console.Clear();
        Console.WriteLine("=== Update Skill ===");

        if (skills.Count == 0) {
            Console.WriteLine("No skills found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Skills:");
        foreach (var skill in skills) {
            Console.WriteLine($"{skill.Id}. {skill.Name}");
        }

        Console.Write("Enter skill ID to update: ");
        string skillInput = Helpers.TrimText(Console.ReadLine());

        int? parsedSkillId = Helpers.ParseIntOrNull(skillInput);
        if (parsedSkillId is null) {
            Console.WriteLine("Invalid skill ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int selectedSkillId = parsedSkillId.Value;

        Skill? skillToUpdate = null;
        foreach (var skill in skills) {
            if (skill.Id == selectedSkillId) {
                skillToUpdate = skill;
                break;
            }
        }

        if (skillToUpdate == null) {
            Console.WriteLine("Skill not found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter new skill name: ");
        string name = Helpers.TrimText(Console.ReadLine());

        if (string.IsNullOrWhiteSpace(name)) {
            Console.WriteLine("Skill name cannot be empty.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        foreach (var skill in skills) {
            if (skill.Id != selectedSkillId && skill.Name == name) {
                Console.WriteLine("Another skill already has that name.");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }
        }

        skillToUpdate.Name = name;
        fileSaver.SaveData(SkillsFilename, skills);

        Console.WriteLine("Skill updated successfully.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void DeleteSkill() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Delete Skill ===");

        if (skills.Count == 0) {
            Console.WriteLine("No skills found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Skills:");
        foreach (var skill in skills) {
            Console.WriteLine($"{skill.Id}. {skill.Name}");
        }

        Console.Write("Enter skill ID to delete: ");
        string skillInput = Helpers.TrimText(Console.ReadLine());

        int? parsedSkillId = Helpers.ParseIntOrNull(skillInput);
        if (parsedSkillId is null) {
            Console.WriteLine("Invalid skill ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int selectedSkillId = parsedSkillId.Value;

        bool isValidSkill = false;
        foreach (var existingSkill in skills) {
            if (existingSkill.Id == selectedSkillId) {
                isValidSkill = true;
                break;
            }
        }

        if (!isValidSkill) {
            Console.WriteLine("Skill not found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        var goalIdsToRemove = new List<int>();
        foreach (var goal in goals) {
            if (goal.SkillId == selectedSkillId) {
                goalIdsToRemove.Add(goal.Id);
            }
        }

        var newActivities = new List<Activity>();
        foreach (var activity in activities) {
            bool removeActivity = false;
            foreach (var goalId in goalIdsToRemove) {
                if (activity.GoalId == goalId) {
                    removeActivity = true;
                    break;
                }
            }
            if (!removeActivity) {
                newActivities.Add(activity);
            }
        }

        var newGoals = new List<Goal>();
        foreach (var goal in goals) {
            if (goal.SkillId != selectedSkillId) {
                newGoals.Add(goal);
            }
        }

        var newSkills = new List<Skill>();
        foreach (var skill in skills) {
            if (skill.Id != selectedSkillId) {
                newSkills.Add(skill);
            }
        }

        fileSaver.SaveData(ActivitiesFilename, newActivities);
        fileSaver.SaveData(GoalsFilename, newGoals);
        fileSaver.SaveData(SkillsFilename, newSkills);

        Console.WriteLine("Skill deleted.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void UpdateGoal() {
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);

        Console.Clear();
        Console.WriteLine("=== Update Goal ===");

        if (goals.Count == 0) {
            Console.WriteLine("No goals found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Goals:");
        foreach (var goal in goals) {
            Console.WriteLine($"{goal.Id}. {goal.Title} (Status: {goal.Status})");
        }

        Console.Write("Enter goal ID to update: ");
        string goalInput = Helpers.TrimText(Console.ReadLine());

        int? parsedGoalId = Helpers.ParseIntOrNull(goalInput);
        if (parsedGoalId is null) {
            Console.WriteLine("Invalid goal ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int selectedGoalId = parsedGoalId.Value;

        Goal? goalToUpdate = null;
        foreach (var goal in goals) {
            if (goal.Id == selectedGoalId) {
                goalToUpdate = goal;
                break;
            }
        }

        if (goalToUpdate == null) {
            Console.WriteLine("Goal not found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("1. Change title");
        Console.WriteLine("2. Change status");
        Console.Write("Select an option: ");
        string mode = Helpers.TrimText(Console.ReadLine());

        if (mode == "1") {
            Console.Write("Enter new goal title: ");
            string title = Helpers.TrimText(Console.ReadLine());

            if (string.IsNullOrWhiteSpace(title)) {
                Console.WriteLine("Goal title cannot be empty.");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            goalToUpdate.Title = title;
        }
        else if (mode == "2") {
            Console.WriteLine();
            Console.WriteLine("Select new status:");
            Console.WriteLine("1. NotStarted");
            Console.WriteLine("2. InProgress");
            Console.WriteLine("3. Completed");
            Console.Write("Select an option: ");
            string statusChoice = Helpers.TrimText(Console.ReadLine());

            switch (statusChoice) {
                case "1":
                    goalToUpdate.Status = ProgressStatus.NotStarted;
                    break;
                case "2":
                    goalToUpdate.Status = ProgressStatus.InProgress;
                    break;
                case "3":
                    goalToUpdate.Status = ProgressStatus.Completed;
                    break;
                default:
                    Console.WriteLine("Invalid status option.");
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    return;
            }
        }
        else {
            Console.WriteLine("Invalid option.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        fileSaver.SaveData(GoalsFilename, goals);

        Console.WriteLine("Goal updated successfully.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void DeleteGoal() {
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Delete Goal ===");

        if (goals.Count == 0) {
            Console.WriteLine("No goals found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Goals:");
        foreach (var goal in goals) {
            Console.WriteLine($"{goal.Id}. {goal.Title} (Status: {goal.Status})");
        }

        Console.Write("Enter goal ID to delete: ");
        string goalInput = Helpers.TrimText(Console.ReadLine());

        int? parsedGoalId = Helpers.ParseIntOrNull(goalInput);
        if (parsedGoalId is null) {
            Console.WriteLine("Invalid goal ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int selectedGoalId = parsedGoalId.Value;

        bool isValidGoal = false;
        foreach (var existingGoal in goals) {
            if (existingGoal.Id == selectedGoalId) {
                isValidGoal = true;
                break;
            }
        }

        if (!isValidGoal) {
            Console.WriteLine("Goal not found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        var newActivities = new List<Activity>();
        foreach (var activity in activities) {
            if (activity.GoalId != selectedGoalId) {
                newActivities.Add(activity);
            }
        }

        var newGoals = new List<Goal>();
        foreach (var goal in goals) {
            if (goal.Id != selectedGoalId) {
                newGoals.Add(goal);
            }
        }

        fileSaver.SaveData(ActivitiesFilename, newActivities);
        fileSaver.SaveData(GoalsFilename, newGoals);

        Console.WriteLine("Goal deleted.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void UpdateActivity() {
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Update Activity ===");

        if (activities.Count == 0) {
            Console.WriteLine("No activities found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Activities:");
        foreach (var activity in activities) {
            Console.WriteLine($"{activity.Id}. {activity.Title} ({activity.Date:g})");
        }

        Console.Write("Enter activity ID to update: ");
        string activityInput = Helpers.TrimText(Console.ReadLine());

        int? parsedActivityId = Helpers.ParseIntOrNull(activityInput);
        if (parsedActivityId is null) {
            Console.WriteLine("Invalid activity ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int selectedActivityId = parsedActivityId.Value;

        Activity? activityToUpdate = null;
        foreach (var activity in activities) {
            if (activity.Id == selectedActivityId) {
                activityToUpdate = activity;
                break;
            }
        }

        if (activityToUpdate == null) {
            Console.WriteLine("Activity not found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter new activity title: ");
        string title = Helpers.TrimText(Console.ReadLine());

        if (string.IsNullOrWhiteSpace(title)) {
            Console.WriteLine("Activity title cannot be empty.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        activityToUpdate.Title = title;
        fileSaver.SaveData(ActivitiesFilename, activities);

        Console.WriteLine("Activity updated successfully.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void DeleteActivity() {
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Delete Activity ===");

        if (activities.Count == 0) {
            Console.WriteLine("No activities found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Activities:");
        foreach (var activity in activities) {
            Console.WriteLine($"{activity.Id}. {activity.Title} ({activity.Date:g})");
        }

        Console.Write("Enter activity ID to delete: ");
        string activityInput = Helpers.TrimText(Console.ReadLine());

        int? parsedActivityId = Helpers.ParseIntOrNull(activityInput);
        if (parsedActivityId is null) {
            Console.WriteLine("Invalid activity ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int selectedActivityId = parsedActivityId.Value;

        bool isValidActivity = false;
        foreach (var existingActivity in activities) {
            if (existingActivity.Id == selectedActivityId) {
                isValidActivity = true;
                break;
            }
        }

        if (!isValidActivity) {
            Console.WriteLine("Activity not found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        var newActivities = new List<Activity>();
        foreach (var activity in activities) {
            if (activity.Id != selectedActivityId) {
                newActivities.Add(activity);
            }
        }

        fileSaver.SaveData(ActivitiesFilename, newActivities);

        Console.WriteLine("Activity deleted.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void ViewSkills() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== View Skills ===");

        if (skills.Count == 0) {
            Console.WriteLine("No skills found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        foreach (var skill in skills) {
            Console.WriteLine($"\nSkill: {skill.Name} (ID: {skill.Id})");

            var skillGoals = new List<Goal>();
            foreach (var goal in goals) {
                if (goal.SkillId == skill.Id) {
                    skillGoals.Add(goal);
                }
            }

            if (skillGoals.Count == 0) {
                Console.WriteLine("  No goals.");
                continue;
            }

            foreach (var goal in skillGoals) {
                Console.WriteLine($"  Goal: {goal.Title} (ID: {goal.Id}, Status: {goal.Status})");

                var goalActivities = new List<Activity>();
                foreach (var activity in activities) {
                    if (activity.GoalId == goal.Id) {
                        goalActivities.Add(activity);
                    }
                }

                if (goalActivities.Count == 0) {
                    Console.WriteLine("    No activities.");
                    continue;
                }

                foreach (var activity in goalActivities) {
                    Console.WriteLine($"    Activity: {activity.Title} ({activity.Date:g})");
                }
            }
        }

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}
