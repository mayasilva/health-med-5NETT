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
                throw new KeyNotFoundException($"Agendamento com ID {id} n�o encontrado.");
            }

            agendamento.Justificativa = justificativa;
            agendamento.Status = Core.Utils.Enum.EStatus.Cancelado;
            _context.Set<Agendamento>().Update(agendamento);
            _context.SaveChanges();
        }

        public void Confirmar(int id)
        {
            var agendamento = _context.Set<Agendamento>().FirstOrDefault(a => a.Id == id);
            if (agendamento == null)
            {
                throw new KeyNotFoundException($"Agendamento com ID {id} n�o encontrado.");
            }

            if (agendamento.Status != Core.Utils.Enum.EStatus.Pendente)
            {
                throw new InvalidOperationException($"Agendamento com ID {id} n�o est� pendente.");
            }

            agendamento.Status = Core.Utils.Enum.EStatus.Confirmado;
            _context.Set<Agendamento>().Update(agendamento);
            _context.SaveChanges();
        }

        IList<Agendamento> IAgendamentoRepository.ObterTodos()
            => _context.Set<Agendamento>().ToList();

        public Agendamento? ObterPorId(int id) => 
            _context.Set<Agendamento>()
                .FirstOrDefault(a => a.Id == id);

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

        public IList<Agendamento> ObterPendentesDoMedico(int idMedico)
        {
             var agendamentos = _context.Set<Agendamento>()
                .Include(a => a.Agenda.Medico)
                .Include(a => a.Paciente)
                .Where(a => a.Agenda.IdMedico == idMedico)
                .Where(a => a.Status == Core.Utils.Enum.EStatus.Pendente)
                .ToList();

            return agendamentos;
        }

        public void Recusar(int id)
        {
            var agendamento = _context.Set<Agendamento>().FirstOrDefault(a => a.Id == id);
            if (agendamento == null)
            {
                throw new KeyNotFoundException($"Agendamento com ID {id} n�o encontrado.");
            }

            agendamento.Status = Core.Utils.Enum.EStatus.Recusado;
            _context.Set<Agendamento>().Update(agendamento);
            _context.SaveChanges();
        }
    }
}