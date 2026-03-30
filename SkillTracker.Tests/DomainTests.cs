using System.Text.Json;
using Xunit;

namespace SkillTracker.Tests;

public class DomainTests {
    [Fact]
    public void TestJsonSerializer() {
        var original = new Skill { Id = 1, Name = "Cooking" };

        string json = JsonSerializer.Serialize(original);
        Skill? deserialized = JsonSerializer.Deserialize<Skill>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(original.Id, deserialized.Id);
        Assert.Equal(original.Name, deserialized.Name);
    }
}
