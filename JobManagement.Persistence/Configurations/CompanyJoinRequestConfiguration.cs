using JobManagement.Domain;
using JobManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManagement.Persistence.Configurations
{
    public class CompanyJoinRequestConfiguration : IEntityTypeConfiguration<CompanyJoinRequest>
    {
        public void Configure(EntityTypeBuilder<CompanyJoinRequest> builder)
        {

            // Store the join request status as strings 
            // instead of numeric value for database readability.

            builder.Property(q => q.Status)
                .HasConversion<string>()
                .HasDefaultValue(JoinRequestStatus.Pending);


        }
    }
}
