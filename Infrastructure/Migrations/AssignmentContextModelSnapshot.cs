﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AssignmentContext))]
    partial class AssignmentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.DataModel.AssignmentDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CollaboratorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("Infrastructure.DataModel.AssignmentTempDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CollaboratorId")
                        .HasColumnType("uuid");

                    b.Property<string>("DeviceBrand")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DeviceDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DeviceModel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DeviceSerialNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AssignmentsTemp");
                });

            modelBuilder.Entity("Infrastructure.DataModel.CollaboratorDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Collaborators");
                });

            modelBuilder.Entity("Infrastructure.DataModel.DeviceDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Infrastructure.DataModel.AssignmentDataModel", b =>
                {
                    b.OwnsOne("Domain.Models.PeriodDate", "PeriodDate", b1 =>
                        {
                            b1.Property<Guid>("AssignmentDataModelId")
                                .HasColumnType("uuid");

                            b1.Property<DateOnly>("FinalDate")
                                .HasColumnType("date");

                            b1.Property<DateOnly>("InitDate")
                                .HasColumnType("date");

                            b1.HasKey("AssignmentDataModelId");

                            b1.ToTable("Assignments");

                            b1.WithOwner()
                                .HasForeignKey("AssignmentDataModelId");
                        });

                    b.Navigation("PeriodDate")
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.DataModel.AssignmentTempDataModel", b =>
                {
                    b.OwnsOne("Domain.Models.PeriodDate", "PeriodDate", b1 =>
                        {
                            b1.Property<Guid>("AssignmentTempDataModelId")
                                .HasColumnType("uuid");

                            b1.Property<DateOnly>("FinalDate")
                                .HasColumnType("date");

                            b1.Property<DateOnly>("InitDate")
                                .HasColumnType("date");

                            b1.HasKey("AssignmentTempDataModelId");

                            b1.ToTable("AssignmentsTemp");

                            b1.WithOwner()
                                .HasForeignKey("AssignmentTempDataModelId");
                        });

                    b.Navigation("PeriodDate")
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.DataModel.CollaboratorDataModel", b =>
                {
                    b.OwnsOne("Domain.Models.PeriodDateTime", "PeriodDateTime", b1 =>
                        {
                            b1.Property<Guid>("CollaboratorDataModelId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("_finalDate")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<DateTime>("_initDate")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("CollaboratorDataModelId");

                            b1.ToTable("Collaborators");

                            b1.WithOwner()
                                .HasForeignKey("CollaboratorDataModelId");
                        });

                    b.Navigation("PeriodDateTime")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
