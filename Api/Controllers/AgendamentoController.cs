using Core.Input;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Core.Entity;
using Hackathon.Api.Dto;

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

    }
}
