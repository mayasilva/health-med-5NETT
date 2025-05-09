using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Medico")]
    public class Medico
    {
        public int Id{ get; set; }
        public string Crm{ get; set; } = "";
        public string Name { get; set; } = "";
        public string Cpf { get; set; } = "";

        public string Senha { get; set; } = "";
        public string Especialidade { get; set; } = "";
        public int ValorConsulta { get; set; }
        public ICollection<Agenda> Agendas { get; set; }

    }
}
