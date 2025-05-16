using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Infra.Repositories;
using DDDUniversidadeBroadcast.Service.Interfaces;
using DDDUniversidadeBroadcast.Service.Services;
using RabbitMQ.Subscriber;

namespace DDDUniversidadeBroadcast.API.Extensions
{
    internal static class ServiceExtension
    {
        internal static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IEventoService, EventoService>();
            services.AddTransient<IPostagemService, PostagemService>();
            services.AddTransient<ISeguidorService, SeguidorService>();
            services.AddTransient<IParticipanteService, ParticipanteService>();

            return services;
        }
    }
}
