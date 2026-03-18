namespace SkillTracker;

public class ConsoleUI
{
    private readonly FileSaver fileSaver = new();

    private const string SkillsFilename = "data/skills.json";
    private const string GoalsFilename = "data/goals.json";
    private const string ActivitiesFilename = "data/activities.json";

    public void Show()
    {
        
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Skill Tracker ===");
            Console.WriteLine("1. Add Skill");
            Console.WriteLine("2. Add Goal");
            Console.WriteLine("3. Add Activity");
            Console.WriteLine("4. View Skills");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            if (choice == null) choice = "";

            switch (choice)
            {
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
                    ViewSkills();
                    break;
                case "5":
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

    private void AddSkill()
    {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);

        Console.Clear();
        Console.WriteLine("=== Add Skill ===");
        Console.Write("Enter skill name: ");
        string name = Console.ReadLine();
        if (name == null) name = "";

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Skill name cannot be empty.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int nextSkillId = 1;

        foreach (var skill in skills)
        {
            if (skill.Id >= nextSkillId)
            {
                nextSkillId = skill.Id + 1;
            }
        }

        var skill = new Skill { 
            Id = nextSkillId, 
            Name = name.Trim() 
        };
        skills.Add(skill);

        fileSaver.SaveData(SkillsFilename, skills);

        Console.WriteLine("Skill added successfully.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void AddGoal()
    {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);

        Console.Clear();
        Console.WriteLine("=== Add Goal ===");

        if (skills.Count == 0)
        {
            Console.WriteLine("No skills found. Please add a skill.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Available Skills:");
        foreach (var skill in skills)
        {
            Console.WriteLine($"{skill.Id}. {skill.Name}");
        }

        Console.Write("Enter skill ID: ");
        string skillInput = Console.ReadLine();
        if (skillInput == null) skillInput = "";

        int skillId;
        try
        {
            skillId = int.Parse(skillInput);
        }
        catch
        {
            Console.WriteLine("Invalid skill ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        bool isValidSkill = false;

        foreach (var skill in skills)
        {
            if (skill.Id == skillId)
            {
                isValidSkill = true;
                break;
            }
        }

        if (!isValidSkill)
        {
            Console.WriteLine("Invalid skill ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter goal title: ");
        string title = Console.ReadLine();
        if (title == null) title = "";

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Goal title cannot be empty.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int nextGoalId = 1;

        foreach (var goal in goals)
        {
            if (goal.Id >= nextGoalId)
            {
                nextGoalId = goal.Id + 1;
            }
        }

        var goal = new Goal {
            Id = nextGoalId,
            SkillId = skillId,
            Title = title.Trim(),
            Status = ProgressStatus.NotStarted
        };
        goals.Add(goal);

        fileSaver.SaveData(GoalsFilename, goals);

        Console.WriteLine("Goal added successfully.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void AddActivity()
    {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== Add Activity ===");

        if (skills.Count == 0)
        {
            Console.WriteLine("No skills found. Add a skill first.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Available Skills:");
        foreach (var skill in skills)
        {
            Console.WriteLine($"{skill.Id}. {skill.Name}");
        }

        Console.Write("Enter skill ID: ");
        string skillInput = Console.ReadLine();
        if (skillInput == null) skillInput = "";

        int skillId;
        try
        {
            skillId = int.Parse(skillInput);
        }
        catch
        {
            Console.WriteLine("Invalid skill ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        bool isValidSkill = false;

        foreach (var skill in skills)
        {
            if (skill.Id == skillId)
            {
                isValidSkill = true;
                break;
            }
        }

        if (!isValidSkill)
        {
            Console.WriteLine("Invalid skill ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        var skillGoals = new List<Goal>();
        foreach (var goal in goals)
        {
            if (goal.SkillId == skillId)
            {
                skillGoals.Add(goal);
            }
        }

        if (skillGoals.Count == 0)
        {
            Console.WriteLine("No goals found for this skill. Please add a goal.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Available Goals:");
        foreach (var goal in skillGoals)
        {
            Console.WriteLine($"{goal.Id}. {goal.Title} ({goal.Status})");
        }

        Console.Write("Enter goal ID: ");
        string goalInput = Console.ReadLine();
        if (goalInput == null) goalInput = "";

        int goalId;
        try
        {
            goalId = int.Parse(goalInput);
        }
        catch
        {
            Console.WriteLine("Invalid goal ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        bool isValidGoal = false;
        foreach (var goal in skillGoals)
        {
            if (goal.Id == goalId)
            {
                isValidGoal = true;
                break;
            }
        }

        if (!isValidGoal)
        {
            Console.WriteLine("Invalid goal ID.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter activity title: ");
        string title = Console.ReadLine();
        if (title == null) title = "";

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Activity title cannot be empty.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        int nextActivityId = 1;

        foreach (var activity in activities)
        {
            if (activity.Id >= nextActivityId)
            {
                nextActivityId = activity.Id + 1;
            }
        }

        var activity = new Activity {
            Id = nextActivityId,
            GoalId = goalId,
            Title = title.Trim(),
            Date = DateTime.Now
        };
        activities.Add(activity);

        fileSaver.SaveData(ActivitiesFilename, activities);
        fileSaver.SaveData(GoalsFilename, goals);

        Console.WriteLine("Activity added successfully.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    private void ViewSkills()
    {
        var skills = fileSaver.LoadData<Skill>(SkillsFilename);
        var goals = fileSaver.LoadData<Goal>(GoalsFilename);
        var activities = fileSaver.LoadData<Activity>(ActivitiesFilename);

        Console.Clear();
        Console.WriteLine("=== View Skills ===");

        if (skills.Count == 0)
        {
            Console.WriteLine("No skills found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            return;
        }

        foreach (var skill in skills)
        {
            Console.WriteLine($"\nSkill: {skill.Name} (ID: {skill.Id})");

            var skillGoals = new List<Goal>();
            foreach (var goal in goals)
            {
                if (goal.SkillId == skill.Id)
                {
                    skillGoals.Add(goal);
                }
            }

            if (skillGoals.Count == 0)
            {
                Console.WriteLine("  No goals.");
                continue;
            }

            foreach (var goal in skillGoals)
            {
                Console.WriteLine($"  Goal: {goal.Title} (ID: {goal.Id}, Status: {goal.Status})");

                var goalActivities = new List<Activity>();
                foreach (var activity in activities)
                {
                    if (activity.GoalId == goal.Id)
                    {
                        goalActivities.Add(activity);
                    }
                }

                if (goalActivities.Count == 0)
                {
                    Console.WriteLine("    No activities.");
                    continue;
                }

                foreach (var activity in goalActivities)
                {
                    Console.WriteLine($"    Activity: {activity.Title} ({activity.Date:g})");
                }
            }
        }

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

}