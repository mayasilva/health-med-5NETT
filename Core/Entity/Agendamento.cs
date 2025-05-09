using Core.Utils.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Agendamento")]
    public class Agendamento
    {
        public int Id { get; set; }
        public EStatus Status { get; set; }
        public int IdAgenda { get; set; }
        public int IdPaciente { get; set; }
        public Agenda Agenda { get; set; }
        public Paciente Paciente { get; set; }


    }
}
