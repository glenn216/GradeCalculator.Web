namespace GradeCalculator.Web.Models;

public class StudentInformationModel
{
    public String Name { get; set; }
    public Int32 LevelID { get; set; }
    public String LevelName { get; set; }
    public Int32 ProgramID { get; set; }
    public String ProgramName { get; set; }
    public Int32 TermID { get; set; }
    public String TermName { get; set; }
    
    private List<StudentInformationModel> StudentInformation { get; set; }
    
    public List<StudentInformationModel> GetStudentInformation()
    {
        return this.StudentInformation;
    }
    public void SetStudentInformation(List<StudentInformationModel> newStudentInformation)
    {
        this.StudentInformation = newStudentInformation;
    }
}