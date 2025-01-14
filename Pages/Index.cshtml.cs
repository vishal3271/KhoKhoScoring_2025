using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _24IN_Ultimate_KHO_KHO_VS.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;  // Ensure this namespace is included

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context; 
        _logger = logger;
    }

    public List<TournamentMaster> TournamentList { get; set; }
    public List<MatchMaster> MatchList { get; set; }
    public List<GetMatchNameTossData_Scoring> GetMatchNameTossDataScoringList{get;set;}
    public async Task OnGetAsync(string tournamentId, string matchNo)
    {
        TournamentList = await _context.TournamentMaster    
            .FromSqlInterpolated($"select idTournament,Tournament_Name from TournamentMaster")
            .ToListAsync();

        MatchList=await _context.MatchMaster
        .FromSqlInterpolated($"SELECT idTournament, MatchNo, Match_Name, idMatch FROM MatchMaster ORDER BY CAST(MatchNo AS INT) DESC;")
        .ToListAsync();


    }


}









    








