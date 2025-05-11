using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Agenda")]
    public class Agenda
    {
        public int Id { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly Hora { get; set; }
        public int IdMedico { get; set; }
        public Medico Medico { get; set; }

        public IList<Agendamento> Agendamentos { get; set; }

    }
}
