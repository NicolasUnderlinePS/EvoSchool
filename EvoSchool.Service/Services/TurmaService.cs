using EvoSchool.Domain.Entities;
using EvoSchool.Domain.Exceptions;
using EvoSchool.Domain.Interfaces.Repositories;
using EvoSchool.Service.DTOs.Responses;
using EvoSchool.Service.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvoSchool.Service.Services
{
    public class TurmaService : ITurmaService
    {
        private readonly ITurmaRepository _turmaRepository;

        public TurmaService(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository;
        }

        public async Task<List<TurmaResponse>> GetListAsync()
        {
            List<Turma> turmas = await _turmaRepository.ListAsync();

            if (turmas == null || !turmas.Any())
                throw new NotFoundException("Nenhuma turma foi encontrada.");

            return turmas.Select(MapToResponse).ToList();
        }

        private static TurmaResponse MapToResponse(Turma turma)
        {
            if (turma == null) return null;

            return new TurmaResponse
            {
                Nome = turma.Nome,
                VagasDisponiveis = turma.VagasDisponiveis
            };
        }
    }
}
