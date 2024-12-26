using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _24IN_Ultimate_KHO_KHO_VS.Data;
using Microsoft.Extensions.Logging;  
using Newtonsoft.Json;

public class ScoringModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ScoringModel> _logger;

    public ScoringModel(ApplicationDbContext context, ILogger<ScoringModel> logger)
    {
        _context = context;  
        _logger = logger;
    }

    public List<StatMaster> StatMasterList { get; set; }
    public List<GetMatchStats_Scoring> GetMatchStatsScoringList {get;set;}


    public async Task OnGetAsync(string tournamentId, string matchNo)
    {
        GetMatchStatsScoringList = await _context.GetMatchStatsScoring
            .FromSqlInterpolated($"EXEC GetMatchStats_Scoring @MatchNo = {matchNo}, @TournamentId = {tournamentId}")
            .ToListAsync();

        if (GetMatchStatsScoringList == null || !GetMatchStatsScoringList.Any())
        {
            _logger.LogWarning($"No player data found for MatchNo {matchNo} and TournamentId {tournamentId}");
        }

        StatMasterList = await _context.StatMaster
            .FromSqlInterpolated($"SELECT idStat, Name FROM StatMaster")
            .ToListAsync();
    }
}










