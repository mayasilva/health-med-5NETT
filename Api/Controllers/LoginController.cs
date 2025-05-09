using Core.Input;
using Core.Repository;
using Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TechChallangeCadastroContatosAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IMedicoRepository _medicoRepository;

        public LoginController(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }
        /// <summary>
        /// Gerar token de autenticação para o medico
        /// </summary>
        /// <param name="loginInput">usuário e senha</param>
        /// <returns>token de autenticação</returns>
        [HttpPost("medico")]
        public IActionResult Post([FromBody] LoginInput loginInput)
        {
            if (ModelState.IsValid)
            {
                var medico = _medicoRepository.ObterPorCrm(loginInput.Crm);
                if (medico != null && medico.Senha.Equals(loginInput.Senha))
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Utils.CHAVE_TOKEN));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var Sectoken = new JwtSecurityToken(null,
                      null,
                      null,
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
