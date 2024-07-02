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

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public readonly StudentLevelModel studentLevelModel = new StudentLevelModel();
    public readonly ProgramModel programModel = new ProgramModel();
    public readonly TermModel termModel = new TermModel();
    

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        InitializeYearLevel();
        InitializeProgram();
        InitializeTerm();
    }

    public IActionResult OnPost()
    {
        StudentInformationModel studentInformationModel = new StudentInformationModel()
        {
            StudentName = Convert.ToString(Request.Form["inputName"]),
            LevelID = Convert.ToInt32(Request.Form["inputYearLevel"]),
            ProgramID = Convert.ToInt32(Request.Form["inputProgram"]),
            TermID = Convert.ToInt32(Request.Form["inputTerm"])
        };
        
        return RedirectToPage("GradeCalculatorForm", new { Serialize = JsonSerializer.Serialize(studentInformationModel) });
    }
    private void InitializeProgram()
    {
        using (var connection = new SqliteConnection(Database.Database.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT ProgramID, ProgramAbbrev, ProgramName FROM tblProgram";

            using (var reader = command.ExecuteReader())
            {
                List<ProgramModel> Program = new List<ProgramModel>();
                while (reader.Read())
                {
                    ProgramModel programItem = new ProgramModel()
                    {
                        ProgramID = reader.GetInt32("ProgramID"),
                        ProgramAbbrev = reader.GetString("ProgramAbbrev"),
                        ProgramName = reader.GetString("ProgramName")
                    };
                    Program.Add(programItem);
                }
                programModel.SetProgram(Program);
            }
        }
    }

    private void InitializeYearLevel()
    {
        using (var connection = new SqliteConnection(Database.Database.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT YearLevelID, YearLevelName FROM tblYearLevel";

            using (var reader = command.ExecuteReader())
            {
                List<StudentLevelModel> Level = new List<StudentLevelModel>();
                while (reader.Read())
                {
                    StudentLevelModel levelItem = new StudentLevelModel()
                    {
                        LevelID = reader.GetInt32("YearLevelID"),
                        LevelName = reader.GetString("YearLevelName")
                    };
                    Level.Add(levelItem);
                }
                studentLevelModel.SetLevel(Level);
            }
        }
    }
    private void InitializeTerm()
    {
        using (var connection = new SqliteConnection(Database.Database.ConnectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"SELECT TermID, TermName FROM tblTerm";

            using (var reader = command.ExecuteReader())
            {
                List<TermModel> Term = new List<TermModel>();
                while (reader.Read())
                {
                    TermModel termItem = new TermModel()
                    {
                        TermID = reader.GetInt32("TermID"),
                        TermName = reader.GetString("TermName")
                    };
                    Term.Add(termItem);
                }
                termModel.SetTerm(Term);
            }
        }
    }
    private String GetLevelName(Int32 levelID)
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
    private String GetProgramName(Int32 programID)
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
    private String GetProgramAbbrev(Int32 programID)
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
    private String GetTermName(Int32 termID)
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