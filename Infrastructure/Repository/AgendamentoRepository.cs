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

        IList<Agendamento> IAgendamentoRepository.ObterTodos()
            => _context.Set<Agendamento>().ToList();
    }
}