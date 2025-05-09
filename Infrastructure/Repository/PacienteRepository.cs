using Core.Entity;
using Core.Repository;
using Infrastructure.Repository;

namespace Hackathon.Infrastructure.Repository
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly ApplicationDbContext _context;
        

        public PacienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Paciente IPacienteRepository.ObterPorCpf(string cpf)
        => _context.Set<Paciente>().FirstOrDefault(entity => entity.Cpf == cpf);

        Paciente IPacienteRepository.ObterPorEmail(string email)
        => _context.Set<Paciente>().FirstOrDefault(entity => entity.Email == email);

        Paciente IPacienteRepository.ObterPorId(int id)
        => _context.Set<Paciente>().FirstOrDefault(entity => entity.Id == id);
    }
}