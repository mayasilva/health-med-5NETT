using Core.Input;
using Core.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Core.Entity;

namespace Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IBus _bus;

        public MedicoController(IMedicoRepository medicoRepository, IBus bus)
        {
            _medicoRepository = medicoRepository;
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
        public IActionResult Get()
        {

            try
            {
                return Ok(_medicoRepository.ObterTodos());
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
        public IActionResult Get([FromRoute] string crm)
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
        public async Task<IActionResult> Post([FromBody] ContatoInput input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var endpoint = await _bus.GetSendEndpoint(new Uri("queue:FilaCadasto"));
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
        /// Necessita de autenticação via token para alterar um contato
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        /// {
        ///     "Id": "Id do Contato"
        ///     "Nome": "Nome do Contato",
        ///     "DDD": "DDD (Região) do telefone do contato",
        ///     "Telefone": "Telefone do Contato",
        ///     "Email": "Email do Contato",
        /// }
        /// 
        /// </remarks>
        /// <param name="input">Objeto de ContatoUpdate</param>
        /// <returns>Contato alterado</returns>
        /// <response code="200">Sucesso na inclusão da alteração do contato na fila-alteracao</response>
        /// <response code="500">Não foi possivel alterar o contato</response>
        /// <response code="401">Token inválido</response>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ContatoUpdateInput input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var endpoint = await _bus.GetSendEndpoint(new Uri("queue:FilaAlteracao"));
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
        /// Necessita de autenticação via token para excluir um contato da base de dados
        /// </summary>
        /// <param name="id">Id do contato</param>
        /// <returns></returns>
        /// <response code="200">Sucesso ao inclir o contato para exclusão na fila-exclusao</response>
        /// <response code="500">Não foi possivel excluir o contato</response>
        /// <response code="401">Token inválido</response>
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var endpoint = await _bus.GetSendEndpoint(new Uri("queue:FilaExclusao"));
                await endpoint.Send(new IdMessage { Id = id});
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
