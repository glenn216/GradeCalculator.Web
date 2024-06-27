namespace GradeCalculator.Web.Models;

public class StudentLevelModel
{
    public Int32 LevelID { get; set; }
    public String LevelName { get; set; }
    private List<StudentLevelModel> Level { get; set; }

    public List<StudentLevelModel> GetLevel()
    {
        return this.Level;
    }
    public void SetLevel(List<StudentLevelModel> newLevel)
    {
        this.Level = newLevel;
    }
}