using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _24IN_Ultimate_KHO_KHO_VS.Data;
using System.Data;
using Microsoft.Data.SqlClient;

public class dataEntryModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<dataEntryModel> _logger;

    public dataEntryModel(ApplicationDbContext context, ILogger<dataEntryModel> logger)
    {
        _context = context; 
        _logger = logger;
    }

    public List<GetMatchStats_Scoring> GetMatchStatsScoringList {get;set;}
    public List<PlayerMaster> PlayerMasterList{get;set;}
    public List<dataEnter_Scoring> dataEnterScoringAwayList {get;set;}
    public List<dataEnter_Scoring> dataEnterScoringHomeList {get;set;}


    public async Task OnGetAsync(string tournamentId, string matchNo)
    {
       
        GetMatchStatsScoringList =await _context.GetMatchStatsScoring
            .FromSqlInterpolated($"EXEC GetMatchStats_Scoring @MatchNo = {matchNo}, @TournamentId = {tournamentId}")
            .ToListAsync();

        if (GetMatchStatsScoringList == null || !GetMatchStatsScoringList.Any())
        {
            _logger.LogWarning($"No player data found for MatchNo {matchNo} and TournamentId {tournamentId}");
        }


        dataEnterScoringAwayList = await _context.dataEnterScoring
            .FromSqlInterpolated($"EXEC dataEnter_Scoring @MatchNo = {matchNo}, @TournamentId = {tournamentId}")
            .AsNoTracking()    ///this is must for getting all data from SP
            .ToListAsync();

        dataEnterScoringAwayList = dataEnterScoringAwayList
                .Where(item => item.team_category == "Home")
                .ToList();




        if (!dataEnterScoringAwayList.Any())
        {
            Console.WriteLine("No data returned from the stored procedure.");
        }
        else
        {
            foreach (var item in dataEnterScoringAwayList)
            {
                Console.WriteLine($"PlayerName: {item.playername}");
            }
        }

            dataEnterScoringHomeList = await _context.dataEnterScoring
            .FromSqlInterpolated($"EXEC dataEnter_Scoring @MatchNo = {matchNo}, @TournamentId = {tournamentId}")
            .AsNoTracking()    ///this is must for getting all data from SP
            .ToListAsync();

            dataEnterScoringHomeList = dataEnterScoringHomeList
                .Where(item => item.team_category == "Away")
                .ToList();

                
        if (!dataEnterScoringHomeList.Any())
        {
            Console.WriteLine("No data returned from the stored procedure.");
        }
        else
        {
            foreach (var item in dataEnterScoringHomeList)
            {
                Console.WriteLine($"PlayerName: {item.playername}");
            }
        }


    }


}
