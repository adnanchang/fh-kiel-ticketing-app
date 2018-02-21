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
        public DbSet<Idea> Idea { get; set; }
        public DbSet<Fields> Fields { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Contributors> Contributors { get; set; }
		public DbSet<Proposal> Proposal { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<Comments> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<TicketingApp>(null);
            base.OnModelCreating(modelBuilder);
        }

        //public TicketingApp()
        //{
        //    this.Configuration.LazyLoadingEnabled = false;
        //}
    }
}