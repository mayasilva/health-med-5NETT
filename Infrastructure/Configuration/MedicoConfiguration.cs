using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entity;

namespace Infrastructure.Repository.Configurations
{
    public class MedicoConfiguration : IEntityTypeConfiguration<Medico>
    {
        public void Configure(EntityTypeBuilder<Medico> builder)
        {
            builder.ToTable("medico");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Crm).HasColumnType("VARCHAR(6)").IsRequired();
            builder.Property(p => p.Name).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.Cpf).HasColumnType("VARCHAR(11)").IsRequired();
            builder.Property(p => p.Especialidade).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.ValorConsulta).HasColumnType("INT").IsRequired();
        }
    }
}
