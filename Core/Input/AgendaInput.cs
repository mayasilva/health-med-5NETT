namespace Core.Input
{
    public class AgendaInput
    {
        public int IdMedico { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly Hora { get; set; }
    }
}