using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations.Schema;

namespace _24IN_Ultimate_KHO_KHO_VS.Data
{

    public class TournamentMaster
    {
        [Key]
        public string idTournament {get;set;}
        public string Tournament_Name {get; set;}

    }
    public class GetMatchNameTossData_Scoring
    {
        [Key]
        public string idTournament{get;set;}
        public string TournamentName{get;set;}
        public string MatchName{get;set;}
        public string MatchNo {get;set;}
        public string HomeTeam{get;set;}
        public string AwayTeam{get;set;}

    }

    public class MatchMaster
    {
        [Key]
        public string idTournament {get;set;}
        public string MatchNo{get;set;}
        public string Match_Name {get;set;}
        public string idMatch {get;set;}

    }

    public class GetMatchStats_Scoring
    {
    [Key]
    public string idTournament { get; set; }
    public string MatchNo{get;set;}
    public string idTie { get; set; }
    public string idHomeTeam { get; set; }
    public string idAwayTeam { get; set; }
    public string HomeTeamName { get; set; }
    public string AwayTeamName { get; set; }

    // Home Player Details
    public string HomePlayerID { get; set; }
    public string HomePlayerName { get; set; }  
    public string HomePlayerStatus { get; set; }
    public string HomeBatchNumber { get; set; } 

    // Away Player Details
    public string AwayPlayerID { get; set; }
    public string AwayPlayerName { get; set; }  
    public string AwayPlayerStatus { get; set; }
    public string AwayBatchNumber { get; set; } 
    }
    
    public class dataEnter_Scoring
    {
        [Key]
        public string idMatch{get;set;}
        public string idTournament {get;set;}
        public string playername {get;set;}
        public string team_category {get;set;}
        public string MatchNo {get;set;}
        public string playerid {get;set;}
        public string playerstatus{get;set;}
        public string teamName{get;set;}
        public string teamId {get;set;}
        // public string batchno {get;set;}
        // public string isWazir{get;set;}
         public int? batchno {get;set;}
        public bool? isWazir{get;set;}
    }

    public class Toss
    {
        [Key]
        public string App_MatchId {get;set;}
        public int TossWinnerId {get;set;}
        public int IsAttacking {get;set;}
        public string idTournament {get;set;}
    }

    public class dataEnterPlayerDetails_Scoring
    {
        [Key]
        public int id {get;set;}
        public string playerid {get;set;}
        public string idTournament {get;set;}
        public string playername {get;set;}
        public string team_category {get;set;}
        public string idMatch {get;set;}

        public int IsAttacking {get;set;}
        public string teamName {get;set;}
        public string teamId {get;set;}
        public string playerstatus {get;set;}
        // public string batchno {get;set;}
        // public string iswazir {get;set;}
        public int batchno {get;set;}
        public bool iswazir {get;set;}

    }

    public class Inning_Batch_Scoring
    {
        [Key]
        public int id { get; set; }

        public int series_id { get; set; }

        public int match_id { get; set; }

        public int inning_number { get; set; }

        public int run_number { get; set; }

        public int attacking_team_id { get; set; }

        public int defending_team_id { get; set; }

        public int batch_number { get; set; }

        public int attempt_number { get; set; }

        public int defender_id { get; set; }

        public int time_on_court_batch { get; set; }

        public string InningTimer { get; set; }

        public string TurnTimer { get; set; }

        public int time_on_court_player { get; set; }

        public bool isOut { get; set; }

        public bool isBatchActive { get; set; }

        public bool isPowerPlay { get; set; }

        public string App_MatchId { get; set; }

        public int attacker_id { get; set; }

        public int out_type_id { get; set; }

        public int gfxBatchNo { get; set; }

        public int attackingPoints { get; set; }
        public int Turn {get;set;}  
        public int defendingPoints {get;set;}
    }

     public class HowOut 
    {
        [Key] 
        public int OutId {get;set;}
        public string OutType { get; set; }
        public int points {get;set;}  
    }

    public class MatchScore
    {
        [Key]
        public int id {get;set;}
        public string MatchScoreid {get;set;}
        public string tourid {get;set;}
        public string tieid {get;set;}
        public string matchId {get;set;}
        public string gamescore {get;set;}
        public string hometeam {get;set;}
        public string awayteam {get;set;}
        public string winningteam {get;set;}
        public long ishometeamtrump {get;set;}
        public long isawayteamtrump {get;set;}
    }



public class TeamPlayer_CurrentMatch
{
    [Key]
    public int id { get; set; } 

    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("App_MatchId")]
    public string AppMatchId { get; set; }

    [Column("batchno")]
    public int BatchNo { get; set; }

    [Column("time_spend")]
    public int? TimeSpend { get; set; }

    [Column("pole_dive")]
    public int? PoleDive { get; set; }

    [Column("self_out")]
    public int? SelfOut { get; set; }

    [Column("sky_dive")]
    public int? skydive { get; set; }


}

public class timedata
{
        [Key]
        public int id {get;set;}
        public string tourid {get;set;}
        public string matchId {get;set;}
        public string hometeam {get;set;}
        public string awayteam {get;set;}
        public int InningTimer { get; set; }
        public int TurnTimer { get; set; } 

}

public class Inning_Batch
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("series_id")]
    public int SeriesId { get; set; }
    [Column("match_id")]
    public int MatchId { get; set; }
    [Column("inning_number")]
    public int InningNumber { get; set; }
    [Column("run_number")]
    public int RunNumber { get; set; }
    [Column("attacking_team_id")]
    public int AttackingTeamId { get; set; }
    [Column("defending_team_id")]
    public int DefendingTeamId { get; set; }
    [Column("batch_number")]
    public int BatchNumber { get; set; }
    [Column("attempt_number")]
    public int AttemptNumber { get; set; }
    [Column("player_id")]
    public int PlayerId { get; set; }
    [Column("time_on_court_batch")]
    public int? TimeOnCourtBatch { get; set; } 
    [Column("clock_in")]
    public string? ClockIn { get; set; } 
    [Column("clock_out")]
    public string? ClockOut { get; set; } 
    [Column("time_on_court_player")]
    public int? TimeOnCourtPlayer { get; set; }
    [Column("isOut")]
    public bool IsOut { get; set; }
    [Column("isBatchActive")]
    public bool IsBatchActive { get; set; }
    [Column("isPowerPlay")]
    public bool IsPowerPlay { get; set; }
    [Column("App_MatchId")]
    public string AppMatchId { get; set; }
    [Column("attacker_id")] 
    public int? AttackerId { get; set; }
    [Column("out_type_id")] 
    public int? OutTypeId { get; set; }
    [Column("gfxBatchNo")] 
    public int? GfxBatchNo { get; set; }
}


}

