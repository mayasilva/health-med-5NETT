using Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Configurations
{
    public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.ToTable("agendamento");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Status).HasColumnType("VARCHAR(10)").IsRequired();
            builder.Property(p => p.IdAgenda).HasColumnType("INT").IsRequired();
            builder.Property(p => p.IdPaciente).HasColumnType("INT").IsRequired();

            builder.HasOne(p => p.Agenda)
                .WithMany(a => a.Agendamentos)
                .HasForeignKey(p => p.IdAgenda);

            builder.HasOne(p => p.Paciente)
                .WithMany(a => a.Agendamentos)
                .HasForeignKey(a => a.IdPaciente)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
