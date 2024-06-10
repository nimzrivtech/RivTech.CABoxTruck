using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class EntityConfiguration : IEntityTypeConfiguration<Entity.Entity>
    {
        public void Configure(EntityTypeBuilder<Entity.Entity> builder)
        {
            builder.Property(e => e.FirstName).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(e => e.MiddleName).HasColumnType("varchar(40)").HasMaxLength(40);
            builder.Property(e => e.LastName).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(e => e.CompanyName).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.FullName).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.HomePhone).HasColumnType("varchar(30)").HasMaxLength(30);
            builder.Property(e => e.MobilePhone).HasColumnType("varchar(30)").HasMaxLength(30);
            builder.Property(e => e.WorkPhone).HasColumnType("varchar(30)").HasMaxLength(30);
            builder.Property(e => e.WorkPhoneExtension).HasColumnType("varchar(30)").HasMaxLength(30);
            builder.Property(e => e.WorkFax).HasColumnType("varchar(30)").HasMaxLength(30);
            builder.Property(e => e.PersonalEmailAddress).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.WorkEmailAddress).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.DriverLicenseNumber).HasColumnType("varchar(130)").HasMaxLength(130);
            builder.Property(e => e.DriverLicenseState).HasColumnType("varchar(30)").HasMaxLength(30);
            builder.Property(e => e.DBA).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.FederalIDNumber).HasColumnType("nvarchar(20)").HasMaxLength(20);
            builder.Property(e => e.ICCMCDocketNumber).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(e => e.USDOTNumber).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(e => e.McNumber).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(e => e.PUCNumber).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(e => e.NAICSCode).HasColumnType("varchar(20)").HasMaxLength(20);
            builder.Property(e => e.SocialSecurityNumber).HasColumnType("nvarchar(20)").HasMaxLength(20);
            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.CreatedDate).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();

            builder.HasMany(a => a.EntityAddresses).WithOne(b => b.Entity).HasForeignKey(c => c.EntityId);
        }
    }
}
