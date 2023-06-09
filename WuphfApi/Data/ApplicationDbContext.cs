using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using WuphfApi.Models;

namespace WuphfApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Commento> Commenti { get; set; }
        public DbSet<Segue> Segue { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Messaggio> Messaggi { get; set; }
        public DbSet<Visualizzato> Visualizzato { get; set; }
        public DbSet<Storia> Storie { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
