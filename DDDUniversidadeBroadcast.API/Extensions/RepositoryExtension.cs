using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Infra.Repositories;
using DDDUniversidadeBroadcast.Service.Interfaces;
using DDDUniversidadeBroadcast.Service.Services;

namespace DDDUniversidadeBroadcast.API.Extensions
{
    internal static class RepositoryExtension
    {
        internal static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IEventoRepository, EventoRepository>();
            services.AddTransient<IPostagemRepository, PostagemRepository>();
            services.AddTransient<ISeguidorRepository, SeguidorRepository>();
            services.AddTransient<IParticipanteRepository, ParticipanteRepository>();

            return services;
        }
    }
}
