using Core.Entity;

namespace Hackathon.Api.Dto
{
    public class AgendamentoDto
    {
        public int Id { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly Hora { get; set; }
        public int IdMedico { get; set; }
        public int IdPaciente { get; set; }
        public string Paciente { get; set; }
        public string Medico { get; set; }
        public string Status { get; set; }
        public string Justificativa { get; set; }

        public AgendamentoDto(Agendamento agendamento)
        {
            Id = agendamento.Id;
            Data = agendamento.Agenda.Data;
            Hora = agendamento.Agenda.Hora;
            IdMedico = agendamento.Agenda.IdMedico;
            IdPaciente = agendamento.IdPaciente;
            Paciente = agendamento.Paciente.Name;
            Medico = agendamento.Agenda.Medico.Name;
            Status = agendamento.Status.ToString();
            Justificativa = agendamento.Justificativa ?? "";
        }
    }
    
}