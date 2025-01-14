using Microsoft.EntityFrameworkCore;

namespace _24IN_Ultimate_KHO_KHO_VS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HowOut> HowOuts { get; set; }
        public DbSet<TournamentMaster> TournamentMaster { get; set;}
        public DbSet<MatchMaster> MatchMaster { get;set; }

        public DbSet<GetMatchStats_Scoring> GetMatchStatsScoring{get;set;} 

        public DbSet<GetMatchNameTossData_Scoring> GetMatchNameTossDataScoring{get;set;}

        public DbSet<dataEnter_Scoring> dataEnterScoring {get;set;}

        public DbSet<Toss> Toss {get;set;}

        public DbSet<dataEnterPlayerDetails_Scoring> dataEnterPlayerDetailsScoring {get;set;}
        public DbSet<Inning_Batch_Scoring> InningBatchScoring {get;set;}

        public DbSet<MatchScore> matchScore {get;set;}

        public DbSet<TeamPlayer_CurrentMatch> TeamPlayerCurrentMatch {get;set;}
        public DbSet<timedata> timedata {get;set;}
        public DbSet<Inning_Batch> InningBatch {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HowOut>()
                .ToTable("HowOut"); 

            modelBuilder.Entity<TournamentMaster>()
                .ToTable("TournamentMaster");

            modelBuilder.Entity<MatchMaster>()
            .ToTable("MatchMaster");

            modelBuilder.Entity<GetMatchStats_Scoring>()
            .ToTable("GetMatchStats_Scoring"); 

            modelBuilder.Entity<GetMatchNameTossData_Scoring>()
            .ToTable("GetMatchNameTossData_Scoring"); 

            modelBuilder.Entity<dataEnter_Scoring>()
            .ToTable("dataEnter_Scoring");

            modelBuilder.Entity<Toss>()
            .ToTable("Toss"); 

            modelBuilder.Entity<dataEnterPlayerDetails_Scoring>()
            .ToTable("dataEnterPlayerDetails_Scoring");

            modelBuilder.Entity<Inning_Batch_Scoring>()
            .ToTable("Inning_Batch_Scoring");

            modelBuilder.Entity<MatchScore>()
            .ToTable("MatchScore");

            modelBuilder.Entity<TeamPlayer_CurrentMatch>()
                .ToTable("TeamPlayer_CurrentMatch");

            modelBuilder.Entity<timedata>()
            .ToTable("timedata");

            modelBuilder.Entity<Inning_Batch>()
            .ToTable("Inning_Batch");

        }


    }
}


