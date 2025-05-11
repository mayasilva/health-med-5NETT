using Core.Entity;
using Core.Input;
using Core.Repository;
using MassTransit;

namespace Consumidor.Eventos
{
    public class ConfirmacaoAgendamentoConsumidor : IConsumer<IdMessage>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public ConfirmacaoAgendamentoConsumidor(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }
        public Task Consume(ConsumeContext<IdMessage> context)
        {
            _agendamentoRepository.Confirmar(context.Message.Id);

            return Task.CompletedTask;
        }
    }
}
