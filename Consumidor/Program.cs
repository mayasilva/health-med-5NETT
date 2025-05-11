using Consumidor;
using Consumidor.Eventos;
using Core.Repository;
using Hackathon.Infrastructure.Repository;
using Infrastructure.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var filaAgenda = configuration.GetSection("MassTransit")["FilaAgenda"] ?? string.Empty;
var filaExclusaoAgenda = configuration.GetSection("MassTransit")["FilaExclusaoAgenda"] ?? string.Empty;
var filaCadastroAgendamento = configuration.GetSection("MassTransit")["FilaCadastroAgendamento"] ?? string.Empty;
var filaCancelamentoAgendamento = configuration.GetSection("MassTransit")["FilaCancelamentoAgendamento"] ?? string.Empty;
var filaConfirmacaoAgendamento = configuration.GetSection("MassTransit")["FilaConfirmacaoAgendamento"] ?? string.Empty;
var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<IAgendaRepository, AgendaRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(servidor, "/", h =>
        {
            h.Username(usuario);
            h.Password(senha);
        });
        
        cfg.ReceiveEndpoint(filaAgenda, e =>
        {
            e.Consumer<CadastroAgendaMedicoConsumidor>(context);
        });

        cfg.ReceiveEndpoint(filaExclusaoAgenda, e =>
        {
            e.Consumer<ExclusaoAgendaMedicoConsumidor>(context);
        });

        cfg.ReceiveEndpoint(filaCadastroAgendamento, e =>
        {
            e.Consumer<CadastroAgendamentoConsumidor>(context);
        });

        cfg.ReceiveEndpoint(filaCancelamentoAgendamento, e =>
        {
            e.Consumer<CancelamentoAgendamentoConsumidor>(context);
        });

        cfg.ReceiveEndpoint(filaConfirmacaoAgendamento, e =>
        {
            e.Consumer<ConfirmacaoAgendamentoConsumidor>(context);
        });

        cfg.ConfigureEndpoints(context);
    });

    x.AddConsumer<CadastroAgendaMedicoConsumidor>();
    x.AddConsumer<ExclusaoAgendaMedicoConsumidor>();
    x.AddConsumer<CadastroAgendamentoConsumidor>();
    x.AddConsumer<CancelamentoAgendamentoConsumidor>();
    x.AddConsumer<ConfirmacaoAgendamentoConsumidor>();
});



var host = builder.Build();
host.Run();
