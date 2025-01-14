using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _24IN_Ultimate_KHO_KHO_VS.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class dataEnterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<dataEntryModel> _logger;

        public dataEnterController(ApplicationDbContext context,ILogger<dataEntryModel> logger)
        {
            _context = context;
            _logger=logger;
        }

[BindProperty]
public Toss TossData { get; set; }

[HttpPost]
public async Task<IActionResult> SaveToss(int teamSelection, int playerStatSelection,string TournamentId, string MatchNo,string matchId)
{
    try
    {
    _logger.LogInformation("SaveToss method invoked. Parameters: teamSelection={TeamSelection}, playerStatSelection={PlayerStatSelection}, TournamentId={TournamentId}, MatchNo={MatchNo}, MatchId={matchId}", teamSelection, playerStatSelection, TournamentId, MatchNo, matchId);

        if (teamSelection == null)
            return BadRequest("Team selection is required.");
        if (playerStatSelection == null)
            return BadRequest("Player stat selection is required.");
        if (string.IsNullOrEmpty(TournamentId))
            return BadRequest("Tournament ID is required.");
        if (string.IsNullOrEmpty(MatchNo))
            return BadRequest("Match number is required.");
        if (string.IsNullOrEmpty(matchId))
            return BadRequest("Match ID is required.");



        TossData = new Toss()
        {
            TossWinnerId = teamSelection,
            IsAttacking = playerStatSelection == 1 ? 1 : 0,
            idTournament = TournamentId,
            App_MatchId = matchId
        };

        _logger.LogInformation("Initialized TossData object: {@TossData}", TossData);

        var existingToss = await _context.Toss
        .FirstOrDefaultAsync(t => t.App_MatchId == matchId);

        if (existingToss != null)
        {
        _logger.LogInformation("Existing toss data found: {@ExistingToss}", existingToss);

            existingToss.TossWinnerId = TossData.TossWinnerId;
            existingToss.IsAttacking = TossData.IsAttacking;
            existingToss.idTournament = TossData.idTournament;

            _context.Toss.Update(existingToss);  
            _logger.LogInformation("Existing toss data updated.");
        }
        else
        {
            _context.Toss.Add(TossData);
            _logger.LogInformation("New toss data added.");
        }

        await _context.SaveChangesAsync();
      
        _logger.LogInformation("SaveToss - Toss data saved successfully.");
        return RedirectToPage("/dataEnter", new { tournamentId = TournamentId, MatchNo, matchId, tossWinnerId = TossData.TossWinnerId, isAttacking = TossData.IsAttacking }); 
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error occurred while saving toss data.");
        return StatusCode(500, "Internal server error.");
    }
}

    [BindProperty]
    public dataEnterPlayerDetails_Scoring  playerData {get;set;}
    [BindProperty]
    public string MatchId{get;set;}
    [HttpPost]
    public async Task<IActionResult> SavePlayerDetail(string TournamentId, int tossWinnerId, string MatchId, int isAttacking, string hometeamname, string hometeamId,string awayteamId, List<string> homeplayerid, List<string> awayplayerid, List<string> homePlayerName, List<int> homeBatchNo, List<string> homePlayerStatus, string awayteamname, List<string> awayplayername, List<int> awayBatchNo, List<string> awayPlayerStatus, List<bool> iswazir)
    {
        try
    {
    int homeTeamIsAttacking = (tossWinnerId.ToString().Trim() == hometeamId.Trim()) ? isAttacking : 0;
    int awayTeamIsAttacking = (tossWinnerId.ToString().Trim() == awayteamId.Trim()) ? isAttacking : 0;

        var playersData = new List<dataEnterPlayerDetails_Scoring>();

        var allPlayers = homePlayerName
            .Select((name, i) => new
            {
                PlayerName = name.Trim(),
                BatchNo = homeBatchNo[i],
                //.Trim(),
                PlayerStatus = homePlayerStatus[i].Trim(),
                TeamCategory = "Home",
                TeamId = hometeamId,
                PlayerId = homeplayerid[i],
                IsAttacking = homeTeamIsAttacking,
                TeamName = hometeamname,
                iswazir=iswazir[i]
                
            })
            .Concat(awayplayername.Select((name, i) => new
            {
                PlayerName = name.Trim(),
                BatchNo = awayBatchNo[i],
                //.Trim(),
                PlayerStatus = awayPlayerStatus[i].Trim(),
                TeamCategory = "Away",
                TeamId = awayteamId,
                PlayerId = awayplayerid[i],
                IsAttacking = awayTeamIsAttacking,
                TeamName = awayteamname,
                iswazir=iswazir[i]
            }));

        var existingPlayers = await _context.dataEnterPlayerDetailsScoring
        .Where(p => p.idMatch == MatchId && p.idTournament == TournamentId && homeplayerid.Contains(p.playerid) || awayplayerid.Contains(p.playerid))
        .ToListAsync();

              
    foreach (var playerData in allPlayers)
    {
        var existingPlayer = existingPlayers?.FirstOrDefault(p => p.playerid == playerData.PlayerId && p.idMatch == MatchId && p.idTournament == TournamentId);

        if (existingPlayer != null)
        {
            existingPlayer.playername = playerData.PlayerName;
            existingPlayer.batchno = playerData.BatchNo;
            existingPlayer.playerstatus = playerData.PlayerStatus;
           if (isAttacking != 0) existingPlayer.IsAttacking = playerData.IsAttacking;
           existingPlayer.iswazir=playerData.iswazir;

            _context.dataEnterPlayerDetailsScoring.Update(existingPlayer);
        }
        else
        {
            playersData.Add(new dataEnterPlayerDetails_Scoring
            {
                idTournament = TournamentId,
                teamName = playerData.TeamName,
                playername = playerData.PlayerName,
                batchno = playerData.BatchNo,
                playerstatus = playerData.PlayerStatus,
                team_category = playerData.TeamCategory,
                idMatch = MatchId,
                teamId = playerData.TeamId,
                playerid = playerData.PlayerId,
                IsAttacking = playerData.IsAttacking,
                iswazir = playerData.iswazir
            });
        }
    }

        if (playersData.Any())
        {
            await _context.dataEnterPlayerDetailsScoring.AddRangeAsync(playersData);
        }
        await _context.SaveChangesAsync();

        return RedirectToPage("/Scoring", new
        {
            tournamentId = TournamentId,
            MatchId,
            isAttacking,
            tossWinnerId,
            hometeamId,
            awayteamId,
            iswazir
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error occurred while saving player data.");
        return StatusCode(500, "Internal server error.");
    }
    }

}
