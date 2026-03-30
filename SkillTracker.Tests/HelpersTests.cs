using Xunit;

namespace SkillTracker.Tests;

public class HelpersTests {
    [Fact]
    public void TestGetNextSkillId() {
        var skills = new List<Skill>();

        int next = Helpers.GetNextSkillId(skills);

        Assert.Equal(1, next);
    }
}
