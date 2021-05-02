using BasketBall.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamPlayer>().HasKey(tp => new { tp.TeamId, tp.PersonId }); //TeamPlayer will have primary keys of TeamId and PersonId
            modelBuilder.Entity<TeamGame>().HasKey(tg => new { tg.TeamId, tg.GameId }); // TeamGame will have pripary keys of TeamId and GameId

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }
        public DbSet<TeamGame> TeamGames { get; set; }
    }
}
