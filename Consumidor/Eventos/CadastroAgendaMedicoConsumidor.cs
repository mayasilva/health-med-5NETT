using Core.Entity;
using Core.Input;
using Core.Repository;
using MassTransit;

namespace Consumidor.Eventos
{
    public class CadastroAgendaMedicoConsumidor : IConsumer<AgendaInput>
    {
        private readonly IAgendaRepository _agendaRepository;

        public CadastroAgendaMedicoConsumidor(IAgendaRepository agendaRepository)
        {
            _agendaRepository = agendaRepository;
        }
        public Task Consume(ConsumeContext<AgendaInput> context)
        {
            var agenda = new Agenda()
            {
                Data = context.Message.Data,
                Hora = context.Message.Hora,
                IdMedico = context.Message.IdMedico
            };
            _agendaRepository.Cadastrar(agenda);

            return Task.CompletedTask;
        }
    }
}
