﻿using System.ComponentModel.DataAnnotations;

namespace Core.Input
{
    public class LoginInput
    {
        [Required(ErrorMessage = "Campo usuário é obrigatório")]
        [DeniedValues("string", ErrorMessage = "Texto 'string' não é permitido")]
        public required string Crm { get; set; }

        [Required(ErrorMessage = "Campo senha é obrigatório")]
        [DeniedValues("string", ErrorMessage = "Texto 'string' não é permitido")]
        public required string Senha { get; set; }
    }

    public class LoginPacienteInput
    {
        [Required(ErrorMessage = "Campo cpf é obrigatório")]
        [DeniedValues("string", ErrorMessage = "Texto 'string' não é permitido")]
        public required string Cpf { get; set; }

        [Required(ErrorMessage = "Campo senha é obrigatório")]
        [DeniedValues("string", ErrorMessage = "Texto 'string' não é permitido")]
        public required string Senha { get; set; }
    }
}
