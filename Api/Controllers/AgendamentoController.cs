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
        /// Necessita de autenticação via token para cadastrar um novo contato
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        /// {
        ///     "Nome": "Nome do Contato",
        ///     "DDD": "DDD (Região) do telefone do contato",
        ///     "Telefone": "Telefone do Contato",
        ///     "Email": "Email do Contato",
        /// }
        /// 
        /// Observação: Não é necessario informar o Id
        /// </remarks>
        /// <param name="input">Objeto do ContatoInput</param>
        /// <returns>Retorna Contato cadastrado</returns>
        /// <response code="200">Sucesso na execução da inclusão do contato na fila-cadastro</response>
        /// <response code="500">Não foi possivel incluir um novo contato</response>
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

    }
}
