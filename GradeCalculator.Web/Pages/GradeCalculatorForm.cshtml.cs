using GradeCalculator.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GradeCalculator.Web.Pages;

public class GradeCalculatorForm : PageModel
{
    public StudentInformationModel studentInformationModel;
    
    public void OnGet(StudentInformationModel newStudentInformationModel)
    {
        this.studentInformationModel = newStudentInformationModel;
        Console.WriteLine("Name: " + studentInformationModel.Name);
        Console.WriteLine("Year/Level: " + studentInformationModel.Level);
    }
}