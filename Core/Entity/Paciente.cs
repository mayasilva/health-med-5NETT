using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Paciente")]
    public class Paciente
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Cpf { get; set; } = "";
        public string Email { get; set; } = "";
        public string Senha { get; set; } = "";
        public IList<Agendamento> Agendamentos {  get; set; } 
    }
}
