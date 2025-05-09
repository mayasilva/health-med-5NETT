using Core.Entity;

namespace Core.Repository
{
    public interface IAgendaRepository
    {
        IList<Agenda> ObterTodos();
        
        void Cadastrar(Agenda entidade);
        void Deletar(int id);
    }
}
