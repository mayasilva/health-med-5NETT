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
    }
}