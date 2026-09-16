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
            builder.Property(q => q.ApprovalStatus)
                .HasConversion<string>()
                .HasDefaultValue(JobApprovalStatus.Pending);

            builder.Property(q => q.Type)
                .IsRequired()
                .HasMaxLength(100);

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
