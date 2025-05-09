using Core.Entity;

namespace Core.Repository
{
    public interface IPacienteRepository
    {
        Paciente ObterPorCpf(string cpf);
        Paciente ObterPorId(int id);
        Paciente ObterPorEmail(string email);
    }
}
