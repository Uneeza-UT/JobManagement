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
       
            builder.HasIndex(q => new { q.JobId, q.ApplicationUserId })
                .IsUnique();

            builder.Property(q => q.Status)
                .HasConversion<string>()
                .HasDefaultValue(JobApplicationStatus.Applied);

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
      
        }
    }
}
