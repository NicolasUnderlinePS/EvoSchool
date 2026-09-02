using EvoSchool.Service.DTOs.Requests;
using EvoSchool.Service.DTOs.Responses;
using EvoSchool.Service.Interfaces.Services;
using EvoSchool.Service.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EvoSchool.Api.Controllers
{
    [RoutePrefix("api/matriculas")]
    public class MatriculasController : ApiController
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculasController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] InsertMatriculaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            MatriculaResponse novoAluno = await _matriculaService.AddAsync(request);

            return CreatedAtRoute("GetAlunoById", new { id = novoAluno.Id }, novoAluno);
        }

    }
}