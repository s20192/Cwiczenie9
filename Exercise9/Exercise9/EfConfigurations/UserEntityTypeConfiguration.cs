using Exercise9.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exercise9.EfConfigurations
{
    public class UserEntityTypeConfiguration: IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> opt)
        {
            opt.HasKey(e => e.IdUser);
            opt.Property(e => e.IdUser).ValueGeneratedOnAdd();
            opt.Property(e => e.RefreshTokenExp).HasColumnType(typeName: "datetime2");
        }
    }
}
