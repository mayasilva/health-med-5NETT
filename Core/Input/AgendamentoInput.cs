using Core.Utils.Enum;

namespace Core.Input
{
    public class AgendamentoInclusaoInput
    {
        public int IdPaciente { get; set; }
        public int IdAgenda { get; set; }
    }

    public class AgendamentoCancelamentoInput
    {
        public int IdAgendamento { get; set; }
        public required string Justificativa { get; set; }
    }
}