using Microsoft.EntityFrameworkCore;

namespace _24IN_Ultimate_KHO_KHO_VS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StatMaster> StatMaster { get; set; }
        public DbSet<PlayerMaster> PlayerMaster { get; set; }
        public DbSet<TournamentMaster> TournamentMaster { get; set;}
        public DbSet<MatchMaster> MatchMaster { get;set; }

        public DbSet<TeamMaster> TeamMaster {get;set;}

        public DbSet<Inning_Batch> InningBatch {get;set;}
        public DbSet<GetMatchStats_Scoring> GetMatchStatsScoring{get;set;} 

        public DbSet<GetMatchNameTossData_Scoring> GetMatchNameTossDataScoring{get;set;}

        public DbSet<dataEnter_Scoring> dataEnterScoring {get;set;}



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatMaster>()
                .ToTable("StatMaster"); 

            modelBuilder.Entity<PlayerMaster>()
                .ToTable("PlayerMaster");

            modelBuilder.Entity<TournamentMaster>()
                .ToTable("TournamentMaster");

            modelBuilder.Entity<MatchMaster>()
            .ToTable("MatchMaster");

            modelBuilder.Entity<TeamMaster>()
            .ToTable("TeamMaster");

            modelBuilder.Entity<Inning_Batch>()
            .ToTable("Inning_Batch");

            modelBuilder.Entity<GetMatchStats_Scoring>()
            .ToTable("GetMatchStats_Scoring"); 

            modelBuilder.Entity<GetMatchNameTossData_Scoring>()
            .ToTable("GetMatchNameTossData_Scoring"); 

            modelBuilder.Entity<dataEnter_Scoring>()
            .ToTable("dataEnter_Scoring"); 

        }


    }
}


