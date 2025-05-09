using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Agenda")]
    public class Agenda
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public int IdMedico { get; set; }
        public int IdPaciente { get; set; }
        public Medico Medico { get; set; }
        public Paciente Paciente { get; set; }
        public string Confirmado { get; set; }

    }
}
