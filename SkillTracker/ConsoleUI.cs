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
            Console.WriteLine("   10. View Progress");
            Console.WriteLine();
            Console.WriteLine("[View Skills]");
            Console.WriteLine();
            Console.WriteLine("   11. View Skills List");
            Console.WriteLine("   12. View Skill Details");
            Console.WriteLine();
            Console.WriteLine("[Session]");
            Console.WriteLine();
            Console.WriteLine("   13. Exit");
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
                    ViewProgress();
                    break;
                case "11":
                    ViewSkillsList();
                    break;
                case "12":
                    ViewSkillDetails();
                    break;
                case "13":
                    running = false;
                    break;
                default:
                    Helpers.Pause("Invalid option.");
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
            Helpers.Pause("Skill name cannot be empty.");
            return;
        }

        foreach (var skill in skills) {
            if (skill.Name == name) {
                Helpers.Pause("A skill with that name already exists.");
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

        Helpers.Pause("Skill added successfully.");
    }

    private void AddGoal() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);

        Console.Clear();
        Console.WriteLine("=== Add Goal ===");

        if (skills.Count == 0) {
            Helpers.Pause("No skills found. Please add a skill.");
            return;
        }

        Helpers.WriteSkills(skills, "Available Skills:");

        Skill? chosenSkill = Helpers.PromptForSkill(skills, "Enter skill ID: ");
        if (chosenSkill == null) {
            return;
        }

        int selectedSkillId = chosenSkill.Id;

        Console.Write("Enter goal title: ");
        string title = Helpers.TrimText(Console.ReadLine());

        if (string.IsNullOrWhiteSpace(title)) {
            Helpers.Pause("Goal title cannot be empty.");
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

        Helpers.Pause("Goal added successfully.");
    }

    private void AddActivity() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Add Activity ===");

        if (skills.Count == 0) {
            Helpers.Pause("No skills found. Add a skill first.");
            return;
        }

        Helpers.WriteSkills(skills, "Available Skills:");

        Skill? activitySkill = Helpers.PromptForSkill(skills, "Enter skill ID: ");
        if (activitySkill == null) {
            return;
        }

        int selectedSkillId = activitySkill.Id;

        var skillGoals = new List<Goal>();
        foreach (var existingGoal in goals) {
            if (existingGoal.SkillId == selectedSkillId) {
                skillGoals.Add(existingGoal);
            }
        }

        if (skillGoals.Count == 0) {
            Helpers.Pause("No goals found for this skill. Please add a goal.");
            return;
        }

        Console.WriteLine("Available Goals:");
        Helpers.WriteGoals(skillGoals, labelStatus: false);

        Goal? chosenGoal = Helpers.PromptForGoal(skillGoals, "Enter goal ID: ");
        if (chosenGoal == null) {
            return;
        }

        int selectedGoalId = chosenGoal.Id;

        if (chosenGoal.Status == ProgressStatus.NotStarted) {
            chosenGoal.Status = ProgressStatus.InProgress;
        }

        Console.Write("Enter activity title: ");
        string title = Helpers.TrimText(Console.ReadLine());

        if (string.IsNullOrWhiteSpace(title)) {
            Helpers.Pause("Activity title cannot be empty.");
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

        Helpers.Pause("Activity added successfully.");
    }

    private void UpdateSkill() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);

        Console.Clear();
        Console.WriteLine("=== Update Skill ===");

        if (skills.Count == 0) {
            Helpers.Pause("No skills found.");
            return;
        }

        Helpers.WriteSkills(skills, "Skills:");

        Skill? skillToUpdate = Helpers.PromptForSkill(
            skills,
            "Enter skill ID to update: ",
            "Invalid skill ID.",
            "Skill not found."
        );
        if (skillToUpdate == null) {
            return;
        }

        int selectedSkillId = skillToUpdate.Id;

        Console.Write("Enter new skill name: ");
        string name = Helpers.TrimText(Console.ReadLine());

        if (string.IsNullOrWhiteSpace(name)) {
            Helpers.Pause("Skill name cannot be empty.");
            return;
        }

        foreach (var skill in skills) {
            if (skill.Id != selectedSkillId && skill.Name == name) {
                Helpers.Pause("Another skill already has that name.");
                return;
            }
        }

        skillToUpdate.Name = name;
        fileSaver.SaveData(SkillsFilename, skills);

        Helpers.Pause("Skill updated successfully.");
    }

    private void DeleteSkill() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Delete Skill ===");

        if (skills.Count == 0) {
            Helpers.Pause("No skills found.");
            return;
        }

        Helpers.WriteSkills(skills, "Skills:");

        Skill? skillToDelete = Helpers.PromptForSkill(
            skills,
            "Enter skill ID to delete: ",
            "Invalid skill ID.",
            "Skill not found."
        );
        if (skillToDelete == null) {
            return;
        }

        int selectedSkillId = skillToDelete.Id;

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

        Helpers.Pause("Skill deleted.");
    }

    private void UpdateGoal() {
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);

        Console.Clear();
        Console.WriteLine("=== Update Goal ===");

        if (goals.Count == 0) {
            Helpers.Pause("No goals found.");
            return;
        }

        Console.WriteLine("Goals:");
        Helpers.WriteGoals(goals);

        Goal? goalToUpdate = Helpers.PromptForGoal(
            goals,
            "Enter goal ID to update: ",
            "Invalid goal ID.",
            "Goal not found."
        );
        if (goalToUpdate == null) {
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
                Helpers.Pause("Goal title cannot be empty.");
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
                    Helpers.Pause("Invalid status option.");
                    return;
            }
        }
        else {
            Helpers.Pause("Invalid option.");
            return;
        }

        fileSaver.SaveData(GoalsFilename, goals);

        Helpers.Pause("Goal updated successfully.");
    }

    private void DeleteGoal() {
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Delete Goal ===");

        if (goals.Count == 0) {
            Helpers.Pause("No goals found.");
            return;
        }

        Console.WriteLine("Goals:");
        Helpers.WriteGoals(goals);

        Goal? goalToDelete = Helpers.PromptForGoal(
            goals,
            "Enter goal ID to delete: ",
            "Invalid goal ID.",
            "Goal not found."
        );
        if (goalToDelete == null) {
            return;
        }

        int selectedGoalId = goalToDelete.Id;

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

        Helpers.Pause("Goal deleted.");
    }

    private void UpdateActivity() {
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Update Activity ===");

        if (activities.Count == 0) {
            Helpers.Pause("No activities found.");
            return;
        }

        Console.WriteLine("Activities:");
        Helpers.WriteActivities(activities);

        Activity? activityToUpdate = Helpers.PromptForActivity(
            activities,
            "Enter activity ID to update: ",
            "Invalid activity ID.",
            "Activity not found."
        );
        if (activityToUpdate == null) {
            return;
        }

        Console.Write("Enter new activity title: ");
        string title = Helpers.TrimText(Console.ReadLine());

        if (string.IsNullOrWhiteSpace(title)) {
            Helpers.Pause("Activity title cannot be empty.");
            return;
        }

        activityToUpdate.Title = title;
        fileSaver.SaveData(ActivitiesFilename, activities);

        Helpers.Pause("Activity updated successfully.");
    }

    private void DeleteActivity() {
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Delete Activity ===");

        if (activities.Count == 0) {
            Helpers.Pause("No activities found.");
            return;
        }

        Console.WriteLine("Activities:");
        Helpers.WriteActivities(activities);

        Activity? activityToDelete = Helpers.PromptForActivity(
            activities,
            "Enter activity ID to delete: ",
            "Invalid activity ID.",
            "Activity not found."
        );
        if (activityToDelete == null) {
            return;
        }

        int selectedActivityId = activityToDelete.Id;

        var newActivities = new List<Activity>();
        foreach (var activity in activities) {
            if (activity.Id != selectedActivityId) {
                newActivities.Add(activity);
            }
        }

        fileSaver.SaveData(ActivitiesFilename, newActivities);

        Helpers.Pause("Activity deleted.");
    }

    private void ViewProgress() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== View Progress ===");
        Console.WriteLine();

        if (skills.Count == 0) {
            Helpers.Pause("No skills found.");
            return;
        }

        int allGoalsTotal = 0;
        int allGoalsCompleted = 0;

        foreach (var skill in skills) {
            int totalGoals = Helpers.CountGoalsForSkill(goals, skill.Id);
            int completedGoals = Helpers.CountCompletedGoalsForSkill(goals, skill.Id);
            int percent = Helpers.CalculateProgressPercent(completedGoals, totalGoals);

            allGoalsTotal = allGoalsTotal + totalGoals;
            allGoalsCompleted = allGoalsCompleted + completedGoals;

            Console.WriteLine($"Skill: {skill.Name} (ID: {skill.Id})");
            if (totalGoals == 0) {
                Console.WriteLine("  Goals: 0 (no goals yet)");
                Console.WriteLine("  Progress: 0%");
            }
            else {
                Console.WriteLine($"  Goals completed: {completedGoals} / {totalGoals}");
                Console.WriteLine($"  Progress: {percent}%");
            }

            var skillGoals = new List<Goal>();
            foreach (var goal in goals) {
                if (goal.SkillId == skill.Id) {
                    skillGoals.Add(goal);
                }
            }

            foreach (var goal in skillGoals) {
                int activityCount = Helpers.CountActivitiesForGoal(activities, goal.Id);
                Console.WriteLine($"    Goal: {goal.Title} — Status: {goal.Status}");
            }

            Console.WriteLine();
        }

        int overallPercent = Helpers.CalculateProgressPercent(allGoalsCompleted, allGoalsTotal);
        Console.WriteLine("--- Overall ---");
        if (allGoalsTotal == 0) {
            Console.WriteLine("No goals yet across all skills.");
        }
        else {
            Console.WriteLine($"Goals completed: {allGoalsCompleted} / {allGoalsTotal}");
            Console.WriteLine($"Overall progress: {overallPercent}%");
        }

        Helpers.Pause();
    }

    private void ViewSkillsList() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);

        Console.Clear();
        Console.WriteLine("=== View Skills List ===");

        if (skills.Count == 0) {
            Helpers.Pause("No skills found.");
            return;
        }

        Helpers.WriteSkills(skills);

        Helpers.Pause();
    }

    private void ViewSkillDetails() {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== View Skill Details ===");

        if (skills.Count == 0) {
            Helpers.Pause("No skills found.");
            return;
        }

        Helpers.WriteSkills(skills, "Skills:");

        Skill? selectedSkill = Helpers.PromptForSkill(
            skills,
            "Enter skill ID: ",
            "Invalid skill ID.",
            "Skill not found."
        );
        if (selectedSkill == null) {
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Skill: {selectedSkill.Name} (ID: {selectedSkill.Id})");

        var skillGoals = new List<Goal>();
        foreach (var goal in goals) {
            if (goal.SkillId == selectedSkill.Id) {
                skillGoals.Add(goal);
            }
        }

        if (skillGoals.Count == 0) {
            Helpers.Pause("No goals for this skill.");
            return;
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

        Helpers.Pause();
    }
}
