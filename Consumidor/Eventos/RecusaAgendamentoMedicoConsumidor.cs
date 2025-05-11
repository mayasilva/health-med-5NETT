using Core.Entity;
using Core.Input;
using Core.Repository;
using MassTransit;

namespace Consumidor.Eventos
{
    public class RecusaAgendamentoConsumidor : IConsumer<IdMessage>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public RecusaAgendamentoConsumidor(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }
        public Task Consume(ConsumeContext<IdMessage> context)
        {
            _agendamentoRepository.Recusar(context.Message.Id);

            return Task.CompletedTask;
        }
    }
}
