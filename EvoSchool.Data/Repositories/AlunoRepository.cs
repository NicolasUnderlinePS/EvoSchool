using Dapper;
using EvoSchool.Domain.Entities;
using EvoSchool.Domain.Interfaces.Repositories;
using EvoSchool.Service.DTOs.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvoSchool.Data.Repositories
{
    public class AlunoRepository : _BaseRepository, IAlunoRepository
    {
        public AlunoRepository(string connectionString) : base(connectionString) { }

        public async Task<Aluno> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT 
	                Id,
	                Nome,
	                Email,
	                DataNascimento,
	                Ativo,
	                DataCadastro
                FROM Aluno WHERE Id = @Id;
            ";

            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Aluno>(sql, new { Id = id });
            }
        }

        public async Task<int> UpdateAsync(Aluno aluno)
        {
            const string sql = @"
                UPDATE Aluno 
                SET Nome = @Nome, 
                    Email = @Email, 
                    Ativo = @Ativo 
                WHERE Id = @Id;";

            using (var connection = CreateConnection())
            {
                return await connection.ExecuteAsync(sql, aluno);
            }
        }

        public async Task<int> AddAsync(Aluno aluno)
        {
            const string sql = @"
                INSERT INTO Aluno (Nome, Email, DataNascimento, Ativo, DataCadastro)
                VALUES (@Nome, @Email, @DataNascimento, @Ativo, GETDATE())
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var connection = CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(sql, aluno);
            }
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = @"
                UPDATE Aluno 
                SET Ativo = 0 
                WHERE Id = @Id;";

            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<PaginationDefaultResponse<Aluno>> GetListAsync(string nome, int pagina, int tamanhoPagina)
        {
            int paginaAtual = pagina < 1 ? 1 : pagina;
            int tamanho = tamanhoPagina < 1 ? 10 : tamanhoPagina;
            int offset = (paginaAtual - 1) * tamanho;

            const string sql = @"
                SELECT COUNT(1) 
                FROM Aluno 
                WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%');

                SELECT 
                    Id, 
                    Nome, 
                    Ativo 
                FROM Aluno 
                WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%')
                ORDER BY Nome ASC
                OFFSET @Offset ROWS 
                FETCH NEXT @TamanhoPagina ROWS ONLY;
            ";

            using (var connection = CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(
                sql, new {
                    Nome = string.IsNullOrWhiteSpace(nome) ? null : nome,
                    Offset = offset,
                    TamanhoPagina = tamanho
                })
            )
            {
                int totalItens = await multi.ReadSingleAsync<int>();
                List<Aluno> itens = (await multi.ReadAsync<Aluno>()).ToList();

                return new PaginationDefaultResponse<Aluno>(itens, totalItens, paginaAtual, tamanho);
            }
        }
    }
}
