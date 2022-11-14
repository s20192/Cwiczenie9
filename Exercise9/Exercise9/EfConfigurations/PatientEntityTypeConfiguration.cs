using Exercise9.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exercise9.EfConfigurations
{
    public class PatientEntityTypeConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> opt)
        {
            opt.HasKey(e => e.IdPatient);
            opt.Property(e => e.IdPatient).ValueGeneratedOnAdd();
            opt.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            opt.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            opt.Property(e => e.Birthdate).IsRequired().HasColumnType(typeName: "Date");

            opt.HasData(
                new Patient { IdPatient = 1, FirstName = "Zdzisław", LastName = "Fajny", Birthdate = new DateTime(1980, 5, 1, 0, 0, 0) },
                new Patient { IdPatient = 2, FirstName = "Halina", LastName = "XYZ", Birthdate = new DateTime(1985, 3, 12, 0, 0, 0) }

                );
        }
    }
}
