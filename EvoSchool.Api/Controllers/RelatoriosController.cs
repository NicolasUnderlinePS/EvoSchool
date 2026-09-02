using EvoSchool.Service.DTOs.Responses;
using EvoSchool.Service.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EvoSchool.Api.Controllers
{
    [RoutePrefix("api/relatorios")]
    public class RelatoriosController : ApiController
    {
        private readonly IRelatorioService _relatorioService;

        public RelatoriosController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService ?? throw new ArgumentNullException(nameof(relatorioService));
        }

        [HttpGet]
        [Route("alunos-por-turma")]
        public async Task<IHttpActionResult> GetStudentsPerClass()
        {
            return Ok(await _relatorioService.ListStudentsPerClassAsync());
        }
    }
}