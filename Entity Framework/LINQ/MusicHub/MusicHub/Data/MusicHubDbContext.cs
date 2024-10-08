﻿using Microsoft.EntityFrameworkCore;
using MusicHub.Data.Models;

namespace MusicHub.Data
{
    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext() {}

        public MusicHubDbContext(DbContextOptions options) : base(options) {}

        public DbSet<Performer> Performers { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<SongPerformer> SongPerformers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                string connectionString = "Server=.;Database=MusicHub;Integrated Security=True;";

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SongPerformer>()
                .HasKey(p => new { p.SongId, p.PerformerId });
        }
    }
}
