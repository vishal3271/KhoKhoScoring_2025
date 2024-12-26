using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _24IN_Ultimate_KHO_KHO_VS.Data;

    
    public class dataEnterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<dataEntryModel> _logger;

        // public async Task<IActionResult> dataEnter(string tournamentId, string matchNo, string matchName)
        // {
        //     _logger.LogInformation($"Scoring page loaded with TournamentId: {tournamentId}, MatchNo: {matchNo}, MatchName: {matchName}");

        //     var playerStats = await _context.GetMatchStatsScoring
        //         .FromSqlInterpolated($"EXEC GetMatchStats_Scoring @MatchNo = {matchNo}, @TournamentId = {tournamentId}")
        //         .ToListAsync();

        //     ViewData["TournamentId"] = tournamentId;
        //     ViewData["MatchNo"] = matchNo;
        //     ViewData["MatchName"] = matchName;
        //     ViewData["PlayerStats"] = playerStats;

        //     return View(); 
        // }

    }
