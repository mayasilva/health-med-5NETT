using Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Configurations
{
    public class AgendaConfiguration : IEntityTypeConfiguration<Agenda>
    {
        public void Configure(EntityTypeBuilder<Agenda> builder)
        {
            builder.ToTable("agenda");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Data).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.Hora).HasColumnType("VARCHAR(11)").IsRequired();
            builder.Property(p => p.IdMedico).HasColumnType("INT").IsRequired();
            builder.Property(p => p.IdPaciente).HasColumnType("INT").IsRequired();
            builder.Property(p => p.Confirmado).HasColumnType("VARCHAR(11)").IsRequired();

            builder.HasOne(p => p.Medico)
                .WithMany(a => a.Agendas)
                .HasForeignKey(a => a.Id);

            builder.HasOne(p => p.Paciente)
                .WithMany(a => a.Agendas)
                .HasForeignKey(a => a.Id);
        }
    }
}
