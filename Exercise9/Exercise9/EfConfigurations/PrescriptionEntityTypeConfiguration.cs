using Exercise9.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Exercise9.EfConfigurations
{
    public class PrescriptionEntityTypeConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> opt)
        {
            opt.HasKey(e => e.IdPrescription);
            opt.Property(e => e.IdPrescription).ValueGeneratedOnAdd();
            opt.Property(e => e.Date).IsRequired().HasColumnType(typeName: "Date");
            opt.Property(e => e.DueDate).IsRequired().HasColumnType(typeName: "Date");

            opt.HasOne(p => p.Doctor)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(p => p.IdDoctor);

            opt.HasOne(p => p.Patient)
                .WithMany(pt => pt.Prescriptions)
                .HasForeignKey(p => p.IdPatient);

            opt.HasData(
                new Prescription
                {
                    IdPrescription = 1,
                    Date = new DateTime(2022, 5, 1, 0, 0, 0),
                    DueDate = new DateTime(2022, 5, 30, 0, 0, 0),
                    IdPatient = 1,
                    IdDoctor = 2
                },
                new Prescription
                {
                    IdPrescription = 2,
                    Date = new DateTime(2022, 4, 1, 0, 0, 0),
                    DueDate = new DateTime(2022, 4, 30, 0, 0, 0),
                    IdPatient = 2,
                    IdDoctor = 2
                }
                );
        }
    }
}
