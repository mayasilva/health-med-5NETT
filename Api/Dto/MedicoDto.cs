using Core.Entity;

namespace Hackathon.Api.Dto
{
    public class MedicoDto
    {
        public int Id { get; set; }
        public string Crm { get; set; }
        public string Name { get; set; }
        public int ValorConsulta { get; set; }
        public string Especialidade { get; set; }

        public MedicoDto(Medico medico)
        {
            Id = medico.Id;
            Crm = medico.Crm;
            Name = medico.Name;
            ValorConsulta = medico.ValorConsulta;
            Especialidade = medico.Especialidade;
        }
    }
}