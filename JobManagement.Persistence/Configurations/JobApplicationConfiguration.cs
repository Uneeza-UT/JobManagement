using JobManagement.Domain;
using JobManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace JobManagement.Persistence.Configurations
{
    public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
    {
        public void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            // Prevent a user from submitting multiple applications
            // for the same job.

            builder.HasIndex(q => new { q.JobId, q.ApplicantId })
                .IsUnique();


            // Store the applicant status and gender enums as strings 
            // instead of numeric value for database readability.

            builder.Property(q => q.Status)
                .HasConversion<string>()
                .HasDefaultValue(JobApplicationStatus.Applied);

            builder.Property(q => q.Gender)
                .HasConversion<string>();


            // Configure the entity properties to have reasonable length limits to prevent excessively large values.

            builder.Property(q => q.FirstName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(q => q.LastName)
               .IsRequired()
               .HasMaxLength(200);

            builder.Property(q => q.Email)
               .IsRequired()
               .HasMaxLength(500);

            builder.Property(q => q.PhoneNumber)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(q => q.Address)
               .IsRequired()
               .HasMaxLength(200);

            builder.Property(q => q.CNIC)
               .HasMaxLength(100);

        }
    }
}
