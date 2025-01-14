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

    public List<dataEnter_Scoring> dataEnterScoringAwayList {get;set;}
    public List<dataEnter_Scoring> dataEnterScoringHomeList {get;set;}
    public List<dataEnter_Scoring> dataEnterScoringList {get;set;} 
    public List<dataEnterPlayerDetails_Scoring> dataEnterPlayerDetailsScoringsList{get;set;}
    public List<dataEnterPlayerDetails_Scoring> dataEnterPlayerDetailsScoringsHomeList{get;set;}
    public List<dataEnterPlayerDetails_Scoring> dataEnterPlayerDetailsScoringsAwayList{get;set;}

    [BindProperty]
    public string MatchId{get;set;}
public async Task OnGetAsync(string tournamentId, string MatchNo, int isAttacking, int tossWinnerId, string matchId)
{
    try
    {
        ViewData["IsAttacking"] = isAttacking;
        ViewData["TossWinnerId"] = tossWinnerId;
        ViewData["MatchNo"]=MatchNo;

// _logger.LogInformation($"Fetching data in ON GET for MatchNo: {MatchNo}, TournamentId: {tournamentId}, matchid : {matchId}");

        var playerData = await _context.dataEnterPlayerDetailsScoring
            .Where(p => p.idMatch == matchId && p.idTournament == tournamentId)    //&& p.batchno != 0
            .ToListAsync();

    // _logger.LogInformation($"Fetched {playerData.Count} records from dataEnterPlayerDetailsScoring for MatchId: {MatchId} and TournamentId: {tournamentId}");
    _logger.LogInformation($"Executed SQL query: {playerData}");


        if (playerData.Any())
        {
_logger.LogInformation($"Fetched {playerData.Count} player records from dataEnterPlayerDetailsScoring.");
        _logger.LogInformation($"Fetched {playerData.Count} records from dataEnterPlayerDetailsScoring for MatchId: {matchId} and TournamentId: {tournamentId}. Data: {Newtonsoft.Json.JsonConvert.SerializeObject(playerData)}");



            dataEnterScoringHomeList = playerData
                .Where(p => p.team_category == "Home")
                .Select(p => new dataEnter_Scoring
                {
                    idMatch = p.idMatch,
                    playername = p.playername, 
                    teamName = p.teamName,  
                    playerid = p.playerid, 
                    teamId = p.teamId,    
                    team_category=p.team_category,          
                    idTournament=p.idTournament,
                    playerstatus=p.playerstatus,
                    batchno=p.batchno,
                    isWazir=p.iswazir
                     })
                .ToList();

            dataEnterScoringAwayList = playerData
                .Where(p => p.team_category == "Away")
                .Select(p => new dataEnter_Scoring
                {
                    idMatch = p.idMatch,
                    playername = p.playername, 
                    teamName = p.teamName,  
                    playerid = p.playerid, 
                    teamId = p.teamId,
                    team_category=p.team_category,
                    idTournament=p.idTournament,
                    playerstatus=p.playerstatus,
                    batchno=p.batchno,
                    isWazir=p.iswazir  
                })
                .ToList();
        }
        else
        {
            

            var allScoringData = await _context.dataEnterScoring
                .FromSqlInterpolated($"EXEC dataEnter_Scoring @MatchNo = {MatchNo}, @TournamentId = {tournamentId}")
                .AsNoTracking()
                .ToListAsync();

            dataEnterScoringHomeList = allScoringData.Where(p => p.team_category == "Home").ToList();
            dataEnterScoringAwayList = allScoringData.Where(p => p.team_category == "Away").ToList();
_logger.LogInformation($"Fetched {allScoringData.Count} player records from dataEnter.");
            _logger.LogInformation($"Fetched {allScoringData.Count} records from dataEnterScoring for MatchNo: {MatchNo} and TournamentId: {tournamentId}. Data: {Newtonsoft.Json.JsonConvert.SerializeObject(allScoringData)}");


        }
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error fetching data: {ex.Message}");
    }
}

}
