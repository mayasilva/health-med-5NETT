using Core.Entity;
using Core.Input;
using Core.Repository;
using MassTransit;

namespace Consumidor.Eventos
{
    public class CancelamentoAgendamentoConsumidor : IConsumer<AgendamentoCancelamentoInput>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public CancelamentoAgendamentoConsumidor(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }
        public Task Consume(ConsumeContext<AgendamentoCancelamentoInput> context)
        {
            _agendamentoRepository.Cancelar(context.Message.IdAgendamento, context.Message.Justificativa);

            return Task.CompletedTask;
        }
    }
}
