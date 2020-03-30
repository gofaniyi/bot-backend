﻿// <auto-generated />
using System;
using GloEpidBot.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GloEpidBot.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GloEpidBot.Model.Domain.PartnerToken", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateJoined")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PartnerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartnerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PartnerTokens");
                });

            modelBuilder.Entity("GloEpidBot.Model.Domain.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateReported")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReporterName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RiskStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symptoms")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Reports");

                    b.HasData(
                        new
                        {
                            Id = 100,
                            Age = 25,
                            DateReported = new DateTime(2020, 3, 29, 19, 4, 37, 116, DateTimeKind.Local).AddTicks(7145),
                            Location = "Cooperative Villas, Ajah",
                            ReporterName = "Kenny",
                            RiskStatus = "High",
                            Symptoms = "I have been experiencing sever cough and headache"
                        },
                        new
                        {
                            Id = 101,
                            Age = 50,
                            DateReported = new DateTime(2020, 3, 29, 19, 4, 37, 118, DateTimeKind.Local).AddTicks(4570),
                            Location = "Cooperative Villas, Ajah",
                            ReporterName = "Teju",
                            RiskStatus = "High",
                            Symptoms = "cough and difficulty in breathing"
                        },
                        new
                        {
                            Id = 102,
                            Age = 70,
                            DateReported = new DateTime(2020, 3, 29, 19, 4, 37, 118, DateTimeKind.Local).AddTicks(4622),
                            Location = "Cooperative Villas, Ajah",
                            ReporterName = "Kenny",
                            RiskStatus = "low",
                            Symptoms = "None"
                        },
                        new
                        {
                            Id = 103,
                            Age = 30,
                            DateReported = new DateTime(2020, 3, 29, 19, 4, 37, 118, DateTimeKind.Local).AddTicks(4625),
                            Location = "Cooperative Villas, Ajah",
                            ReporterName = "Kenny",
                            RiskStatus = "low",
                            Symptoms = "None"
                        },
                        new
                        {
                            Id = 4,
                            Age = 80,
                            DateReported = new DateTime(2020, 3, 29, 19, 4, 37, 118, DateTimeKind.Local).AddTicks(4628),
                            Location = "Cooperative Villas, Ajah",
                            ReporterName = "Tega",
                            RiskStatus = "High",
                            Symptoms = "sever cough and headache"
                        });
                });

            modelBuilder.Entity("GloEpidBot.Model.Domain.SelfAssesment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CloseContact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HouseAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ocupation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicPlaces")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symptoms")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SymptomsStart")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TravelHistory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TravelPlaces")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Assesments");
                });

            modelBuilder.Entity("GloEpidBot.Model.Domain.assesment", b =>
                {
                    b.Property<string>("assesmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("evaluationOutcome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("evaluationScore")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("evaluationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("publicKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("source")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("assesmentId");

                    b.ToTable("GetAssesments");
                });

            modelBuilder.Entity("GloEpidBot.Model.Domain.questions", b =>
                {
                    b.Property<string>("questionsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("assesmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("question")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("response")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("score")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("questionsId");

                    b.HasIndex("assesmentId");

                    b.ToTable("questions");
                });

            modelBuilder.Entity("GloEpidBot.Model.Domain.questions", b =>
                {
                    b.HasOne("GloEpidBot.Model.Domain.assesment", null)
                        .WithMany("questions")
                        .HasForeignKey("assesmentId");
                });
#pragma warning restore 612, 618
        }
    }
}
