using JobManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManagement.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasIndex(x => x.Email)
                .IsUnique();


            // Configure the entity properties to have reasonable length limits to prevent excessively large values.

            builder.Property(q => q.Name)
                .HasMaxLength(500);

            builder.Property(q => q.Email)
                .HasMaxLength(500);

            builder.Property(q => q.PhoneNumber)
                .HasMaxLength(100);

            builder.Property(q => q.Country)
                .HasMaxLength(100);

            builder.Property(q => q.City)
                .HasMaxLength(100);

            builder.Property(q => q.Address)
                .HasMaxLength(200);
        }
    }
}
