using Dapper;
using EvoSchool.Domain.Entities;
using EvoSchool.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvoSchool.Data.Repositories
{
    public class TurmaRepository : _BaseRepository, ITurmaRepository
    {
        public TurmaRepository(string connectionString) : base(connectionString) { }

        public async Task<List<Turma>> ListAsync()
        {
            const string sql = @"
                SELECT 
                    Id, 
                    Nome, 
                    VagasDisponiveis 
                FROM Turma;
            ";

            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<Turma>(sql)).ToList();
            }
        }

        public async Task<Turma> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT 
                    Id, 
                    Nome, 
                    VagasDisponiveis 
                FROM Turma WHERE Id = @Id;
            ";

            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<Turma>(sql, new { Id = id })).FirstOrDefault();
            }
        }

        public async Task<List<Turma>> ListOpenClassVacanciesAsync()
        {
            const string sql = @"
                SELECT 
                    Id, 
                    Nome, 
                    VagasDisponiveis 
                FROM Turma
                WHERE VagasDisponiveis > 0;
            ";

            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<Turma>(sql)).ToList();
            }
        }
    }
}
