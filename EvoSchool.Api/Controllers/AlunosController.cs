using EvoSchool.Service.DTOs.Requests;
using EvoSchool.Service.DTOs.Responses;
using EvoSchool.Service.Interfaces.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace EvoSchool.Api.Controllers
{
    [RoutePrefix("api/alunos")]
    public class AlunosController : ApiController
    {
        private readonly IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get([FromUri] string nome = null, [FromUri] int pagina = 1, [FromUri] int tamanhoPagina = 10)
        {
            var resultado = await _alunoService.GetListAsync(nome, pagina, tamanhoPagina);
            return Ok(resultado);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetAlunoById")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var aluno = await _alunoService.GetByIdAsync(id);
            return Ok(aluno);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] InsertAlunoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AlunoResponse novoAluno = await _alunoService.AddAsync(request);

            return CreatedAtRoute("GetAlunoById", new { id = novoAluno.Id }, novoAluno);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] UpdateAlunoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _alunoService.UpdateAsync(id, request);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            await _alunoService.DeleteAsync(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}