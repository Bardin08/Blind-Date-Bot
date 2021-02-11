﻿// <auto-generated />
using System;
using BlindDateBot.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlindDateBot.Data.Migrations
{
    [DbContext(typeof(SqlServerContext))]
    [Migration("20210211063210_AddFieldsForBlockedUser")]
    partial class AddFieldsForBlockedUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("BlindDateBot.Domain.Models.DateModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("FirstUserId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("SecondUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FirstUserId");

                    b.HasIndex("SecondUserId");

                    b.ToTable("Dates");
                });

            modelBuilder.Entity("BlindDateBot.Domain.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id")
                        .UseIdentityColumn();

                    b.Property<string>("BlockReason")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("")
                        .HasColumnName("block_reason");

                    b.Property<int>("ComplaintsAmount")
                        .HasColumnType("int");

                    b.Property<int>("Gender")
                        .HasColumnType("int")
                        .HasColumnName("user_gender");

                    b.Property<int>("InterlocutorGender")
                        .HasColumnType("int")
                        .HasColumnName("interlocutor_gender");

                    b.Property<bool>("IsBlocked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("is_blocked");

                    b.Property<bool>("IsFree")
                        .HasColumnType("bit")
                        .HasColumnName("is_free");

                    b.Property<bool>("IsVisible")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("is_visible");

                    b.Property<int>("TelegramId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("user_pkey");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlindDateBot.Domain.Models.DateModel", b =>
                {
                    b.HasOne("BlindDateBot.Domain.Models.UserModel", "FirstUser")
                        .WithMany()
                        .HasForeignKey("FirstUserId");

                    b.HasOne("BlindDateBot.Domain.Models.UserModel", "SecondUser")
                        .WithMany()
                        .HasForeignKey("SecondUserId");

                    b.Navigation("FirstUser");

                    b.Navigation("SecondUser");
                });
#pragma warning restore 612, 618
        }
    }
}
