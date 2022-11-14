using Exercise9.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exercise9.EfConfigurations
{
    public class DoctorEntityTypeConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> opt)
        {
            opt.HasKey(e => e.IdDoctor);
            opt.Property(e => e.IdDoctor).ValueGeneratedOnAdd();
            opt.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            opt.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            opt.Property(e => e.Email).IsRequired().HasMaxLength(100);

            opt.HasData(
                new Doctor { IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan@xxx" },
                new Doctor { IdDoctor = 2, FirstName = "Anna", LastName = "Nowak", Email = "anna@xxx" }
                );
        }
    }
}
