﻿using Core.Entity;

namespace Core.Repository
{
    public interface IAgendamentoRepository
    {
        IList<Agendamento> ObterTodos();  
        Agendamento? ObterPorId(int id);
        IList<Agendamento> ObterPorIdMedico(int idMedico);
        IList<Agendamento> ObterPendentesDoMedico(int idMedico);
        IList<Agendamento> ObterPorIdPaciente(int idPaciente);
        void Cadastrar(Agendamento entidade);
        void Alterar(Agendamento entidade);
        void Cancelar(int id, string justificativa); 
        void Confirmar(int id);
        void Recusar(int id);
    }
}
