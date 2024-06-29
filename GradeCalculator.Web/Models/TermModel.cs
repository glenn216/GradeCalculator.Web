namespace GradeCalculator.Web.Models;

public class TermModel
{
    public Int32 TermID { get; set; }
    public String TermName { get; set; }
    
    private List<TermModel> Term { get; set; }
    
    public List<TermModel> GetTerm()
    {
        return this.Term;
    }
    public void SetTerm(List<TermModel> newTerm)
    {
        this.Term = newTerm;
    }
}