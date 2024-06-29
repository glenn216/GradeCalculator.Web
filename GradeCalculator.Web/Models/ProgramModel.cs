namespace GradeCalculator.Web.Models;

public class ProgramModel
{
    public Int32 ProgramID { get; set; }
    public String ProgramAbbrev { get; set; }
    public String ProgramName { get; set; }
    
    private List<ProgramModel> Program { get; set; }
    
    public List<ProgramModel> GetProgram()
    {
        return this.Program;
    }
    public void SetProgram(List<ProgramModel> newProgram)
    {
        this.Program = newProgram;
    }
}