using EvoSchool.Data.Repositories;
using EvoSchool.Domain.Interfaces.Repositories;
using EvoSchool.Service.Interfaces.Services;
using EvoSchool.Service.Services;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Configuration;
using System.Web.Http;

namespace EvoSchool.Api.App_Start
{
    public static class SimpleInjectorWebApiInitializer
    {
        public static void Initialize(HttpConfiguration config)
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            RegistrarServicos(container);

            container.RegisterWebApiControllers(config);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void RegistrarServicos(Container container)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

            container.Register<IAlunoRepository>(() => new AlunoRepository(connectionString), Lifestyle.Scoped);
            container.Register<ITurmaRepository>(() => new TurmaRepository(connectionString), Lifestyle.Scoped);
            container.Register<IMatriculaRepository>(() => new MatriculaRepository(connectionString), Lifestyle.Scoped);
            container.Register<IRelatorioRepository>(() => new RelatorioRepository(connectionString), Lifestyle.Scoped);

            container.Register<IAlunoService, AlunoService>(Lifestyle.Scoped);           
            container.Register<ITurmaService, TurmaService>(Lifestyle.Scoped);           
            container.Register<IRelatorioService, RelatorioService>(Lifestyle.Scoped);           
            //container.Register<IMatriculaService, MatriculaService>(Lifestyle.Scoped);
            container.Register<IMatriculaService>(() => new MatriculaService(
                container.GetInstance<IMatriculaRepository>(),
                container.GetInstance<IAlunoRepository>(),
                container.GetInstance<ITurmaRepository>(),
                connectionString
            ), Lifestyle.Scoped);
        }
    }
}