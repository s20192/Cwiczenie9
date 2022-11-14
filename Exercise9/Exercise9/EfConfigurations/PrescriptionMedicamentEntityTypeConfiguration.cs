using Exercise9.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Exercise9.EfConfigurations
{
    public class PrescriptionMedicamentEntityTypeConfiguration : IEntityTypeConfiguration<Prescription_Medicament>
    {
        public void Configure(EntityTypeBuilder<Prescription_Medicament> opt)
        {
            opt.HasKey(e => new
            {
                e.IdMedicament,
                e.IdPrescription
            });

            opt.Property(e => e.Dose).IsRequired();
            opt.Property(e => e.Details).IsRequired().HasMaxLength(100);

            opt.HasOne(p => p.Medicament)
                .WithMany(m => m.Prescription_Medicaments)
                .HasForeignKey(p => p.IdMedicament);

            opt.HasOne(p => p.Prescription)
                .WithMany(pr => pr.Prescription_Medicaments)
                .HasForeignKey(p => p.IdPrescription);

            opt.HasData(
                new Prescription_Medicament { IdMedicament = 1, IdPrescription = 1, Dose = 5, Details = "treatment1" },
                new Prescription_Medicament { IdMedicament = 2, IdPrescription = 1, Dose = 3, Details = "treatment1" },
                new Prescription_Medicament { IdMedicament = 1, IdPrescription = 2, Dose = 2, Details = "treatment2" },
                new Prescription_Medicament { IdMedicament = 2, IdPrescription = 2, Dose = 4, Details = "treatment2" }
                );
        }
    }
}
