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
    private DateTime currentDate = DateTime.Now;
    public async void OnGet(String Serialize)
    {
        this.studentInformationModel = JsonSerializer.Deserialize<StudentInformationModel>(Serialize);
        //this.studentInformationModel = newStudentInformationModel;
        Console.WriteLine("------ A user logged on ------");
        Console.WriteLine($"Name: {studentInformationModel.StudentName}");
        Console.WriteLine($"Year/Level: {GetLevelName(studentInformationModel.LevelID)}");
        Console.WriteLine($"Program: {GetProgramName(studentInformationModel.ProgramID)} ({GetProgramAbbrev(studentInformationModel.ProgramID)})");
        Console.WriteLine($"Term: {GetTermName(studentInformationModel.TermID)}");
        Console.WriteLine($"Date accessed: {currentDate}");
        Console.WriteLine("------------------------------");
        
        InitializeCourse(studentInformationModel.LevelID, studentInformationModel.ProgramID, studentInformationModel.TermID);
    }
    public async Task<IActionResult> OnPostCalculateFinalGrade()
    {
        var formData = Request.Form;
        Double PRELIMS = Convert.ToDouble(formData["inputPrelims"]) * .20;
        Double MIDTERMS = Convert.ToDouble(formData["inputMidterms"]) * .20;
        Double PREFINALS = Convert.ToDouble(formData["inputPrefinals"]) * .20;
        Double FINALS = Convert.ToDouble(formData["inputFinals"]) * .40;

        Double FINAL_GRADE = PRELIMS + MIDTERMS + PREFINALS + FINALS;
        FINAL_GRADE = Math.Round(FINAL_GRADE, 2);

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
            >= 81.50 and <= 85.49 => 2.25,
            >= 77.50 and <= 81.49 => 2.50,
            >= 73.50 and <= 77.49 => 2.75,
            >= 69.50 and <= 73.49 => 3.00,
            <= 69.49 => 5.00,
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
    public String GetLevelName(Int32 levelID)
    {
        using (var connection = new SqliteConnection(Database.Database.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT YearLevelName FROM tblYearLevel WHERE YearLevelID = @LevelID";
            command.Parameters.AddWithValue("@LevelID", levelID);

            using (var reader = command.ExecuteReader())
            {
                String YearLevelName = null;
                while (reader.Read())
                {
                    YearLevelName = reader.GetString("YearLevelName");
                }

                return YearLevelName;
            }
        }
    }
    public String GetProgramName(Int32 programID)
    {
        using (var connection = new SqliteConnection(Database.Database.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT ProgramName FROM tblProgram WHERE ProgramID = @ProgramID";
            command.Parameters.AddWithValue("@ProgramID", programID);

            using (var reader = command.ExecuteReader())
            {
                String ProgramName = null;
                while (reader.Read())
                {
                    ProgramName = reader.GetString("ProgramName");
                }

                return ProgramName;
            }
        }
    }
    public String GetProgramAbbrev(Int32 programID)
    {
        using (var connection = new SqliteConnection(Database.Database.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT ProgramAbbrev FROM tblProgram WHERE ProgramID = @ProgramID";
            command.Parameters.AddWithValue("@ProgramID", programID);

            using (var reader = command.ExecuteReader())
            {
                String ProgramAbbrev = null;
                while (reader.Read())
                {
                    ProgramAbbrev = reader.GetString("ProgramAbbrev");
                }

                return ProgramAbbrev;
            }
        }
    }
    public String GetTermName(Int32 termID)
    {
        using (var connection = new SqliteConnection(Database.Database.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT TermName FROM tblTerm WHERE TermID = @TermID";
            command.Parameters.AddWithValue("@TermID", termID);

            using (var reader = command.ExecuteReader())
            {
                String TermName = null;
                while (reader.Read())
                {
                    TermName = reader.GetString("TermName");
                }

                return TermName;
            }
        }
    }
}