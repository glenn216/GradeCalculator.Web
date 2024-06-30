#region MIT License
/*
 * Copyright (c) 2024 Glenn Alon
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using System.Data;
using System.Text.Json;
using GradeCalculator.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace GradeCalculator.Web.Pages;

public class GradeCalculatorForm : PageModel
{
    public StudentInformationModel studentInformationModel;
    public CourseModel courseModel = new CourseModel();
    public void OnGet(string json)
    {
        this.studentInformationModel = JsonSerializer.Deserialize<StudentInformationModel>(json);

        //this.studentInformationModel = newStudentInformationModel;
        Console.WriteLine("Name: " + studentInformationModel.Name);
        Console.WriteLine("Year/Level: " + studentInformationModel.LevelName);
        Console.WriteLine("Program: " + studentInformationModel.ProgramName);
        Console.WriteLine("Term: " + studentInformationModel.TermName);
        
        InitializeCourse(studentInformationModel.LevelID, studentInformationModel.ProgramID, studentInformationModel.TermID);
    }
    public async Task<IActionResult> OnPostCalculateFinalGrade()
    {
        var formData = Request.Form;
        String CourseName = formData["courseName"];
        Double PRELIMS = Convert.ToDouble(formData["inputPrelims"]) * .20;
        Double MIDTERMS = Convert.ToDouble(formData["inputMidterms"]) * .20;
        Double PREFINALS = Convert.ToDouble(formData["inputPrefinals"]) * .20;
        Double FINALS = Convert.ToDouble(formData["inputFinals"]) * .40;

        Double FINAL_GRADE = PRELIMS + MIDTERMS + PREFINALS + FINALS;
        FINAL_GRADE = Math.Round(FINAL_GRADE, 2);
       
        Console.WriteLine("--- " + formData["studentName"] + " ---");
        Console.WriteLine("Course: " + CourseName);
        Console.WriteLine("Final Grade: " + FINAL_GRADE);
        return await Task.FromResult<IActionResult>(new JsonResult(new
        {
            finalGrade = gpScale(FINAL_GRADE)
        }));

    }
    private double gpScale(double GRADE)
    {
        return GRADE switch
        {
            >= 97.50 and <= 100 => 1.00,
            >= 94.50 and <= 97.49 => 1.25,
            >= 91.50 and <= 94.49 => 1.50,
            >= 88.50 and <= 91.49 => 1.75,
            >= 85.50 and <= 88.49 => 2.00,
            >= 82.50 and <= 85.49 => 2.25,
            >= 79.50 and <= 82.49 => 2.50,
            >= 76.50 and <= 79.49 => 2.75,
            >= 74.50 and <= 76.49 => 3.00,
            <= 74.49 => 5.00,
            _ => 0.00
        };
    }

    private void InitializeCourse(Int32 YearLevelID, Int32 ProgramID, Int32 TermID)
    {
        using (var connection = new SqliteConnection(Database.Database.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT tblCourse.CourseCode, tblCourse.CourseName FROM tblCourse INNER JOIN tblCourseLoad ON tblCourse.CourseCode = tblCourseLoad.CourseCode WHERE YearLevelID = @YearLevelID AND ProgramID = @ProgramID AND TermID = @TermID";
            command.Parameters.AddWithValue("@YearLevelID", YearLevelID);
            command.Parameters.AddWithValue("@ProgramID", ProgramID);
            command.Parameters.AddWithValue("@TermID", TermID);
            using (var reader = command.ExecuteReader())
            {
                List<CourseModel> Course = new List<CourseModel>();
                while (reader.Read())
                {
                    CourseModel courseItem = new CourseModel()
                    {
                        CourseCode = reader.GetString("CourseCode"),
                        CourseName = reader.GetString("CourseName")
                    };
                    Course.Add(courseItem);
                }
                courseModel.SetCourse(Course);
            }
        }
    }
}