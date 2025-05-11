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
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IAgendaRepository _agendaRepository;
        private readonly IBus _bus;

        public MedicoController(IMedicoRepository medicoRepository, IAgendaRepository agendaRepository, IBus bus)
        {
            _medicoRepository = medicoRepository;
            _agendaRepository = agendaRepository;
            _bus = bus;
        }

        /// <summary>
        /// Necessita de autenticação via token para retorno de todos os contatos
        /// </summary>
        /// <returns>Retorna uma lista de contato</returns>
        /// <response code="200">Sucesso na execução ao retornar os contatos</response>
        /// <response code="500">Não foi possivel retornar as informações dos contatos</response>
        /// <response code="401">Token inválido</response>
        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery] string? especialidade)
        {
            try
            {
                var medicos = _medicoRepository.ObterTodos(especialidade);
    
                return Ok(medicos.Select(m => new MedicoDto(m)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Necessita de autenticação via token para retorno o contato por Id
        /// </summary>
        /// <param name="crm"></param>
        /// <returns>Retorna um Contato filtrado pelo Id</returns>
        /// <response code="200">Sucesso na execução ao retornar do contato</response>
        /// <response code="500">Não foi possivel retornar as informações do contato</response>
        /// <response code="401">Token inválido</response>
        [Authorize]
        [HttpGet("PorCrm/{crm}")]
        public IActionResult ObterPorCrm([FromRoute] string crm)
        {

            try
            {
                return Ok(_medicoRepository.ObterPorCrm(crm));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        /// <summary>
        /// Necessita de autenticação via token para retornar a agenda do médico
        /// </summary>
        /// <param name="crm">CRM do médico</param>
        /// <returns>Retorna a agenda do médico</returns>
        /// <response code="200">Sucesso ao retornar a agenda</response>
        /// <response code="500">Não foi possível retornar a agenda</response>
        /// <response code="401">Token inválido</response>
        [Authorize]
        [HttpGet("{crm}/agenda")]
        public IActionResult GetAgenda([FromRoute] string crm)
        {
            try
            {
                var agendas = _agendaRepository.ObterDisponiveis(crm);
                if (agendas  == null)
                {
                    return NotFound("Agenda não encontrada para o médico informado.");
                }

                return Ok(agendas.Select(a => new AgendaDto(a)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Necessita de autenticação via token para excluir uma agenda
        /// </summary>
        /// <param name="id">Id do contato</param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao inclir o contato para exclusão na fila-exclusao</response>
        /// <response code="500">Não foi possivel excluir o contato</response>
        /// <response code="401">Token inválido</response>
        [Authorize]
        [HttpDelete("{crm}/agenda/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var endpoint = await _bus.GetSendEndpoint(new Uri("queue:FilaExclusaoAgenda"));
                await endpoint.Send(new IdMessage { Id = id });
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize]
        [HttpPost("Agenda")]
        public async Task<IActionResult> CriarAgenda([FromBody] AgendaInput input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

                    var endpoint = await _bus.GetSendEndpoint(new Uri("queue:FilaAgenda"));
                    await endpoint.Send(input);

                    Console.WriteLine($"Agendamento criado: {input.Data} {input.Hora} para o médico com CRM {input.IdMedico}");

                    return Ok("Agenda criada com sucesso.");
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
