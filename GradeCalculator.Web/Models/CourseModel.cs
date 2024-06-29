namespace GradeCalculator.Web.Models;

public class CourseModel
{
    public String CourseCode { get; set; }
    public String CourseName { get; set; }
    
    private List<CourseModel> Course { get; set; }
    
    public List<CourseModel> GetCourse()
    {
        return this.Course;
    }
    public void SetCourse(List<CourseModel> newCourse)
    {
        this.Course = newCourse;
    }
}