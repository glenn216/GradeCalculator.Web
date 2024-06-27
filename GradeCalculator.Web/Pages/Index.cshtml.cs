using System.Data;
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
            Level = GetLevelName(Convert.ToInt32(Request.Form["inputYearLevel"])) 
        };
        return RedirectToPage("GradeCalculatorForm", studentInformationModel);
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
}