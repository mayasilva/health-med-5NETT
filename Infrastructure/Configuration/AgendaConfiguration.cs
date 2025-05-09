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

            builder.Property(p => p.Data).HasColumnType("DATE").IsRequired();
            builder.Property(p => p.Hora).HasColumnType("TIME").IsRequired();
            builder.Property(p => p.IdMedico).HasColumnType("INT").IsRequired();

            builder.HasOne(p => p.Medico)
                .WithMany(a => a.Agendas)
                .HasForeignKey(a => a.IdMedico)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
