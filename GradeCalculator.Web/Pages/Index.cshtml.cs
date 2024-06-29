using System.Data;
using System.Text.Json;
using GradeCalculator.Web.database;
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
            Name = Convert.ToString(Request.Form["inputName"]),
            LevelID = Convert.ToInt32(Request.Form["inputYearLevel"]),
            LevelName = GetLevelName(Convert.ToInt32(Request.Form["inputYearLevel"])),
            ProgramID = Convert.ToInt32(Request.Form["inputProgram"]),
            ProgramName = $"{GetProgramName(Convert.ToInt32(Request.Form["inputProgram"]))} ({GetProgramAbbrev(Convert.ToInt32(Request.Form["inputProgram"]))})",
            TermID = Convert.ToInt32(Request.Form["inputTerm"]),
            TermName = GetTermName(Convert.ToInt32(Request.Form["inputTerm"]))
        };
        string json = JsonSerializer.Serialize(studentInformationModel);
        //return RedirectToPage("GradeCalculatorForm", studentInformationModel);
        return RedirectToPage("GradeCalculatorForm", new { json });
    }
    private void InitializeProgram()
    {
        using (var connection = new SqliteConnection(Database.ConnectionString))
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
        using (var connection = new SqliteConnection(Database.ConnectionString))
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
        using (var connection = new SqliteConnection(Database.ConnectionString))
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
        using (var connection = new SqliteConnection(Database.ConnectionString))
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
        using (var connection = new SqliteConnection(Database.ConnectionString))
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
        using (var connection = new SqliteConnection(Database.ConnectionString))
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
        using (var connection = new SqliteConnection(Database.ConnectionString))
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