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

               var matches = await _context.MatchMaster
                .Where(m => m.idTournament == tournamentId)
                .OrderByDescending(m => Convert.ToInt32(m.MatchNo))
                .Select(m => new { id = m.MatchNo, name = m.Match_Name,matchid=m.idMatch })
                .ToListAsync();

            if (matches == null || !matches.Any())
            {
                return NotFound("No matches found.");
            }
            return Ok(matches);
        }

    }
