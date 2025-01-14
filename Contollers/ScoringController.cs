using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _24IN_Ultimate_KHO_KHO_VS.Data;
using Newtonsoft.Json;

    public class ScoringController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ScoringModel> _logger;


        public ScoringController(ApplicationDbContext context, ILogger<ScoringModel> logger)
        {
            _context = context;
            _logger=logger;
        }
        public List<Inning_Batch_Scoring> InningBatchScoringsList {get;set;}
        public List<MatchScore> matchScoresList {get;set;}
        public List<timedata> timedatas {get;set;}
        public List<TeamPlayer_CurrentMatch> teamPlayerCurrentMatches {get;set;}
        public List<Inning_Batch> InningBatch {get;set;}


// [HttpGet]
// public async Task<IActionResult> FilterPlayersByBatch(int batchNo, string tournamentId, string MatchId, int isAttacking, int isHomeAttacking, int isAwayAttacking)
// {
//     try
//     {
//         _logger.LogInformation($"Received parameters in batch dropdown- batchNo: {batchNo}, isAttacking: {isAttacking}, tournamentId: {tournamentId}, matchId: {MatchId}, isHomeAttacking: {isHomeAttacking}, isAwayAttacking: {isAwayAttacking}");

//         var filteredPlayers = await (from player in _context.dataEnterPlayerDetailsScoring
//                                       join time in _context.timedata // Joining timedata table
//                                       on new { player.idTournament, player.idMatch, player.teamId } 
//                                       equals new { time.tourid, time.matchId, time.hometeam, time.awayteam } // Join condition on tourid, matchId, hometeam, awayteam
//                                       where player.batchno == batchNo
//                                             && player.idTournament == tournamentId
//                                             && player.idMatch == MatchId
//                                             && player.playerstatus.Trim() == "In Play"
//                                             && (
//                                                 (player.team_category == "home" && player.teamId == player.hometeam) 
//                                                 || (player.team_category == "away" && player.teamId == player.awayteam)
//                                             )
//                                             && ((isHomeAttacking == 0 && player.IsAttacking == 0)
//                                                  || (isAwayAttacking == 0 && player.IsAttacking == 0))
//                                       select new 
//                                       {
//                                           player.playerid,
//                                           player.playername,
//                                           player.IsAttacking,
//                                           player.batchno,
//                                           player.idTournament,
//                                           player.idMatch,
//                                           player.playerstatus,
//                                           time.TurnTimer // Select TurnTimer from timedata
//                                       })
//                                       .ToListAsync();

//         _logger.LogInformation($"Filtered players: {filteredPlayers.Count}");

//         return Json(new { players = filteredPlayers });
//     }
//     catch (Exception ex)
//     {
//         _logger.LogError($"Error fetching players by batch: {ex.Message}");
//         return StatusCode(500, "Internal server error");
//     }
// }




[HttpGet]
public async Task<IActionResult> FilterPlayersByBatch(int batchNo, string tournamentId, string MatchId, int isAttacking, int isHomeAttacking, int isAwayAttacking)
{
    try
    {
        _logger.LogInformation($"Received parameters in batch dropdown- batchNo: {batchNo}, isAttacking: {isAttacking}, tournamentId: {tournamentId}, matchId: {MatchId},isHomeAttacking: {isHomeAttacking}, isAwayAttacking: {isAwayAttacking}");
        var filteredPlayers = await _context.dataEnterPlayerDetailsScoring
            .Where(p => p.batchno == batchNo 
                        && p.idTournament == tournamentId 
                        && p.idMatch == MatchId 
                        && p.playerstatus.Trim() == "In Play"
                        &&(
                         (isHomeAttacking == 0 && p.IsAttacking == 0) 
                            || (isAwayAttacking == 0 && p.IsAttacking == 0) 
                        )
                        )
            .ToListAsync();

                    _logger.LogInformation($"Filtered players: {filteredPlayers.Count}");


        return Json(new { players = filteredPlayers });
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error fetching players by batch: {ex.Message}");
        return StatusCode(500, "Internal server error");
    }
}

[HttpPost]
public async Task<IActionResult> AddStats([FromBody] InningBatchScoringRequest request)
{
    try
    {
    _logger.LogInformation("AttackingPlayerId: {AttackingPlayerId}, DefendingPlayerId: {DefendingPlayerId}, Inning: {Inning}, Turn: {Turn}, AttackingTeamId: {AttackingTeamId}, DefendingTeamId: {DefendingTeamId}, OutTypeId: {OutTypeId}, AppMatchId: {AppMatchId}",
    request?.AttackingPlayerId, request?.DefendingPlayerId, request?.Inning, request?.Turn, request?.AttackingTeamId, request?.DefendingTeamId, request?.OutTypeId, request?.AppMatchId);



        int attackingPoints = 0;
        int defendingPoints = 0;

        if (request?.AttackingPlayerId > 0 && request?.DefendingPlayerId > 0)
        {
            attackingPoints = 2;
            defendingPoints = 0;
        }
        else if (request?.DefendingPlayerId > 0)
        {
            attackingPoints = 0;
            defendingPoints = 1;
        }

        var inningSeconds = int.TryParse(request?.InningTimer, out var parsedInningTimer) ? parsedInningTimer : 0;
        var turnSeconds = int.TryParse(request?.TurnTimer, out var parsedTurnTimer) ? parsedTurnTimer : 0;

       var newEntry = new Inning_Batch_Scoring
        {
        attacker_id = request?.AttackingPlayerId ?? 0,
        defender_id = request?.DefendingPlayerId ?? 0,
        inning_number = request.Inning > 0 ? request.Inning : 0,
        Turn = request.Turn > 0 ? request.Turn : 0,
        attackingPoints = attackingPoints,
        defendingPoints=defendingPoints,
        attacking_team_id = request.AttackingTeamId > 0 ? request.AttackingTeamId : 0,
        defending_team_id  = request.DefendingTeamId > 0 ? request.DefendingTeamId : 0,
        out_type_id = request.OutTypeId > 0 ? request.OutTypeId : 0, 
        App_MatchId = string.IsNullOrEmpty(request.AppMatchId) ? "0" : request.AppMatchId,
        InningTimer = ConvertSecondsToTimeFormat(inningSeconds),
        TurnTimer = ConvertSecondsToTimeFormat(turnSeconds),
        };
        _logger.LogInformation("Created new Inning_Batch_Scoring entry: {@NewEntry}", newEntry);

        await _context.InningBatchScoring.AddAsync(newEntry);
        await _context.SaveChangesAsync();


        return Json(new { success = true });
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error adding stats: {ex.Message}");
        return Json(new { success = false, message = "An error occurred while adding stats." });
    }
}

public static string ConvertSecondsToTimeFormat(int seconds)
{
    int minutes = seconds / 60;
    int remainingSeconds = seconds % 60;
    return $"{minutes:D2}:{remainingSeconds:D2}";
}
    public class InningBatchScoringRequest
    {
        public int AttackingPlayerId { get; set; }
        public int DefendingPlayerId { get; set; }
        public int Inning { get; set; }
        public int Turn { get; set; }
        public int AttackingTeamId {get;set;}
        public int DefendingTeamId{get;set;}
        public int OutTypeId {get;set;}
        public string AppMatchId {get;set;}
        public string InningTimer {get;set;}
        public string TurnTimer {get;set;}
    }


[HttpPost]
public async Task<IActionResult> AddScore([FromBody] MatchScoreUpdateModel model)  
{
     if (model == null)
    {
        return BadRequest("Request body is null or invalid.");
    }
    _logger.LogInformation("Match ID : ${Match ID}", model.matchId );

    if (string.IsNullOrEmpty(model.matchId))
    {
        return BadRequest("Match ID is required.");
    }

    var matchScore = _context.matchScore.FirstOrDefault(m => m.matchId == model.matchId);

    if (matchScore == null)
    {
        matchScore = new MatchScore
        {
            matchId = model.matchId,
            MatchScoreid = model.MatchScoreid,
            tourid = model.tourid,
            tieid = model.tieid,
            hometeam = model.hometeam,
            awayteam = model.awayteam,
            winningteam = model.winningteam,
            ishometeamtrump = model.ishometeamtrump ?? 0,
            isawayteamtrump = model.isawayteamtrump ?? 0,
            gamescore = "0 - 0" 
        };

        _context.matchScore.Add(matchScore);
        await _context.SaveChangesAsync(); 
        return Json(new { success = true, message = "New match added.", gamescore = matchScore.gamescore });
    }

    int homeScore = 0;
    int awayScore = 0;

    if (!string.IsNullOrEmpty(matchScore.gamescore))
    {
        var scores = matchScore.gamescore.Split('-');
        homeScore = int.Parse(scores[0].Trim());
        awayScore = int.Parse(scores[1].Trim());
    }

    if (model.Team == "home")
    {
        homeScore += model.ScoreChange;
    }
    else if (model.Team == "away")
    {
        awayScore += model.ScoreChange;
    }

    homeScore = Math.Max(homeScore, 0);
    awayScore = Math.Max(awayScore, 0);

    matchScore.gamescore = $"{homeScore} - {awayScore}";

    matchScore.MatchScoreid = model.MatchScoreid ?? matchScore.MatchScoreid;
    matchScore.tourid = model.tourid ?? matchScore.tourid;
    matchScore.tieid = model.tieid ?? matchScore.tieid;
    matchScore.hometeam = model.hometeam ?? matchScore.hometeam;
    matchScore.awayteam = model.awayteam ?? matchScore.awayteam;
    matchScore.winningteam = model.winningteam ?? matchScore.winningteam;
    matchScore.ishometeamtrump = model.ishometeamtrump ?? matchScore.ishometeamtrump;
    matchScore.isawayteamtrump = model.isawayteamtrump ?? matchScore.isawayteamtrump;


    var timeDataEntry = new timedata
    {
        tourid = model.tourid,
        matchId = model.matchId,
        hometeam = model.hometeam,
        awayteam = model.awayteam,
        InningTimer = model.InningTimer, 
        TurnTimer = model.TurnTimer,
    };

    _context.timedata.Add(timeDataEntry);

    await _context.SaveChangesAsync(); 

     var timedataQuery = @"
        SELECT * 
FROM timedata
WHERE id >= (SELECT MAX(id) - 2 FROM timedata)
ORDER BY id ASC;";

    var timedatas = await _context.timedata.FromSqlRaw(timedataQuery).ToListAsync();
    

    return Json(new { success = true, message = "Score updated.", gamescore = matchScore.gamescore, timedatas = timedatas });
}

public class MatchScoreUpdateModel
{
    public string MatchScoreid { get; set; }
    public string tourid { get; set; }
    public string tieid { get; set; }
    public string matchId { get; set; }
    public string Team { get; set; }
    public int ScoreChange { get; set; }
    public string hometeam { get; set; }
    public string awayteam { get; set; }
    public string winningteam { get; set; }
    public long? ishometeamtrump { get; set; } 
    public long? isawayteamtrump { get; set; }
    public int InningTimer {get;set;}
    public int TurnTimer {get;set;}
}

[HttpGet]
public async Task<IActionResult> GetTimedatas(string tournamentId, string matchId)
{
    try
    {
        var timedataQuery = @"
           SELECT * 
FROM timedata
WHERE id >= (SELECT MAX(id) - 2 FROM timedata)
ORDER BY id ASC;";
        
        // Fetch the timedata
        var timedatas = await _context.timedata.FromSqlRaw(timedataQuery).ToListAsync();
        _logger.LogInformation("Fetched timedatas: {@Timedatas}", timedatas);

        return Json(new { timedatas });
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error fetching timedatas: {ex.Message}");
        return Json(new { success = false, message = "Error fetching timedatas." });
    }
}


[HttpPost]
public async Task<IActionResult> SavePlayerData([FromBody] PlayerData playerData)
{
    if (ModelState.IsValid)
    {
        Console.WriteLine("Processing player stats...");

        // Check if matchId and batchNo (tourId) already exist
        var existingPlayer = await _context.TeamPlayerCurrentMatch
            .FirstOrDefaultAsync(p => p.AppMatchId == playerData.matchId && p.BatchNo == playerData.batchNo);

        await AddInningBatchRecord(playerData);

        if (existingPlayer != null)
        {
            // If record exists, update it
            Console.WriteLine("Found existing player stats. Updating...");

            existingPlayer.PlayerId = playerData.playerId;
            existingPlayer.TimeSpend = playerData.time;
            existingPlayer.PoleDive = playerData.Stats == 2 ? 1 : 0;
            existingPlayer.SelfOut = playerData.Stats == 6 ? 1 : 0;
            existingPlayer.skydive = playerData.Stats == 5 ? 1 : 0;

            try
            {
                await _context.SaveChangesAsync();  // Asynchronous save
                Console.WriteLine("Player stats updated successfully.");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating player stats: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }
        else
        {
            // If record does not exist, create a new one
            Console.WriteLine("No existing record found. Inserting new player stats...");

            var newPlayer = new TeamPlayer_CurrentMatch
            {
                AppMatchId = playerData.matchId,
                BatchNo = playerData.batchNo,
                PlayerId = playerData.playerId,
                TimeSpend = playerData.time,
                PoleDive = playerData.Stats == 2 ? 1 : 0,
                SelfOut = playerData.Stats == 6 ? 1 : 0,
                skydive = playerData.Stats == 5 ? 1 : 0,
            };

            try
            {
                _context.TeamPlayerCurrentMatch.Add(newPlayer);
                await _context.SaveChangesAsync();  // Asynchronous save
                Console.WriteLine("Player stats saved successfully.");
                return Json(new { success = true });


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving player stats: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

    }

    Console.WriteLine("Model state is not valid.");
    return Json(new { success = false });
}

public class PlayerData
{
    public int time { get; set; }
    public int Stats { get; set; }
    public int playerId { get; set; }
    public string matchId { get; set; }
    public int batchNo { get; set; }
    public int skydive {get;set;}
    public int attacker {get;set;}
    public int defender {get;set;}
    public int Inning {get;set;}
    public int Turn {get;set;}
    public int attackingTeamId{get;set;}
    public int defendingTeamId{get;set;}

}

private async Task AddInningBatchRecord(PlayerData playerData)
{
    var newInningBatch = new Inning_Batch
    {
        SeriesId = 1,
        MatchId = 0,
        InningNumber = playerData.Inning,
        RunNumber = 0, 
        AttackingTeamId = playerData.attackingTeamId,
        DefendingTeamId = playerData.defendingTeamId,
        BatchNumber = playerData.batchNo,
        AttemptNumber = 1,
        PlayerId = playerData.playerId,
        TimeOnCourtBatch = playerData.time,
        ClockIn = "0",
        ClockOut = "0",
        TimeOnCourtPlayer = playerData.time,
        IsOut = playerData.Stats == 2 || playerData.Stats == 6 || playerData.Stats == 5, // ? 1 : 0,
        IsBatchActive = true, 
        IsPowerPlay = false,         
        AppMatchId = playerData.matchId,
        AttackerId = playerData.attacker,
        OutTypeId = playerData.Stats, 
        GfxBatchNo =playerData.batchNo,
    };

    try
    {
        _context.InningBatch.Add(newInningBatch);
        await _context.SaveChangesAsync();
        Console.WriteLine("Inning batch data saved successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error saving inning batch data: " + ex.Message);
        throw; 
    }
}


[HttpPost]
public async Task<IActionResult> UndoLastInningBatch()
{
    try
    {
        var latestBatch = await _context.InningBatch
            .OrderByDescending(b => b.Id) 
            .FirstOrDefaultAsync();

        if (latestBatch == null)
        {
            return Json(new { success = false, message = "No records found to undo." });
        }

        _context.InningBatch.Remove(latestBatch);
        await _context.SaveChangesAsync();

        Console.WriteLine("Latest inning batch record deleted successfully.");
        return Json(new { success = true, message = "Last operation undone." });
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error during undo operation: " + ex.Message);
        return Json(new { success = false, message = "Failed to undo operation.", error = ex.Message });
    }
}




}