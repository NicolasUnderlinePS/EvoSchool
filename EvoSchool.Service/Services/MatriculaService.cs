using EvoSchool.Domain.Entities;
using EvoSchool.Domain.Exceptions;
using EvoSchool.Domain.Interfaces.Repositories;
using EvoSchool.Service.DTOs.Requests;
using EvoSchool.Service.DTOs.Responses;
using EvoSchool.Service.Interfaces.Services;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EvoSchool.Service.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ITurmaRepository _turmaRepository;

        public MatriculaService(IMatriculaRepository matriculaRepository, IAlunoRepository alunoRepository, ITurmaRepository turmaRepository)
        {
            _matriculaRepository = matriculaRepository ?? throw new ArgumentNullException(nameof(matriculaRepository));
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
            _turmaRepository = turmaRepository ?? throw new ArgumentNullException(nameof(turmaRepository));
        }

        public async Task<MatriculaResponse> AddAsync(InsertMatriculaRequest request)
        {
            if (request == null)
                throw new BusinessRuleException("Dados da matrícula não informados.");

            if (request.AlunoId <= 0 || request.TurmaId <= 0)
                throw new BusinessRuleException("Id de Aluno e Turma devem ser válidos.");

            Aluno aluno = await _alunoRepository.GetByIdAsync(request.AlunoId);

            if (aluno == null)
                throw new NotFoundException($"Aluno com Id {request.AlunoId} não foi encontrado.");

            if (!aluno.Ativo)
                throw new BusinessRuleException("Aluno inativo não pode realizar matrícula.");


            Turma turma = await _turmaRepository.GetByIdAsync(request.TurmaId);

            if (turma == null)
                throw new NotFoundException($"Turma com Id {request.TurmaId} não foi encontrada.");

            if (turma.VagasDisponiveis <= 0)
                throw new BusinessRuleException("Não há vagas disponíveis para a turma informada.");


            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {

                        bool hasRegistered = await _matriculaRepository.AlreadyRegisteredAsync(request.AlunoId, request.TurmaId, connection, transaction);

                        if (hasRegistered)
                            throw new BusinessRuleException("O aluno já está matriculado nesta turma.");

                        bool vacancyDecremented = await _matriculaRepository.DecrementVacancyAsync(request.TurmaId, connection, transaction);

                        if (vacancyDecremented == false)
                            throw new BusinessRuleException("Não foi possível reservar a vaga. Vagas esgotadas no momento da confirmação.");

                        int matriculaId = await _matriculaRepository.AddAsync(request.AlunoId, request.TurmaId, connection, transaction);

                        transaction.Commit();

                        return new MatriculaResponse
                        {
                            Id = matriculaId,
                            AlunoId = request.AlunoId,
                            TurmaId = request.TurmaId,
                            DataMatricula = DateTime.Now.ToString("yyyy-MM-dd")
                        };
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
