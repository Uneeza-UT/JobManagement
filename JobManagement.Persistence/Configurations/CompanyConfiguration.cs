using JobManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManagement.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {

            builder.Property(q => q.Email)
                .HasMaxLength(500);

            builder.Property(q => q.PhoneNumber)
                .HasMaxLength(100);
        }
    }
}
