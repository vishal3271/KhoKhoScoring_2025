using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _24IN_Ultimate_KHO_KHO_VS.Data;
using Newtonsoft.Json;

    
    public class ScoringController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScoringController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Scoring(string tournamentId, string matchNo, string selectedTeam, string choice)
        {
            if (string.IsNullOrEmpty(tournamentId) || string.IsNullOrEmpty(matchNo))
            {
                return BadRequest("Tournament ID and Match No are required.");
            }

            ViewBag.TournamentId = tournamentId;
            ViewBag.MatchNo = matchNo;
            ViewBag.SelectedTeam = selectedTeam;
            ViewBag.Choice = choice;

            var matchData = _context.MatchMaster
                .FirstOrDefault(m => m.idTournament == tournamentId && m.MatchNo == matchNo);

            if (matchData == null)
            {
                return NotFound("No match data found for the given Tournament and Match.");
            }

            return View("Scoring");
        }

        public async Task<IActionResult> GetPlayerStats(string matchNo, string tournamentId)
        {
            // Call the stored procedure to get player stats
            var playerStats = await _context.GetMatchStatsScoring
                .FromSqlInterpolated($"EXEC GetMatchStats_Scoring @MatchNo = {matchNo}, @TournamentId = {tournamentId}")
                .ToListAsync();

            if (!playerStats.Any())
            {
                return NotFound(new { message = "No data found for the given MatchNo" });
            }

            return Json(playerStats);
        }


}