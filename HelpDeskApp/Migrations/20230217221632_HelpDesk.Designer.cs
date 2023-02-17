﻿// <auto-generated />
using System;
using HelpDeskApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HelpDeskApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230217221632_HelpDesk")]
    partial class HelpDesk
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("HelpDeskApp.Models.TicketCategories", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("TicketCategories");
                });

            modelBuilder.Entity("HelpDeskApp.Models.TicketStatuses", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("TicketStatuses");
                });

            modelBuilder.Entity("HelpDeskApp.Models.Tickets", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descriptions")
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("responsibleUserID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ticketCategoryID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ticketOwnerID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ticketStatusID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("responsibleUserID");

                    b.HasIndex("ticketCategoryID");

                    b.HasIndex("ticketOwnerID");

                    b.HasIndex("ticketStatusID");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("HelpDeskApp.Models.Users", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HelpDeskApp.Models.Tickets", b =>
                {
                    b.HasOne("HelpDeskApp.Models.Users", "responsibleUser")
                        .WithMany()
                        .HasForeignKey("responsibleUserID");

                    b.HasOne("HelpDeskApp.Models.TicketCategories", "ticketCategory")
                        .WithMany()
                        .HasForeignKey("ticketCategoryID");

                    b.HasOne("HelpDeskApp.Models.Users", "ticketOwner")
                        .WithMany()
                        .HasForeignKey("ticketOwnerID");

                    b.HasOne("HelpDeskApp.Models.TicketStatuses", "ticketStatus")
                        .WithMany()
                        .HasForeignKey("ticketStatusID");

                    b.Navigation("responsibleUser");

                    b.Navigation("ticketCategory");

                    b.Navigation("ticketOwner");

                    b.Navigation("ticketStatus");
                });
#pragma warning restore 612, 618
        }
    }
}
