namespace GradeCalculator.Web.Models;

public class CumulativeGradeModel
{
    public String CourseName { get; set; }
    public Double Prelims { get; set; }
    public Double Midterms { get; set; }
    public Double Prefinals { get; set; }
    public Double Finals { get; set; }
    public List<Double> GeneralWeightedAverage { get; set; }
    public  List<Double> GetGeneralWeightedAverage()
    {
        return this.GeneralWeightedAverage;
    }
    public void SetGeneralWeightedAverage( List<Double> newGeneralWeightedAverage)
    {
        this.GeneralWeightedAverage = newGeneralWeightedAverage;
    }
    
    private List<CumulativeGradeModel> CumulativeGrade { get; set; }
    
    public List<CumulativeGradeModel> GetCumulativeGrade()
    {
        return this.CumulativeGrade;
    }
    public void SetCumulativeGrade(List<CumulativeGradeModel> newCumulativeGrade)
    {
        this.CumulativeGrade = newCumulativeGrade;
    }
}