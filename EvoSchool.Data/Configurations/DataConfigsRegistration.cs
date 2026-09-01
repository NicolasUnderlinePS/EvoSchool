using EvoSchool.Data.Repositories;
using EvoSchool.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSchool.Data.Configurations
{
    public static class DataConfigsRegistration
    {
        public static IServiceCollection AddDataInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration.GetConnectionString("DefaultConnection");
            var connectionString = "Server=localhost;Database=TesteEscola;Trusted_Connection=True;TrustServerCertificate=True;";

            services.AddScoped<IAlunoRepository>(provider => new AlunoRepository(connectionString));
            services.AddScoped<ITurmaRepository>(provider => new TurmaRepository(connectionString));
            services.AddScoped<IMatriculaRepository>(provider => new MatriculaRepository(connectionString));
            services.AddScoped<IRelatorioRepository>(provider => new RelatorioRepository(connectionString));

            return services;
        }
    }
}
