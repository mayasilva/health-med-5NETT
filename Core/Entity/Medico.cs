using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Cliente")]
    public class Medico
    {
        public int Id{ get; set; }
        public int Crm{ get; set; }
        public string Name { get; set; } = "";
        public string Cpf { get; set; } = "";
        public string Especialidade { get; set; } = "";
        public int ValorConsulta { get; set; }
        public ICollection<Agenda> Agendas { get; set; }

    }
}
