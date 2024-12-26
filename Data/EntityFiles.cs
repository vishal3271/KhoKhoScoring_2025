using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Net.Http.Headers;

namespace _24IN_Ultimate_KHO_KHO_VS.Data
{
    public class StatMaster 
    {
        [Key] 
        public string idStat {get;set;}
        public string Name { get; set; }  
    }

    public class PlayerMaster
    {
        [Key]
        public string idPlayer {get; set;}
        public string Player_Name {get; set;}
    }

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

    public class TeamMaster
    {
        [Key]
        public string  idTeam {get;set;}
        public String Team_Name {get;set;}
    }

    public class Inning_Batch
    {
        public int id{get;set;}
        public int batch_number{get;set;}
        public int player_id{get;set;}
        public int inning_number{get;set;}
    }
    
    public class dataEnter_Scoring
    {
        [Key]
        public string idTournament {get;set;}
        public string playername {get;set;}
        public string team_category {get;set;}
        public string MatchNo {get;set;}
        public string playerid {get;set;}
        public string playerstatus{get;set;}
        public string teamName{get;set;}
    }

}
