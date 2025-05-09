using Core.Entity;
using Core.Input;
using Core.Repository;
using MassTransit;

namespace Consumidor.Eventos
{
    public class CadastroAgendamentoConsumidor : IConsumer<AgendamentoInclusaoInput>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public CadastroAgendamentoConsumidor(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }
        public Task Consume(ConsumeContext<AgendamentoInclusaoInput> context)
        {
            var agendamento = new Agendamento()
            {
                IdAgenda = context.Message.IdAgenda,
                IdPaciente = context.Message.IdPaciente,
                Status = Core.Utils.Enum.EStatus.Pendente,
            };
            _agendamentoRepository.Cadastrar(agendamento);

            return Task.CompletedTask;
        }
    }
}
