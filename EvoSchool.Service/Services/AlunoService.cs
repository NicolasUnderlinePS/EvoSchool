using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoSchool.Domain.Commons;
using EvoSchool.Domain.Entities;
using EvoSchool.Domain.Exceptions;
using EvoSchool.Domain.Interfaces.Repositories;
using EvoSchool.Service.DTOs.Requests;
using EvoSchool.Service.DTOs.Responses;
using EvoSchool.Service.Interfaces.Services;

namespace EvoSchool.Service.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository ?? throw new ArgumentNullException(nameof(alunoRepository));
        }

        public async Task<AlunoResponse> AddAsync(InsertAlunoRequest request)
        {
            if (request == null)
                throw new BusinessRuleException("Dados do aluno não informados.");

            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new BusinessRuleException("O nome do aluno é obrigatório.");

            Aluno aluno = new Aluno
            {
                Nome = request.Nome.Trim(),
                Ativo = request.Ativo,
                Email = request.Email?.Trim(),
                DataNascimento = request.DataNascimento,
                DataCadastro = DateTime.Now
            };

            aluno.Id = await _alunoRepository.AddAsync(aluno);

            return MapToResponse(aluno);
        }

        public async Task<AlunoResponse> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new BusinessRuleException("ID do aluno inválido.");

            Aluno aluno = await _alunoRepository.GetByIdAsync(id);
            if (aluno == null)
                throw new NotFoundException($"Aluno com ID {id} não foi encontrado.");

            return MapToResponse(aluno);
        }

        public async Task<PaginationModel<AlunoResponse>> GetListAsync(string nome, int pagina, int tamanhoPagina)
        {
            pagina = pagina < 1 ? 1 : pagina;
            tamanhoPagina = tamanhoPagina < 1 ? 10 : Math.Min(tamanhoPagina, 100);

            PaginationModel<Aluno> listReturned = await _alunoRepository.GetListAsync(nome?.Trim(), pagina, tamanhoPagina);

            if (listReturned == null || listReturned.Itens == null)
                throw new NotFoundException("Nenhum aluno encontrado.");

            List<AlunoResponse> itensResponse = listReturned.Itens.Select(MapToResponse).ToList();

            return new PaginationModel<AlunoResponse>(
                itensResponse,
                listReturned.TotalItens,
                listReturned.PaginaAtual,
                listReturned.TamanhoPagina
            );
        }

        public async Task UpdateAsync(int id, UpdateAlunoRequest request)
        {
            if (id <= 0)
                throw new BusinessRuleException("ID do aluno inválido.");

            if (request == null)
                throw new BusinessRuleException("Dados de atualização não fornecidos.");

            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new BusinessRuleException("O nome do aluno é obrigatório.");

            Aluno student = await _alunoRepository.GetByIdAsync(id);
            if (student == null)
                throw new NotFoundException($"Aluno com ID {id} não foi encontrado para atualização.");

            student.Nome = request.Nome.Trim();
            student.Email = request.Email?.Trim();
            student.Ativo = request.Ativo;

            await _alunoRepository.UpdateAsync(student);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new BusinessRuleException("ID do aluno inválido.");

            Aluno student = await _alunoRepository.GetByIdAsync(id);
            if (student == null)
                throw new NotFoundException($"Aluno com ID {id} não foi encontrado para exclusão.");

            await _alunoRepository.DeleteAsync(id);
        }

        private static AlunoResponse MapToResponse(Aluno entity)
        {
            if (entity == null) return null;

            return new AlunoResponse
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Email = entity.Email,
                Ativo = entity.Ativo ? "Ativo" : "Inativo",
                DataCadastro = entity.DataCadastro.ToString("yyyy-MM-dd"),
                DataNascimento = entity.DataNascimento.ToString("yyyy-MM-dd")
            };
        }
    }
}