﻿// <auto-generated />
using System;
using Feirapp.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Feirapp.Infrastructure.Migrations
{
    [DbContext(typeof(BaseContext))]
    partial class BaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Feirapp.Entities.Entities.Cest", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("NcmCode")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Segment")
                        .HasColumnType("longtext");

                    b.HasKey("Code");

                    b.HasIndex("NcmCode");

                    b.ToTable("Cests");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.GroceryItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AltNames")
                        .HasColumnType("longtext");

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Brand")
                        .HasColumnType("longtext");

                    b.Property<string>("CestCode")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("longtext");

                    b.Property<string>("MeasureUnit")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NcmCode")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Barcode");

                    b.HasIndex("CestCode");

                    b.HasIndex("NcmCode");

                    b.ToTable("GroceryItems");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.Ncm", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Code");

                    b.ToTable("Ncms");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.PriceLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<long>("GroceryItemId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<long>("StoreId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.HasIndex("GroceryItemId", "Barcode", "LogDate", "StoreId")
                        .IsUnique();

                    b.ToTable("PriceLogs");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.Store", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AltNames")
                        .HasColumnType("longtext");

                    b.Property<string>("Cep")
                        .HasColumnType("longtext");

                    b.Property<string>("CityName")
                        .HasColumnType("longtext");

                    b.Property<string>("Cnpj")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Neighborhood")
                        .HasColumnType("longtext");

                    b.Property<string>("State")
                        .HasColumnType("longtext");

                    b.Property<string>("Street")
                        .HasColumnType("longtext");

                    b.Property<string>("StreetNumber")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Cnpj")
                        .IsUnique();

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("FailedLoginAttempts")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.Cest", b =>
                {
                    b.HasOne("Feirapp.Entities.Entities.Ncm", "Ncm")
                        .WithMany("Cests")
                        .HasForeignKey("NcmCode");

                    b.Navigation("Ncm");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.GroceryItem", b =>
                {
                    b.HasOne("Feirapp.Entities.Entities.Cest", "Cest")
                        .WithMany("GroceryItems")
                        .HasForeignKey("CestCode");

                    b.HasOne("Feirapp.Entities.Entities.Ncm", "Ncm")
                        .WithMany()
                        .HasForeignKey("NcmCode");

                    b.Navigation("Cest");

                    b.Navigation("Ncm");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.PriceLog", b =>
                {
                    b.HasOne("Feirapp.Entities.Entities.GroceryItem", "GroceryItem")
                        .WithMany("PriceHistory")
                        .HasForeignKey("GroceryItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Feirapp.Entities.Entities.Store", "Store")
                        .WithMany("PriceLogs")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroceryItem");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.Cest", b =>
                {
                    b.Navigation("GroceryItems");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.GroceryItem", b =>
                {
                    b.Navigation("PriceHistory");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.Ncm", b =>
                {
                    b.Navigation("Cests");
                });

            modelBuilder.Entity("Feirapp.Entities.Entities.Store", b =>
                {
                    b.Navigation("PriceLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
