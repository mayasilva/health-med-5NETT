using Core.Entity;

namespace Core.Repository
{
    public interface IMedicoRepository
    {
        IList<Medico> ObterTodos(string? especialidade);
        Medico ObterPorCrm(string crm);
        Medico? ObterAgendaPorCrm (string crm);
        void Cadastrar(Medico entidade);
        void Alterar(Medico entidade);
        void Deletar(string crm);
    }
}
