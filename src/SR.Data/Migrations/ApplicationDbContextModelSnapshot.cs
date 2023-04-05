﻿// <auto-generated />
using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SR.Data;

#nullable disable

namespace SR.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SR.Shared.Entities.AshorePass", b =>
                {
                    b.Property<Guid>("AshorePassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CrewProcessingId")
                        .HasColumnType("uuid");

                    b.Property<int>("NumberOfPass")
                        .HasColumnType("integer");

                    b.Property<int>("PassTypeId")
                        .HasColumnType("integer");

                    b.HasKey("AshorePassId");

                    b.HasIndex("CrewProcessingId");

                    b.HasIndex("PassTypeId");

                    b.ToTable("AshorePasses");
                });

            modelBuilder.Entity("SR.Shared.Entities.AshorePassType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AshorePassTypes");
                });

            modelBuilder.Entity("SR.Shared.Entities.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Action")
                        .HasColumnType("text");

                    b.Property<string>("AffectedColumns")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<IPAddress>("IpAddress")
                        .HasColumnType("inet");

                    b.Property<string>("NewValues")
                        .HasColumnType("text");

                    b.Property<string>("OldValues")
                        .HasColumnType("text");

                    b.Property<string>("PrimaryKey")
                        .HasColumnType("text");

                    b.Property<string>("TableName")
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("SR.Shared.Entities.CrewProcessing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OfficerId")
                        .HasColumnType("uuid");

                    b.Property<int>("Total")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OfficerId");

                    b.ToTable("CrewProcessings");
                });

            modelBuilder.Entity("SR.Shared.Entities.DisEmbarkation", b =>
                {
                    b.Property<Guid>("DisEmbarkationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<Guid>("CrewProcessingId")
                        .HasColumnType("uuid");

                    b.Property<int>("NationalityId")
                        .HasColumnType("integer");

                    b.HasKey("DisEmbarkationId");

                    b.HasIndex("CrewProcessingId");

                    b.HasIndex("NationalityId");

                    b.ToTable("DisEmbarkations");
                });

            modelBuilder.Entity("SR.Shared.Entities.Embarkation", b =>
                {
                    b.Property<Guid>("EmbarkationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<Guid>("CrewProcessingId")
                        .HasColumnType("uuid");

                    b.Property<int>("NationalityId")
                        .HasColumnType("integer");

                    b.HasKey("EmbarkationId");

                    b.HasIndex("CrewProcessingId");

                    b.HasIndex("NationalityId");

                    b.ToTable("Embarkations");
                });

            modelBuilder.Entity("SR.Shared.Entities.NationalityType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Nationalities");
                });

            modelBuilder.Entity("SR.Shared.Entities.Office", b =>
                {
                    b.Property<Guid>("OfficeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("GpsCode")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Region")
                        .HasColumnType("text");

                    b.HasKey("OfficeId");

                    b.ToTable("Office");
                });

            modelBuilder.Entity("SR.Shared.Entities.Officer", b =>
                {
                    b.Property<Guid>("OfficerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<int>("Rank")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("OfficerId");

                    b.ToTable("Officers");
                });

            modelBuilder.Entity("SR.Shared.Entities.OperationInspection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OfficerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OperationOfficeId")
                        .HasColumnType("uuid");

                    b.Property<int>("Total")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OfficerId");

                    b.HasIndex("OperationOfficeId");

                    b.ToTable("OperationInspections");
                });

            modelBuilder.Entity("SR.Shared.Entities.OperationInspectionDetail", b =>
                {
                    b.Property<Guid>("OperationInspectionDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<Guid>("OperationInspectionId")
                        .HasColumnType("uuid");

                    b.Property<int>("OperationTypeId")
                        .HasColumnType("integer");

                    b.HasKey("OperationInspectionDetailId");

                    b.HasIndex("OperationInspectionId");

                    b.HasIndex("OperationTypeId");

                    b.ToTable("OperationInspectionDetails");
                });

            modelBuilder.Entity("SR.Shared.Entities.OperationOffice", b =>
                {
                    b.Property<Guid>("OperationOfficeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("OfficeName")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.HasKey("OperationOfficeId");

                    b.ToTable("OperationOffices");
                });

            modelBuilder.Entity("SR.Shared.Entities.OperationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OperationTypes");
                });

            modelBuilder.Entity("SR.Shared.Entities.PassportProcessing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("NumberDeclined")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfAdults")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfChildren")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfMissing")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfNew")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfRenewal")
                        .HasColumnType("integer");

                    b.Property<Guid>("OfficerId")
                        .HasColumnType("uuid");

                    b.Property<int>("TotalApplications")
                        .HasColumnType("integer");

                    b.Property<int>("TotalProcessed")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OfficerId");

                    b.ToTable("PassportProcessings");
                });

            modelBuilder.Entity("SR.Shared.Entities.PermitProcessing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OfficerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Penalties")
                        .HasColumnType("text");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<int>("Total")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OfficerId");

                    b.ToTable("PermitProcessings");
                });

            modelBuilder.Entity("SR.Shared.Entities.PermitProcessingDetail", b =>
                {
                    b.Property<Guid>("PermitProcessingDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Duration")
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("NationalityId")
                        .HasColumnType("integer");

                    b.Property<string>("PermitNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("PermitProcessingId")
                        .HasColumnType("uuid");

                    b.Property<int>("PermitTypeId")
                        .HasColumnType("integer");

                    b.HasKey("PermitProcessingDetailId");

                    b.HasIndex("NationalityId");

                    b.HasIndex("PermitProcessingId");

                    b.HasIndex("PermitTypeId");

                    b.ToTable("PermitProcessingDetails");
                });

            modelBuilder.Entity("SR.Shared.Entities.PermitType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PermitTypes");
                });

            modelBuilder.Entity("SR.Shared.Entities.RevenueCollection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OfficerId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("OfficerId");

                    b.ToTable("RevenueCollections");
                });

            modelBuilder.Entity("SR.Shared.Entities.RevenueCollectionDetail", b =>
                {
                    b.Property<Guid>("RevenueCollectionDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("RevenueCollectionId")
                        .HasColumnType("uuid");

                    b.Property<int>("RevenueTypeId")
                        .HasColumnType("integer");

                    b.HasKey("RevenueCollectionDetailId");

                    b.HasIndex("RevenueCollectionId");

                    b.HasIndex("RevenueTypeId");

                    b.ToTable("RevenueCollectionDetails");
                });

            modelBuilder.Entity("SR.Shared.Entities.RevenueType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RevenueTypes");
                });

            modelBuilder.Entity("SR.Shared.Entities.VesselsDocked", b =>
                {
                    b.Property<Guid>("VesselDockedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<Guid>("CrewProcessingId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("VesselTypeId")
                        .HasColumnType("integer");

                    b.HasKey("VesselDockedId");

                    b.HasIndex("CrewProcessingId");

                    b.HasIndex("VesselTypeId");

                    b.ToTable("VesselsDocked");
                });

            modelBuilder.Entity("SR.Shared.Entities.VesselType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("VesselTypes");
                });

            modelBuilder.Entity("SR.Shared.Identity.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("OfficerId")
                        .HasColumnType("uuid");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("OfficerId")
                        .IsUnique();

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SR.Shared.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SR.Shared.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SR.Shared.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SR.Shared.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SR.Shared.Entities.AshorePass", b =>
                {
                    b.HasOne("SR.Shared.Entities.CrewProcessing", "CrewProcessing")
                        .WithMany()
                        .HasForeignKey("CrewProcessingId");

                    b.HasOne("SR.Shared.Entities.AshorePassType", "PassType")
                        .WithMany()
                        .HasForeignKey("PassTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CrewProcessing");

                    b.Navigation("PassType");
                });

            modelBuilder.Entity("SR.Shared.Entities.CrewProcessing", b =>
                {
                    b.HasOne("SR.Shared.Entities.Officer", "Officer")
                        .WithMany("CrewProcessings")
                        .HasForeignKey("OfficerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Officer");
                });

            modelBuilder.Entity("SR.Shared.Entities.DisEmbarkation", b =>
                {
                    b.HasOne("SR.Shared.Entities.CrewProcessing", "CrewProcessing")
                        .WithMany("DisEmbarkations")
                        .HasForeignKey("CrewProcessingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SR.Shared.Entities.NationalityType", "Nationality")
                        .WithMany()
                        .HasForeignKey("NationalityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CrewProcessing");

                    b.Navigation("Nationality");
                });

            modelBuilder.Entity("SR.Shared.Entities.Embarkation", b =>
                {
                    b.HasOne("SR.Shared.Entities.CrewProcessing", "CrewProcessing")
                        .WithMany("Embarkations")
                        .HasForeignKey("CrewProcessingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SR.Shared.Entities.NationalityType", "Nationality")
                        .WithMany()
                        .HasForeignKey("NationalityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CrewProcessing");

                    b.Navigation("Nationality");
                });

            modelBuilder.Entity("SR.Shared.Entities.OperationInspection", b =>
                {
                    b.HasOne("SR.Shared.Entities.Officer", "Officer")
                        .WithMany("OperationInspections")
                        .HasForeignKey("OfficerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SR.Shared.Entities.OperationOffice", "OperationOffice")
                        .WithMany("OperationInspections")
                        .HasForeignKey("OperationOfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Officer");

                    b.Navigation("OperationOffice");
                });

            modelBuilder.Entity("SR.Shared.Entities.OperationInspectionDetail", b =>
                {
                    b.HasOne("SR.Shared.Entities.OperationInspection", "OperationInspection")
                        .WithMany("OperationInspectionDetails")
                        .HasForeignKey("OperationInspectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SR.Shared.Entities.OperationType", "OperationType")
                        .WithMany()
                        .HasForeignKey("OperationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OperationInspection");

                    b.Navigation("OperationType");
                });

            modelBuilder.Entity("SR.Shared.Entities.PassportProcessing", b =>
                {
                    b.HasOne("SR.Shared.Entities.Officer", "Officer")
                        .WithMany("PassportProcessings")
                        .HasForeignKey("OfficerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Officer");
                });

            modelBuilder.Entity("SR.Shared.Entities.PermitProcessing", b =>
                {
                    b.HasOne("SR.Shared.Entities.Officer", "Officer")
                        .WithMany("PermitProcessings")
                        .HasForeignKey("OfficerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Officer");
                });

            modelBuilder.Entity("SR.Shared.Entities.PermitProcessingDetail", b =>
                {
                    b.HasOne("SR.Shared.Entities.NationalityType", "Nationality")
                        .WithMany()
                        .HasForeignKey("NationalityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SR.Shared.Entities.PermitProcessing", "PermitProcessing")
                        .WithMany("PermitProcessingDetails")
                        .HasForeignKey("PermitProcessingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SR.Shared.Entities.PermitType", "PermitType")
                        .WithMany()
                        .HasForeignKey("PermitTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Nationality");

                    b.Navigation("PermitProcessing");

                    b.Navigation("PermitType");
                });

            modelBuilder.Entity("SR.Shared.Entities.RevenueCollection", b =>
                {
                    b.HasOne("SR.Shared.Entities.Officer", "Officer")
                        .WithMany("RevenueCollections")
                        .HasForeignKey("OfficerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Officer");
                });

            modelBuilder.Entity("SR.Shared.Entities.RevenueCollectionDetail", b =>
                {
                    b.HasOne("SR.Shared.Entities.RevenueCollection", "RevenueCollection")
                        .WithMany("RevenueCollectionDetails")
                        .HasForeignKey("RevenueCollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SR.Shared.Entities.RevenueType", "RevenueType")
                        .WithMany()
                        .HasForeignKey("RevenueTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RevenueCollection");

                    b.Navigation("RevenueType");
                });

            modelBuilder.Entity("SR.Shared.Entities.VesselsDocked", b =>
                {
                    b.HasOne("SR.Shared.Entities.CrewProcessing", "CrewProcessing")
                        .WithMany("VesselsDocked")
                        .HasForeignKey("CrewProcessingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SR.Shared.Entities.VesselType", "VesselType")
                        .WithMany()
                        .HasForeignKey("VesselTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CrewProcessing");

                    b.Navigation("VesselType");
                });

            modelBuilder.Entity("SR.Shared.Identity.ApplicationUser", b =>
                {
                    b.HasOne("SR.Shared.Entities.Officer", null)
                        .WithOne("User")
                        .HasForeignKey("SR.Shared.Identity.ApplicationUser", "OfficerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SR.Shared.Entities.CrewProcessing", b =>
                {
                    b.Navigation("DisEmbarkations");

                    b.Navigation("Embarkations");

                    b.Navigation("VesselsDocked");
                });

            modelBuilder.Entity("SR.Shared.Entities.Officer", b =>
                {
                    b.Navigation("CrewProcessings");

                    b.Navigation("OperationInspections");

                    b.Navigation("PassportProcessings");

                    b.Navigation("PermitProcessings");

                    b.Navigation("RevenueCollections");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SR.Shared.Entities.OperationInspection", b =>
                {
                    b.Navigation("OperationInspectionDetails");
                });

            modelBuilder.Entity("SR.Shared.Entities.OperationOffice", b =>
                {
                    b.Navigation("OperationInspections");
                });

            modelBuilder.Entity("SR.Shared.Entities.PermitProcessing", b =>
                {
                    b.Navigation("PermitProcessingDetails");
                });

            modelBuilder.Entity("SR.Shared.Entities.RevenueCollection", b =>
                {
                    b.Navigation("RevenueCollectionDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
