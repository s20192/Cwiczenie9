using Exercise9.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exercise9.EfConfigurations
{
    public class MedicamentEntityTypeConfiguration : IEntityTypeConfiguration<Medicament>
    {
        public void Configure(EntityTypeBuilder<Medicament> opt)
        {
            opt.HasKey(e => e.IdMedicament);
            opt.Property(e => e.IdMedicament).ValueGeneratedOnAdd();
            opt.Property(e => e.Name).IsRequired().HasMaxLength(100);
            opt.Property(e => e.Description).IsRequired().HasMaxLength(100);
            opt.Property(e => e.Type).IsRequired().HasMaxLength(100);
            opt.HasData(
                new Medicament { IdMedicament = 1, Name = "DrugA", Description = "Very strong drug", Type = "syrup" },
                new Medicament { IdMedicament = 2, Name = "DrugB", Description = "Not so strong drug", Type = "pills" }
                );
        }
    }
}
