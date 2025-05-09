using Core.Entity;

namespace Hackathon.Api.Dto
{
    public class AgendaDto
    {
        public int Id { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly Hora { get; set; }

        public AgendaDto(Agenda agenda)
        {
            Id = agenda.Id;
            Data = agenda.Data;
            Hora = agenda.Hora;
        }
    }
}