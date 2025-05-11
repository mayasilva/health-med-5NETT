using Core.Entity;

namespace Core.Repository
{
    public interface IAgendaRepository
    {
        IList<Agenda> ObterTodos();
        IList<Agenda> ObterDisponiveis(string crm);
        
        void Cadastrar(Agenda entidade);
        void Deletar(int id);
    }
}
