using Dapper;
using EvoSchool.Domain.Interfaces.Repositories;
using EvoSchool.Service.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvoSchool.Data.Repositories
{
    internal class RelatorioRepository : _BaseRepository, IRelatorioRepository
    {
        public RelatorioRepository(string connectionString) : base(connectionString) { }

        public async Task<List<AlunosPorTurmaResponse>> ListStudentsPerClassAsync()
        {
            const string sql = @"
                SELECT 
                    t.Id AS TurmaId,
                    t.Nome AS NomeTurma,
                    COUNT(m.Id) AS TotalMatriculados,
                    t.VagasDisponiveis AS VagasRestantes
                FROM Turma t
                LEFT JOIN Matricula m ON m.TurmaId = t.Id
                GROUP BY t.Id, t.Nome, t.VagasDisponiveis;
            ";

            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<AlunosPorTurmaResponse>(sql)).ToList();
            }
        }
    }
}
