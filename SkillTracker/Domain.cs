namespace SkillTracker;

public enum ProgressStatus {
    NotStarted,
    InProgress,
    Completed
}

public class Skill {
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class Goal {
    public int Id { get; set; }
    public int SkillId { get; set; }
    public string Title { get; set; } = "";
    public ProgressStatus Status { get; set; } = ProgressStatus.NotStarted;
}

public class Activity {
    public int Id { get; set; }
    public int GoalId { get; set; }
    public string Title { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Now;
}
