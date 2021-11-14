﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrackDataAccess.Database;

namespace TrackDataAccess.Migrations
{
    [DbContext(typeof(TrackDbContext))]
    partial class TrackDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(256)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(256)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(256)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TrackDataAccess.Models.AccessCode", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime")
                        .HasColumnName("creation_time");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("access_codes");
                });

            modelBuilder.Entity("TrackDataAccess.Models.CommandLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime")
                        .HasColumnName("creation_time");

                    b.Property<string>("Payload")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)")
                        .HasColumnName("payload");

                    b.Property<string>("Response")
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)")
                        .HasColumnName("response");

                    b.Property<string>("TrackerId")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("tracker_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("type");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("command_logs");
                });

            modelBuilder.Entity("TrackDataAccess.Models.Identity.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime")
                        .HasColumnName("creation_time");

                    b.Property<DateTime?>("DeleteTime")
                        .HasColumnType("datetime")
                        .HasColumnName("delete_time");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Explanation")
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)")
                        .HasColumnName("explanation");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TrackDataAccess.Models.Image", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("id");

                    b.Property<byte[]>("Bytes")
                        .IsRequired()
                        .HasColumnType("MEDIUMBLOB")
                        .HasColumnName("bytes");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime")
                        .HasColumnName("creation_time");

                    b.Property<int>("Height")
                        .HasColumnType("int")
                        .HasColumnName("height");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("name");

                    b.Property<int>("Width")
                        .HasColumnType("int")
                        .HasColumnName("width");

                    b.HasKey("Id");

                    b.ToTable("images");
                });

            modelBuilder.Entity("TrackDataAccess.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime")
                        .HasColumnName("creation_time");

                    b.Property<string>("TrackerId")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("tracker_id");

                    b.Property<string>("discriminator")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("TrackerId");

                    b.ToTable("messages");

                    b.HasDiscriminator<string>("discriminator").HasValue("message");
                });

            modelBuilder.Entity("TrackDataAccess.Models.Tracker", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("id");

                    b.Property<string>("CommandSet")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("command_set");

                    b.Property<string>("Configs")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("configs");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime")
                        .HasColumnName("creation_time");

                    b.Property<string>("DefaultIcon")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("default_icon");

                    b.Property<DateTime?>("DeleteTime")
                        .HasColumnType("datetime")
                        .HasColumnName("delete_time");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("display_name");

                    b.Property<string>("Explanation")
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)")
                        .HasColumnName("explanation");

                    b.Property<string>("IconImageId")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("icon_image_id");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastConnectedServer")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("last_connected_server");

                    b.Property<DateTime?>("LastConnection")
                        .HasColumnType("datetime")
                        .HasColumnName("last_connection");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("varchar(8)")
                        .HasColumnName("manufacturer");

                    b.Property<string>("ProductModel")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("product_model");

                    b.Property<string>("ProductType")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("product_type");

                    b.Property<string>("RawID")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("raw_id");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("serial_number");

                    b.Property<bool>("ShowOnMap")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("show_on_map");

                    b.Property<string>("Status")
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("status");

                    b.Property<string>("UserId")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("trackers");
                });

            modelBuilder.Entity("TrackDataAccess.Models.TrackerPermittedUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("user_id");

                    b.Property<string>("TrackerId")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("tracker_id");

                    b.Property<string>("Permissions")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("permissions");

                    b.HasKey("UserId", "TrackerId");

                    b.HasIndex("TrackerId");

                    b.ToTable("tracker_permitted_users");
                });

            modelBuilder.Entity("TrackDataAccess.Models.TrackerUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("user_id");

                    b.Property<string>("TrackerId")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("tracker_id");

                    b.HasKey("UserId", "TrackerId");

                    b.HasIndex("TrackerId");

                    b.ToTable("tracker_users");
                });

            modelBuilder.Entity("TrackDataAccess.Models.GpsTrackerMessage", b =>
                {
                    b.HasBaseType("TrackDataAccess.Models.Message");

                    b.Property<double?>("Altitude")
                        .HasColumnType("double")
                        .HasColumnName("altitude");

                    b.Property<double?>("Battery")
                        .HasColumnType("double")
                        .HasColumnName("battery");

                    b.Property<double>("Direction")
                        .HasColumnType("double")
                        .HasColumnName("direction");

                    b.Property<bool>("IsValid")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_valid");

                    b.Property<double>("Latitude")
                        .HasColumnType("double")
                        .HasColumnName("latitude");

                    b.Property<string>("LatitudeMark")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)")
                        .HasColumnName("latitude_mark");

                    b.Property<double>("Longitude")
                        .HasColumnType("double")
                        .HasColumnName("longitude");

                    b.Property<string>("LongitudeMark")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)")
                        .HasColumnName("longitude_mark");

                    b.Property<DateTime>("MessageTime")
                        .HasColumnType("datetime")
                        .HasColumnName("message_time");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("message_type");

                    b.Property<double?>("SignalStrength")
                        .HasColumnType("double")
                        .HasColumnName("signal_strength");

                    b.Property<double>("Speed")
                        .HasColumnType("double")
                        .HasColumnName("speed");

                    b.Property<string>("TrackerState")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("tracker_state");

                    b.ToTable("messages");

                    b.HasDiscriminator().HasValue("gps_message");
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
                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", null)
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", null)
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

                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TrackDataAccess.Models.Message", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Tracker", "Tracker")
                        .WithMany()
                        .HasForeignKey("TrackerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tracker");
                });

            modelBuilder.Entity("TrackDataAccess.Models.TrackerPermittedUser", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Tracker", "Tracker")
                        .WithMany("PermittedUsers")
                        .HasForeignKey("TrackerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", "User")
                        .WithMany("PermittedTrackers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tracker");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TrackDataAccess.Models.TrackerUser", b =>
                {
                    b.HasOne("TrackDataAccess.Models.Tracker", "Tracker")
                        .WithMany("Users")
                        .HasForeignKey("TrackerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackDataAccess.Models.Identity.AppUser", "User")
                        .WithMany("Trackers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tracker");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TrackDataAccess.Models.Identity.AppUser", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("PermittedTrackers");

                    b.Navigation("Trackers");
                });

            modelBuilder.Entity("TrackDataAccess.Models.Tracker", b =>
                {
                    b.Navigation("PermittedUsers");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
