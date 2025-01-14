using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _24IN_Ultimate_KHO_KHO_VS.Data;
using Microsoft.Extensions.Logging;  
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ScoringModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ScoringModel> _logger;

    public ScoringModel(ApplicationDbContext context, ILogger<ScoringModel> logger)
    {
        _context = context;  
        _logger = logger;
    }

    public List<HowOut> HowOutList { get; set; }
    public List<dataEnterPlayerDetails_Scoring> dataEnterPlayerDetailsScoringList {get;set;}
    public List<dataEnterPlayerDetails_Scoring> homeInplay {get;set;}
    public List<dataEnterPlayerDetails_Scoring> awayInplay {get;set;}
     public List<dataEnterPlayerDetails_Scoring> homeInbench {get;set;}
    public List<dataEnterPlayerDetails_Scoring> awayInbench {get;set;}
    public List<Toss> tosses {get;set;}
    public List<Inning_Batch_Scoring> InningBatchScoringsList {get;set;}
    public List<Inning_Batch_Scoring> attackingTeamPoints {get;set;}
    public List<MatchScore> matchScore {get;set;}
    public List<timedata> timedatas {get;set;} 
    public int IsAttacking{get;set;}

public async Task OnGetAsync(string tournamentId, string matchId, int isAttacking, int tossWinnerId, string hometeamId, string awayteamId, int? batchNo)
{
    try
    {
        ViewData["MatchId"] = matchId;
        ViewData["IsAttacking"] = isAttacking;
        ViewData["TossWinnerId"] = tossWinnerId;
        ViewData["HomeTeamId"] = hometeamId;
        ViewData["AwayTeamId"] = awayteamId;

        var dataEnter = await _context.dataEnterPlayerDetailsScoring
            .FromSqlInterpolated($"select * from dataEnterPlayerDetails_Scoring")
            .ToListAsync();

        var InningBatchScorings =await _context.InningBatchScoring
        .FromSqlInterpolated($"select * from Inning_Batch_scoring")
        .ToListAsync();

        var homeTeamAttacking = tossWinnerId.ToString() == hometeamId ? isAttacking : 0;
        var awayTeamAttacking = tossWinnerId.ToString() == awayteamId ? isAttacking : 0;

        bool isHomeTeamAttacking = homeTeamAttacking == 1;

        if (isHomeTeamAttacking)
        {
            homeInplay = dataEnter
                .Where(p => p.team_category == "Home" && p.idTournament == tournamentId && p.idMatch == matchId && p.playerstatus.Trim() == "In Play" ) 
                .ToList();
            homeInbench = dataEnter
                .Where(p => p.team_category == "Home" && p.idTournament == tournamentId && p.idMatch == matchId && p.playerstatus.Trim() == "In Bench"   )
                .ToList();
            awayInplay = dataEnter
                .Where(p => p.team_category == "Away" && p.idTournament == tournamentId && p.idMatch == matchId && p.playerstatus.Trim() == "In Play"   )
                .ToList();
            awayInbench = dataEnter
                .Where(p => p.team_category == "Away" && p.idTournament == tournamentId && p.idMatch == matchId && p.playerstatus.Trim() == "In Bench" )
                .ToList();
            
        }
        else
        {
            awayInplay = dataEnter
                .Where(p => p.team_category == "Away" && p.idTournament == tournamentId && p.idMatch == matchId && p.playerstatus.Trim() == "In Play"  )
                .ToList();
            awayInbench = dataEnter
                .Where(p => p.team_category == "Away" && p.idTournament == tournamentId && p.idMatch == matchId && p.playerstatus.Trim() == "In Bench" )
                .ToList();
            homeInplay = dataEnter
                .Where(p => p.team_category == "Home" && p.idTournament == tournamentId && p.idMatch == matchId && p.playerstatus.Trim() == "In Play"  )
                .ToList();
            homeInbench = dataEnter
                .Where(p => p.team_category == "Home" && p.idTournament == tournamentId && p.idMatch == matchId && p.playerstatus.Trim() == "In Bench"  )
                .ToList();
        }

        if (batchNo!= null)
        {
            homeInplay = homeInplay.Where(p => p.batchno == batchNo).ToList();
            awayInplay = awayInplay.Where(p => p.batchno == batchNo ).ToList();
        }

        var matchScore = await _context.matchScore
            .Where(ms => ms.matchId == matchId)
            .FirstOrDefaultAsync();

        if (matchScore != null)
        {
            var score = matchScore.gamescore;  
            int homeScore = 0, awayScore = 0;

            if (!string.IsNullOrEmpty(score))
            {
                var scores = score.Split(" - ");
                if (scores.Length == 2)
                {
                    bool homeParsed = int.TryParse(scores[0], out homeScore);
                    bool awayParsed = int.TryParse(scores[1], out awayScore);

                    if (!homeParsed || !awayParsed)
                    {
                        ViewData["ErrorMessage"] = "Failed to parse home or away score.";
                    }
                }
                else
                {
                    ViewData["ErrorMessage"] = "Invalid score format. Expected format: 'home - away'.";
                }
            }

            ViewData["HomeScore"] = homeScore;
            ViewData["AwayScore"] = awayScore;
        }
        else
        {
            ViewData["ErrorMessage"] = "No match score found for the provided match ID.";
        }

            var timedataQuery = @"
          SELECT * 
FROM timedata
WHERE id >= (SELECT MAX(id) - 4 FROM timedata)
ORDER BY id ASC;";
        
        var timedatas = await _context.timedata.FromSqlRaw(timedataQuery).ToListAsync();
         _logger.LogInformation("Fetched timedatas: {@Timedatas}", timedatas);


        _logger.LogInformation("Fetched timedatas: {@Timedatas}", timedatas);
        this.timedatas = timedatas;



    }
    catch (Exception ex)
    {
        _logger.LogError($"Error fetching data: {ex.Message}");
    }
}


}










