using Core.Entity;
using Core.Repository;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Api.Controllers;
using Hackathon.Api.Dto;

namespace Test.Unitario
{
    public class MedicoControllerTest
    {
        [Fact]
        public void Get_DeveRetornar200ComUmaListaVazia_QuandoRepositoryDevolverVazio()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            mockMedicoRepository.Setup(repository => repository.ObterTodos(null)).Returns(new List<Medico>());
            var mockBus = new Mock<IBus>();


            var medicoController = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);

            var resultado = Assert.IsType<OkObjectResult>(medicoController.Get(null));
            Assert.NotNull(resultado);
            Assert.Equal(StatusCodes.Status200OK, resultado.StatusCode);
            var valor = Assert.IsType<List<MedicoDto>>(resultado.Value);
            Assert.Empty(valor);
        }

        [Fact]
        public void Get_DeveRetornarBadRequest_QuandoDerUmaExceção()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            mockMedicoRepository.Setup(repository => repository.ObterTodos(null)).Throws<System.IO.IOException>();
            var mockBus = new Mock<IBus>();

            var medicoController = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);

            var resultado = Assert.IsType<BadRequestObjectResult>(medicoController.Get(null));
            Assert.Equal(StatusCodes.Status400BadRequest, resultado.StatusCode);

        }

        [Fact]
        public void ObterPorCrm_DeveRetornar200_QuandoEncontrarMedico()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            var mockBus = new Mock<IBus>();
            var medico = new Medico { Crm = "123" };
            mockMedicoRepository.Setup(r => r.ObterPorCrm("123")).Returns(medico);

            var controller = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);

            var resultado = Assert.IsType<OkObjectResult>(controller.ObterPorCrm("123"));
            Assert.Equal(StatusCodes.Status200OK, resultado.StatusCode);
            Assert.Equal(medico, resultado.Value);
        }

        [Fact]
        public void ObterPorCrm_DeveRetornarBadRequest_QuandoRepositoryLancarExcecao()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            var mockBus = new Mock<IBus>();
            mockMedicoRepository.Setup(r => r.ObterPorCrm(It.IsAny<string>())).Throws(new Exception("erro"));

            var controller = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);

            var resultado = Assert.IsType<BadRequestObjectResult>(controller.ObterPorCrm("123"));
            Assert.Equal(StatusCodes.Status400BadRequest, resultado.StatusCode);
        }

        [Fact]
        public void GetAgenda_DeveRetornarOk_QuandoAgendasExistem()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            var mockBus = new Mock<IBus>();
            var agendas = new List<Agenda> { new Agenda() };
            mockAgendaRepository.Setup(r => r.ObterDisponiveis("crm1")).Returns(agendas);

            var controller = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);

            var resultado = Assert.IsType<OkObjectResult>(controller.GetAgenda("crm1"));
            Assert.Equal(StatusCodes.Status200OK, resultado.StatusCode);
        }

        [Fact]
        public void GetAgenda_DeveRetornarBadRequest_QuandoRepositoryLancarExcecao()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            var mockBus = new Mock<IBus>();
            mockAgendaRepository.Setup(r => r.ObterDisponiveis(It.IsAny<string>())).Throws(new Exception("erro"));

            var controller = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);

            var resultado = Assert.IsType<BadRequestObjectResult>(controller.GetAgenda("crm1"));
            Assert.Equal(StatusCodes.Status400BadRequest, resultado.StatusCode);
        }

        [Fact]
        public async Task Delete_DeveRetornarOk_QuandoSucesso()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            var mockBus = new Mock<IBus>();
            var mockEndpoint = new Mock<ISendEndpoint>();
            mockBus.Setup(b => b.GetSendEndpoint(It.IsAny<Uri>())).ReturnsAsync(mockEndpoint.Object);

            var controller = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);

            var resultado = await controller.Delete(1);
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public async Task Delete_DeveRetornarBadRequest_QuandoExcecao()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            var mockBus = new Mock<IBus>();
            mockBus.Setup(b => b.GetSendEndpoint(It.IsAny<Uri>())).ThrowsAsync(new Exception("erro"));

            var controller = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);

            var resultado = await controller.Delete(1);
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public async Task CriarAgenda_DeveRetornarOk_QuandoModelStateValido()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            var mockBus = new Mock<IBus>();
            var mockEndpoint = new Mock<ISendEndpoint>();
            mockBus.Setup(b => b.GetSendEndpoint(It.IsAny<Uri>())).ReturnsAsync(mockEndpoint.Object);

            var controller = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var input = new Core.Input.AgendaInput { IdMedico = 1, Data = new DateOnly(2025, 10, 1), Hora = new TimeOnly(10, 0) };

            var resultado = await controller.CriarAgenda(input);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact]
        public async Task CriarAgenda_DeveRetornarBadRequest_QuandoModelStateInvalido()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            var mockBus = new Mock<IBus>();
            var controller = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("erro", "erro");

            var input = new Core.Input.AgendaInput();

            var resultado = await controller.CriarAgenda(input);
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public async Task CriarAgenda_DeveRetornarBadRequest_QuandoExcecao()
        {
            var mockMedicoRepository = new Mock<IMedicoRepository>();
            var mockAgendaRepository = new Mock<IAgendaRepository>();
            var mockBus = new Mock<IBus>();
            mockBus.Setup(b => b.GetSendEndpoint(It.IsAny<Uri>())).ThrowsAsync(new Exception("erro"));

            var controller = new MedicoController(mockMedicoRepository.Object, mockAgendaRepository.Object, mockBus.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var input = new Core.Input.AgendaInput { IdMedico = 1, Data = new DateOnly(2025, 10, 1), Hora = new TimeOnly(10, 0) };

            var resultado = await controller.CriarAgenda(input);
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

    }
}
