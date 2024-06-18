using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions options) : base(options) { }
        public DbSet<UserEntity>? Users { get; set; }
        public DbSet<NoteEntity>? Notes { get; set; }
        public DbSet<LabelEntity>? Labels { get; set; }
        public DbSet<CollaboratorEntity>? Collaborators { get; set; }
        public DbSet<NoteLabelEntity>? NoteLabel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>().HasIndex(u => u.email).IsUnique();

            modelBuilder.Entity<NoteLabelEntity>().HasKey(n1=> new {n1.NoteId,n1.LabelId});
            modelBuilder.Entity<NoteLabelEntity>()
                .HasOne(n1 => n1.Notes)
                .WithMany(n => n.NoteLabel)
                .HasForeignKey(n1 => n1.NoteId);
            modelBuilder.Entity<NoteLabelEntity>()
                .HasOne(n1 => n1.Labels)
                .WithMany(n => n.NoteLabel)
                .HasForeignKey(n1 => n1.LabelId);

            modelBuilder.Entity<CollaboratorEntity>()
           .HasIndex(c => new { c.Email, c.NotesId })
           .IsUnique();

            modelBuilder.Entity<CollaboratorEntity>()
                .HasOne(c => c.Notes)
                .WithMany(n => n.Collabarators)
                .HasForeignKey(c => c.NotesId);
        }
    }
}
