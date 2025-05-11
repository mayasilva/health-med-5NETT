using Core.Input;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Core.Entity;
using Hackathon.Api.Dto;
using System.Security.Claims;
using MassTransit.Futures.Contracts;

namespace Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IBus _bus;

        public AgendamentoController(IAgendamentoRepository agendamentoRepository, IBus bus)
        {
            _agendamentoRepository = agendamentoRepository;
            _bus = bus;
        }
        protected string GetCurrentUserId() => User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        protected string GetCurrentUserType() => User.Claims.FirstOrDefault(c => c.Type =="tipoUsuario")?.Value;


        /// <summary>
        /// Necessita de autenticação via token para cadastrar um novo agendamento
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        /// {
        ///     "Nome": "Nome do Agendamento",
        ///     "DDD": "DDD (Região) do telefone",
        ///     "Telefone": "Telefone",
        ///     "Email": "Email"
        /// }
        /// 
        /// Observação: Não é necessário informar o Id
        /// </remarks>
        /// <param name="input">Objeto do AgendamentoInclusaoInput</param>
        /// <returns>Retorna sucesso ou erro na inclusão do agendamento</returns>
        /// <response code="200">Sucesso na execução da inclusão do agendamento na fila</response>
        /// <response code="400">Dados inválidos ou erro na requisição</response>
        /// <response code="401">Token inválido</response>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AgendamentoInclusaoInput input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var endpoint = await _bus.GetSendEndpoint(new Uri("queue:FilaCadastroAgendamento"));
                    await endpoint.Send(input);
                    return Ok();
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }


        
        /// <summary>
        /// Necessita de autenticação via token para cadastrar um novo agendamento
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        /// {
        ///     "IdAgendamento": "1",
        ///     "Justificativa": "Justificativa do cancelamento",
        /// }
        /// 
        /// </remarks>
        /// <param name="input">Objeto do AgendamentoCancelamentoInput</param>
        /// <returns>Retorna sucesso ou erro no cancelamento do agendamento</returns>
        /// <response code="200">Sucesso na execução do cancelamento do agendamento na fila</response>
        /// <response code="400">Dados inválidos ou erro na requisição</response>
        /// <response code="401">Token inválido</response>
        [Authorize]
        [HttpPost("cancelar")]
        public async Task<IActionResult> Post([FromBody] AgendamentoCancelamentoInput input)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var userId = GetCurrentUserId();
                    var userType = GetCurrentUserType();

                    var agendamento = _agendamentoRepository.ObterPorId(input.IdAgendamento);
                    if (agendamento == null)
                        return NotFound();

                    if (userType != "paciente" || agendamento.IdPaciente.ToString() != userId)
                        return Forbid();

                    var endpoint = await _bus.GetSendEndpoint(new Uri("queue:FilaCancelamentoAgendamento"));
                    await endpoint.Send(input);
                    return Ok();
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        /// <summary>
        /// Necessita de autenticação via token para confirmar um agendamento
        /// </summary>

        /// <param name="id">Id do agendamento</param>
        /// <returns>Retorna sucesso ou erro na confirmação do agendamento</returns>
        /// <response code="200">Sucesso na execução da confirmação do agendamento na fila</response>
        /// <response code="400">Dados inválidos ou erro na requisição</response>
        /// <response code="401">Token inválido</response>
        [Authorize]
        [HttpPost("{id:int}/confirmar")]
        public async Task<IActionResult> Post([FromRoute] int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var endpoint = await _bus.GetSendEndpoint(new Uri("queue:FilaConfirmacaoAgendamento"));
                    await endpoint.Send(new IdMessage { Id = id });
                    return Ok();
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        /// <summary>
        /// Necessita de autenticação via token para recusar um agendamento
        /// </summary>

        /// <param name="id">Id do agendamento</param>
        /// <returns>Retorna sucesso ou erro na recusa do agendamento</returns>
        /// <response code="200">Sucesso na execução da recusa do agendamento na fila</response>
        /// <response code="400">Dados inválidos ou erro na requisição</response>
        /// <response code="401">Token inválido</response>
        [Authorize]
        [HttpPost("{id:int}/recusar")]
        public async Task<IActionResult> Recusar([FromRoute] int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var endpoint = await _bus.GetSendEndpoint(new Uri("queue:FilaRecusaAgendamento"));
                    await endpoint.Send(new IdMessage { Id = id });
                    return Ok();
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [Authorize]
        [HttpGet("medico/{idMedico:int}")]
        public IActionResult ObterDoMedico([FromRoute] int idMedico)
        {
            try
            {
                var agendamentos = _agendamentoRepository.ObterPorIdMedico(idMedico);
                if (agendamentos == null)
                {
                    return NotFound();
                }

                return Ok(agendamentos.Select(a => new AgendamentoDto(a)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("medico/{idMedico:int}/pendentes")]
        public IActionResult ObterPendentesDoMedico([FromRoute] int idMedico)
        {
            try
            {
                var agendamentos = _agendamentoRepository.ObterPendentesDoMedico(idMedico);
                if (agendamentos == null)
                {
                    return NotFound();
                }

                return Ok(agendamentos.Select(a => new AgendamentoDto(a)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("paciente/{idPaciente:int}")]
        public IActionResult ObterDoPaciente([FromRoute] int idPaciente)
        {
            try
            {
                var agendamentos = _agendamentoRepository.ObterPorIdPaciente(idPaciente);
                if (agendamentos == null)
                {
                    return NotFound();
                }

                return Ok(agendamentos.Select(a => new AgendamentoDto(a)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        

    }
}
