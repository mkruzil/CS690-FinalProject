using Xunit;

namespace SkillTracker.Tests;

public class FileSaverTests {
    [Fact]
    public void TestLoadData() {
        string guid = Guid.NewGuid().ToString();
        string tmpFolder = Path.GetTempPath();
        string fileName = guid + ".json";
        string path = Path.Combine(tmpFolder, fileName);

        Assert.False(File.Exists(path));

        var saver = new FileSaver();
        List<Skill> result = saver.LoadData<Skill>(path);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
