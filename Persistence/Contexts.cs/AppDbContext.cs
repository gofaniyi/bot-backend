using System;
using GloEpidBot.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace GloEpidBot.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
        public virtual DbSet<SelfAssesment> Assesments{get;set;}
        public virtual DbSet<assesment> GetAssesments { get; set; }
        public virtual DbSet<PartnerToken> PartnerTokens { get; set; }

         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Report>().HasData
            (
                new Report { 
                  Id = 100, 
                  ReporterName = "Kenny",  
                  Symptoms = "I have been experiencing sever cough and headache",
                  RiskStatus = "High",
                  Location = "Cooperative Villas, Ajah",
                  Age = 25,
                  DateReported = DateTime.Now
                  },
                new Report { 
                  Id = 101, 
                  ReporterName = "Teju",  
                  Symptoms = "cough and difficulty in breathing",
                  RiskStatus = "High",
                  Location = "Cooperative Villas, Ajah",
                  Age = 50,
                  DateReported = DateTime.Now
                  },
                new Report { 
                  Id = 102, 
                  ReporterName = "Kenny",  
                  Symptoms = "None",
                  RiskStatus = "low",
                  Location = "Cooperative Villas, Ajah",
                  Age = 70,
                  DateReported = DateTime.Now
                  },
                new Report { 
                  Id = 103, 
                  ReporterName = "Kenny",  
                  Symptoms = "None",
                  RiskStatus = "low",
                  Age = 30,
                  Location = "Cooperative Villas, Ajah",
                  DateReported = DateTime.Now
                  },  
                new Report { 
                  Id = 4, 
                  ReporterName = "Tega",  
                  Symptoms = "sever cough and headache",
                  RiskStatus = "High",
                  Age = 80,
                  Location = "Cooperative Villas, Ajah",
                  DateReported = DateTime.Now
                  }
            );

        }
    }
}


