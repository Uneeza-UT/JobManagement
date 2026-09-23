using JobManagement.Domain;
using JobManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace JobManagement.Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {

            // Store the enums approval status, job type, and employment type as strings
            // instead of their numeric values for database readability.

            builder.Property(q => q.ApprovalStatus)
                .HasConversion<string>()
                .HasDefaultValue(JobApprovalStatus.Pending);


            builder.Property(q => q.JobType)
                .HasConversion<string>();


            builder.Property(q => q.EmploymentType)
                .HasConversion<string>();


            // Configure the entity properties to have reasonable length limits to prevent excessively large values.


            builder.Property(q => q.Location)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(q => q.Salary)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
