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

        void IAgendaRepository.Cadastrar(Agenda entidade)
        {
            _context.Set<Agenda>().Add(entidade);
            _context.SaveChanges();
        }

        IList<Agenda> IAgendaRepository.ObterTodos()
            => _context.Set<Agenda>().ToList();
    }
}