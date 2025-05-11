using Core.Entity;
using Core.Repository;
using Infrastructure.Repository;

namespace Hackathon.Infrastructure.Repository
{
    public class AgendaRepository : IAgendaRepository
    {
        private readonly ApplicationDbContext _context;
        

        public AgendaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Deletar(int id)
        {
            var agenda = _context.Set<Agenda>().FirstOrDefault(entity => entity.Id == id);
            if (agenda != null)
            {
                _context.Set<Agenda>().Remove(agenda);
                _context.SaveChanges();
            }
        }

        void IAgendaRepository.Cadastrar(Agenda entidade)
        {
            _context.Set<Agenda>().Add(entidade);
            _context.SaveChanges();
        }

        IList<Agenda> IAgendaRepository.ObterTodos()
            => _context.Set<Agenda>().ToList();

        public IList<Agenda> ObterDisponiveis(string crm)
        {
            return _context.Set<Agenda>()
            .Where(a => a.Medico.Crm == crm)
            .Where(a => a.Agendamentos.All(ag => ag.Status == Core.Utils.Enum.EStatus.Cancelado 
                || ag.Status == Core.Utils.Enum.EStatus.Recusado))
                .ToList();
        }

    }
}