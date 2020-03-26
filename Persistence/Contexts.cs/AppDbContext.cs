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
                  DateReported = DateTime.Parse("25/03/2020")
                  },
                new Report { 
                  Id = 101, 
                  ReporterName = "Teju",  
                  Symptoms = "cough and difficulty in breathing",
                  RiskStatus = "High",
                  Location = "Cooperative Villas, Ajah",
                  Age = 50,
                  DateReported = DateTime.Parse("24/03/2020")
                  },
                new Report { 
                  Id = 102, 
                  ReporterName = "Kenny",  
                  Symptoms = "None",
                  RiskStatus = "low",
                  Location = "Cooperative Villas, Ajah",
                  Age = 70,
                  DateReported = DateTime.Parse("23/03/2020")
                  },
                new Report { 
                  Id = 103, 
                  ReporterName = "Kenny",  
                  Symptoms = "None",
                  RiskStatus = "low",
                  Age = 30,
                  Location = "Cooperative Villas, Ajah",
                  DateReported = DateTime.Parse("22/03/2020")
                  },  
                new Report { 
                  Id = 4, 
                  ReporterName = "Tega",  
                  Symptoms = "sever cough and headache",
                  RiskStatus = "High",
                  Age = 80,
                  Location = "Cooperative Villas, Ajah",
                  DateReported = DateTime.Parse("21/03/2020")
                  }
            );

        }
    }
}


