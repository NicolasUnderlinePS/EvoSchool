using EvoSchool.Domain.Commons;
using EvoSchool.Domain.Entities;
using EvoSchool.Domain.Exceptions;
using EvoSchool.Domain.Interfaces.Repositories;
using EvoSchool.Service.DTOs.Requests;
using EvoSchool.Service.DTOs.Responses;
using EvoSchool.Service.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvoSchool.Service.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
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
                Ativo = true,
                Email = request.Email.Trim(),
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

            PaginationModel<Aluno> resultadoPaginado = await _alunoRepository.GetListAsync(nome, pagina, tamanhoPagina);

            List<AlunoResponse> itensResponse = resultadoPaginado.Itens.Select(MapToResponse).ToList();

            return new PaginationModel<AlunoResponse>(
                itensResponse,
                resultadoPaginado.TotalItens,
                resultadoPaginado.PaginaAtual,
                resultadoPaginado.TamanhoPagina
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

            Aluno alunoExistente = await _alunoRepository.GetByIdAsync(id);

            if (alunoExistente == null)
                throw new NotFoundException($"Aluno com ID {id} não foi encontrado para atualização.");

            alunoExistente.Nome = request.Nome.Trim();
            alunoExistente.Email = request.Email.Trim();
            alunoExistente.Ativo = request.Ativo;

            await _alunoRepository.UpdateAsync(alunoExistente);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new BusinessRuleException("ID do aluno inválido.");

            Aluno alunoExistente = await _alunoRepository.GetByIdAsync(id);
            if (alunoExistente == null)
                throw new NotFoundException($"Aluno com ID {id} não foi encontrado para exclusão.");

            await _alunoRepository.DeleteAsync(id);
        }

        private static AlunoResponse MapToResponse(Aluno aluno)
        {
            if (aluno == null) return null;

            return new AlunoResponse
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Ativo = aluno.Ativo ? "Ativo" : "Inativo",
                DataCadastro = aluno.DataCadastro.ToString("dd-MM-yyyy"),
                DataNascimento = aluno.DataNascimento.ToString("dd-MM-yyyy")
            };
        }
    }
}
