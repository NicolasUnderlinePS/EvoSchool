using EvoSchool.Domain.Commons;
using EvoSchool.Domain.Entities;
using System.Threading.Tasks;

namespace EvoSchool.Domain.Interfaces.Repositories
{
    public interface IAlunoRepository
    {
        Task<PaginationModel<Aluno>> GetListAsync(string nome, int pagina, int tamanhoPagina);
        Task<Aluno> GetByIdAsync(int id);
        Task<int> AddAsync(Aluno aluno);
        Task<int> UpdateAsync(Aluno aluno);
        Task DeleteAsync(int id);
    }
}
