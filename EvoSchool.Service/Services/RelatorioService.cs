using EvoSchool.Domain.Exceptions;
using EvoSchool.Domain.Interfaces.Repositories;
using EvoSchool.Domain.Models;
using EvoSchool.Service.DTOs.Responses;
using EvoSchool.Service.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvoSchool.Service.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IRelatorioRepository _relatorioRepository;

        public RelatorioService(IRelatorioRepository relatorioRepository)
        {
            _relatorioRepository = relatorioRepository;
        }

        public async Task<List<AlunosTurmaResponse>> ListStudentsPerClassAsync()
        {
            List<AlunosTurmaModel> listReturned = await _relatorioRepository.ListStudentsPerClassAsync();

            if (listReturned == null || !listReturned.Any())
                throw new NotFoundException("Nenhum dado encontrado para o relatório de alunos por turma.");

            return listReturned?.ConvertAll(MapToResponse);
        }


        private static AlunosTurmaResponse MapToResponse(AlunosTurmaModel model)
        {
            if (model == null) return null;

            return new AlunosTurmaResponse
            {
                TurmaId = model.TurmaId,
                NomeTurma = model.NomeTurma,
                TotalMatriculados = model.TotalMatriculados,
                VagasRestantes = model.VagasRestantes
            };
        }
    }
}
