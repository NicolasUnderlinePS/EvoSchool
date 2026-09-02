using EvoSchool.Service.DTOs.Requests;
using EvoSchool.Service.DTOs.Responses;
using EvoSchool.Service.Interfaces.Services;
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
            return Ok(await _alunoService.GetListAsync(nome, pagina, tamanhoPagina));
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetAlunoById")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            return Ok(await _alunoService.GetByIdAsync(id));
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] InsertAlunoRequest request)
        {
            if (request == null)
                return BadRequest("O corpo da requisição não pode ser nulo.");

            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            AlunoResponse newStudent = await _alunoService.AddAsync(request);

            return CreatedAtRoute("GetAlunoById", new { id = newStudent.Id }, newStudent);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] UpdateAlunoRequest request)
        {
            if (request == null)
                return BadRequest("O corpo da requisição não pode ser nulo.");

            if (ModelState.IsValid == false)
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