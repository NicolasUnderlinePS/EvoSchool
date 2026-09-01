using EvoSchool.Domain.Entities;
using EvoSchool.Service.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoSchool.Domain.Interfaces.Repositories
{
    public interface IAlunoRepository
    {
        Task<PaginationDefaultResponse<Aluno>> GetListAsync(string nome, int pagina, int tamanhoPagina);
        Task<Aluno> GetByIdAsync(int id);
        Task<int> AddAsync(Aluno aluno);
        Task<int> UpdateAsync(Aluno aluno);
        Task DeleteAsync(int id);
    }
}
