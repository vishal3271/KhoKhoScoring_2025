using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _24IN_Ultimate_KHO_KHO_VS.Data;

    
    public class IndexController : Controller
    {
         public IActionResult Index(string tournamentId, string matchNo)
        {
            ViewBag.TournamentId = tournamentId;
            ViewBag.MatchNo = matchNo;
            return View();
        }
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;


        public IndexController(ApplicationDbContext context,ILogger<IndexModel> logger)
        {
            _context = context;
            _logger=logger;
        }

        [HttpGet("GetMatches")]
        public async Task<IActionResult> GetMatches(string tournamentId)
        {
            if (string.IsNullOrEmpty(tournamentId))
            {
                return BadRequest("Tournament ID is required.");
            }

             _logger.LogInformation($"Get Matches TournamentId: {tournamentId}");

                var matches = await _context.MatchMaster
                .Where(m => m.idTournament == tournamentId)
                .Select(m => new { id = m.MatchNo, name = m.Match_Name})
                .ToListAsync();

            if (matches == null || !matches.Any())
            {
                return Json(new { message = "No matches found" });
            }

            var result = matches.Select(m => new { id = m.id, name = m.name }).ToList();

            return Json(result);
        }

        [HttpGet("GetTossData")]
        public async Task<IActionResult> GetTossData(string tournamentId, string matchNo)
        {
            if (string.IsNullOrEmpty(tournamentId) || string.IsNullOrEmpty(matchNo))
            {
                return BadRequest("Tournament ID and Match No are required.");
            }

            _logger.LogInformation($"Fetching toss data for TournamentId: {tournamentId}, MatchNo: {matchNo}");

            var tossData = await _context.GetMatchNameTossDataScoring
                .FromSqlRaw("EXEC GetMatchNameTossData_Scoring @TournamentId = {0}, @MatchNo = {1}", tournamentId, matchNo)
                .ToListAsync();

            if (tossData == null || !tossData.Any())
            {
                return Json(new { message = "No toss data found" });
            }

            var result = tossData.Select(t => new { id = t.idTournament, winner = t.HomeTeam, loosing = t.AwayTeam }).ToList();
            Console.WriteLine(result);
            return Json(result);
        }

    }
