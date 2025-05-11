using Core.Input;
using Core.Repository;
using Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TechChallangeCadastroContatosAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IMedicoRepository _medicoRepository;
        private readonly IPacienteRepository _pacienteRepository;

        public LoginController( IMedicoRepository medicoRepository, 
                                IPacienteRepository pacienteRepository) 
        {
            _medicoRepository = medicoRepository;
            _pacienteRepository = pacienteRepository;
        }
        /// <summary>
        /// Gerar token de autenticação para o medico
        /// </summary>
        /// <param name="loginInput">usuário e senha</param>
        /// <returns>token de autenticação</returns>
        [HttpPost("medico")]
        public IActionResult LoginMedico([FromBody] LoginInput loginInput)
        {
            if (ModelState.IsValid)
            {
                var medico = _medicoRepository.ObterPorCrm(loginInput.Crm);
                if (medico != null && medico.Senha.Equals(loginInput.Senha))
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Utils.CHAVE_TOKEN));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                        new Claim("id", medico.Id.ToString()),
                        new Claim("tipoUsuario", "medico")
                    };

                    var Sectoken = new JwtSecurityToken(
                        null,
                        null,
                        claims,
                        expires: DateTime.Now.AddMinutes(120),
                        signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                    return Ok(token);
                }
                else
                {
                    return Unauthorized("Usuário ou senha incorretos");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }


        /// <summary>
        /// Gerar token de autenticação para o paciente
        /// </summary>
        /// <param name="loginInput">usuário e senha</param>
        /// <returns>token de autenticação</returns>
        [HttpPost("paciente")]
        public IActionResult LoginPaciente([FromBody] LoginPacienteInput loginInput)
        {
            if (ModelState.IsValid)
            {
                var paciente = _pacienteRepository.ObterPorCpf(loginInput.Cpf);
                if (paciente != null && paciente.Senha.Equals(loginInput.Senha))
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Utils.CHAVE_TOKEN));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                        new Claim("id", paciente.Id.ToString()),
                        new Claim("tipoUsuario", "paciente")
                    };

                    var Sectoken = new JwtSecurityToken(
                        null,
                        null,
                        claims,
                        expires: DateTime.Now.AddMinutes(120),
                        signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                    return Ok(token);
                }
                else
                {
                    return Unauthorized("Usuário ou senha incorretos");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
    }
}
