using Core.Entity;
using Core.Input;
using Core.Repository;
using MassTransit;

namespace Consumidor.Eventos
{
    public class ExclusaoAgendaMedicoConsumidor : IConsumer<IdMessage>
    {
        private readonly IAgendaRepository _agendaRepository;

        public ExclusaoAgendaMedicoConsumidor(IAgendaRepository agendaRepository)
        {
            _agendaRepository = agendaRepository;
        }
        public Task Consume(ConsumeContext<IdMessage> context)
        {
            
            _agendaRepository.Deletar(context.Message.Id);
            return Task.CompletedTask;
        }
    }
}
