using Dapper;
using EvoSchool.Domain.Interfaces.Repositories;
using System.Data;
using System.Threading.Tasks;

namespace EvoSchool.Data.Repositories
{
    internal class MatriculaRepository : _BaseRepository, IMatriculaRepository
    {
        public MatriculaRepository(string connectionString) : base(connectionString) { }

        public async Task<int> AddAsync(int alunoId, int turmaId, IDbConnection connection, IDbTransaction transaction)
        {
            const string sql = @"
                INSERT INTO Matricula (AlunoId, TurmaId, DataMatricula) 
                VALUES (@AlunoId, @TurmaId, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            return await connection.ExecuteScalarAsync<int>(sql,new { AlunoId = alunoId, TurmaId = turmaId }, transaction);
        }

        public async Task<bool> AlreadyRegisteredAsync(int alunoId, int turmaId, IDbConnection connection, IDbTransaction transaction)
        {
            const string sql = @"
                SELECT COUNT(1) 
                FROM Matricula 
                WHERE AlunoId = @AlunoId AND TurmaId = @TurmaId;
            ";

            int rowReturned = await connection.ExecuteScalarAsync<int>(sql, new { AlunoId = alunoId, TurmaId = turmaId },transaction);

            return rowReturned > 0;
        }

        public async Task<bool> DecrementVacancyAsync(int turmaId, IDbConnection connection, IDbTransaction transaction)
        {
            const string sql = @"
                UPDATE Turma 
                SET VagasDisponiveis = VagasDisponiveis - 1 
                WHERE Id = @TurmaId AND VagasDisponiveis > 0;
            ";

            int rowReturned = await connection.ExecuteAsync(sql,new { TurmaId = turmaId }, transaction);

            return rowReturned > 0;
        }
    }
}
