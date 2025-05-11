using Core.Entity;
using Core.Repository;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repository
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        private readonly ApplicationDbContext _context;
        

        public AgendamentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        void IAgendamentoRepository.Cadastrar(Agendamento entidade)
        {
            _context.Set<Agendamento>().Add(entidade);
            _context.SaveChanges();
        }

        void IAgendamentoRepository.Alterar(Agendamento entidade)
        {
            _context.Set<Agendamento>().Update(entidade);
            _context.SaveChanges();
        }

        public void Cancelar(int id, string justificativa)
        {
            var agendamento = _context.Set<Agendamento>().FirstOrDefault(a => a.Id == id);
            if (agendamento == null)
            {
                throw new KeyNotFoundException($"Agendamento com ID {id} não encontrado.");
            }

            agendamento.Justificativa = justificativa;
            agendamento.Status = Core.Utils.Enum.EStatus.Cancelado;
            _context.Set<Agendamento>().Update(agendamento);
            _context.SaveChanges();
        }

        IList<Agendamento> IAgendamentoRepository.ObterTodos()
            => _context.Set<Agendamento>().ToList();

        public IList<Agendamento> ObterPorIdMedico(int idMedico)
        {
            var agendamentos = _context.Set<Agendamento>()
                .Include(a => a.Agenda.Medico)
                .Include(a => a.Paciente)
                .Where(a => a.Agenda.IdMedico == idMedico)
                .ToList();

            return agendamentos;
        }

        public IList<Agendamento> ObterPorIdPaciente(int idPaciente)
        {
            var agendamentos = _context.Set<Agendamento>()
                .Where(a => a.Paciente.Id == idPaciente)
                .Include(a => a.Agenda.Medico)
                .Include(a => a.Paciente)
                .ToList();

            return agendamentos;
        }
    }
}