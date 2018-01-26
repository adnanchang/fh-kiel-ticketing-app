using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FH_Kiel_Ticketing_App.Models;

namespace FH_Kiel_Ticketing_App.Context
{
    public class TicketingApp : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Supervisor> Supervisor { get; set; }
        public DbSet<RoleIdentifier> RoleIdentifier { get; set; }
        public DbSet<RoleIdentifierDetails> RoleIdentifierDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<TicketingApp>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}