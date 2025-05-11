using Core.Entity;

namespace Core.Repository
{
    public interface IAgendamentoRepository
    {
        IList<Agendamento> ObterTodos();  
        void Cadastrar(Agendamento entidade);
        void Alterar(Agendamento entidade);
        void Cancelar(int id, string justificativa); 
    }
}
